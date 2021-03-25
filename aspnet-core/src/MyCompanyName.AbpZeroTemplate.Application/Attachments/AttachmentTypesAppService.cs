using MyCompanyName.AbpZeroTemplate.Attachments;
					using System.Collections.Generic;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.Attachments.Exporting;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MyCompanyName.AbpZeroTemplate.Attachments
{
	[AbpAuthorize(AppPermissions.Pages_AttachmentTypes)]
    public class AttachmentTypesAppService : AbpZeroTemplateAppServiceBase, IAttachmentTypesAppService
    {
		 private readonly IRepository<AttachmentType> _attachmentTypeRepository;
		 private readonly IAttachmentTypesExcelExporter _attachmentTypesExcelExporter;
		 private readonly IRepository<AttachmentEntityType,int> _lookup_attachmentEntityTypeRepository;
		 

		  public AttachmentTypesAppService(IRepository<AttachmentType> attachmentTypeRepository, IAttachmentTypesExcelExporter attachmentTypesExcelExporter , IRepository<AttachmentEntityType, int> lookup_attachmentEntityTypeRepository) 
		  {
			_attachmentTypeRepository = attachmentTypeRepository;
			_attachmentTypesExcelExporter = attachmentTypesExcelExporter;
			_lookup_attachmentEntityTypeRepository = lookup_attachmentEntityTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetAttachmentTypeForViewDto>> GetAll(GetAllAttachmentTypesInput input)
         {
			
			var filteredAttachmentTypes = _attachmentTypeRepository.GetAll()
						.Include( e => e.EntityTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.AllowedExtensions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter),  e => e.ArName == input.ArNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter),  e => e.EnName == input.EnNameFilter)
						.WhereIf(input.MinMaxSizeFilter != null, e => e.MaxSize >= input.MinMaxSizeFilter)
						.WhereIf(input.MaxMaxSizeFilter != null, e => e.MaxSize <= input.MaxMaxSizeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AllowedExtensionsFilter),  e => e.AllowedExtensions == input.AllowedExtensionsFilter)
						.WhereIf(input.MinMaxAttachmentsFilter != null, e => e.MaxAttachments >= input.MinMaxAttachmentsFilter)
						.WhereIf(input.MaxMaxAttachmentsFilter != null, e => e.MaxAttachments <= input.MaxMaxAttachmentsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentEntityTypeArNameFilter), e => e.EntityTypeFk != null && e.EntityTypeFk.ArName == input.AttachmentEntityTypeArNameFilter);

			var pagedAndFilteredAttachmentTypes = filteredAttachmentTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var attachmentTypes = from o in pagedAndFilteredAttachmentTypes
                         join o1 in _lookup_attachmentEntityTypeRepository.GetAll() on o.EntityTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttachmentTypeForViewDto() {
							AttachmentType = new AttachmentTypeDto
							{
                                ArName = o.ArName,
                                EnName = o.EnName,
                                MaxSize = o.MaxSize,
                                AllowedExtensions = o.AllowedExtensions,
                                MaxAttachments = o.MaxAttachments,
                                Id = o.Id
							},
                         	AttachmentEntityTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
						};

            var totalCount = await filteredAttachmentTypes.CountAsync();

            return new PagedResultDto<GetAttachmentTypeForViewDto>(
                totalCount,
                await attachmentTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetAttachmentTypeForViewDto> GetAttachmentTypeForView(int id)
         {
            var attachmentType = await _attachmentTypeRepository.GetAsync(id);

            var output = new GetAttachmentTypeForViewDto { AttachmentType = ObjectMapper.Map<AttachmentTypeDto>(attachmentType) };

		    if (output.AttachmentType.EntityTypeId != null)
            {
                var _lookupAttachmentEntityType = await _lookup_attachmentEntityTypeRepository.FirstOrDefaultAsync((int)output.AttachmentType.EntityTypeId);
                output.AttachmentEntityTypeArName = _lookupAttachmentEntityType?.ArName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Edit)]
		 public async Task<GetAttachmentTypeForEditOutput> GetAttachmentTypeForEdit(EntityDto input)
         {
            var attachmentType = await _attachmentTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAttachmentTypeForEditOutput {AttachmentType = ObjectMapper.Map<CreateOrEditAttachmentTypeDto>(attachmentType)};

		    if (output.AttachmentType.EntityTypeId != null)
            {
                var _lookupAttachmentEntityType = await _lookup_attachmentEntityTypeRepository.FirstOrDefaultAsync((int)output.AttachmentType.EntityTypeId);
                output.AttachmentEntityTypeArName = _lookupAttachmentEntityType?.ArName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAttachmentTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Create)]
		 protected virtual async Task Create(CreateOrEditAttachmentTypeDto input)
         {
            var attachmentType = ObjectMapper.Map<AttachmentType>(input);

			
			if (AbpSession.TenantId != null)
			{
				attachmentType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _attachmentTypeRepository.InsertAsync(attachmentType);
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditAttachmentTypeDto input)
         {
            var attachmentType = await _attachmentTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, attachmentType);
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _attachmentTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAttachmentTypesToExcel(GetAllAttachmentTypesForExcelInput input)
         {
			
			var filteredAttachmentTypes = _attachmentTypeRepository.GetAll()
						.Include( e => e.EntityTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.AllowedExtensions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter),  e => e.ArName == input.ArNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter),  e => e.EnName == input.EnNameFilter)
						.WhereIf(input.MinMaxSizeFilter != null, e => e.MaxSize >= input.MinMaxSizeFilter)
						.WhereIf(input.MaxMaxSizeFilter != null, e => e.MaxSize <= input.MaxMaxSizeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AllowedExtensionsFilter),  e => e.AllowedExtensions == input.AllowedExtensionsFilter)
						.WhereIf(input.MinMaxAttachmentsFilter != null, e => e.MaxAttachments >= input.MinMaxAttachmentsFilter)
						.WhereIf(input.MaxMaxAttachmentsFilter != null, e => e.MaxAttachments <= input.MaxMaxAttachmentsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentEntityTypeArNameFilter), e => e.EntityTypeFk != null && e.EntityTypeFk.ArName == input.AttachmentEntityTypeArNameFilter);

			var query = (from o in filteredAttachmentTypes
                         join o1 in _lookup_attachmentEntityTypeRepository.GetAll() on o.EntityTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttachmentTypeForViewDto() { 
							AttachmentType = new AttachmentTypeDto
							{
                                ArName = o.ArName,
                                EnName = o.EnName,
                                MaxSize = o.MaxSize,
                                AllowedExtensions = o.AllowedExtensions,
                                MaxAttachments = o.MaxAttachments,
                                Id = o.Id
							},
                         	AttachmentEntityTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
						 });


            var attachmentTypeListDtos = await query.ToListAsync();

            return _attachmentTypesExcelExporter.ExportToFile(attachmentTypeListDtos);
         }


			[AbpAuthorize(AppPermissions.Pages_AttachmentTypes)]
			public async Task<List<AttachmentTypeAttachmentEntityTypeLookupTableDto>> GetAllAttachmentEntityTypeForTableDropdown()
			{
				return await _lookup_attachmentEntityTypeRepository.GetAll()
					.Select(attachmentEntityType => new AttachmentTypeAttachmentEntityTypeLookupTableDto
					{
						Id = attachmentEntityType.Id,
						DisplayName = attachmentEntityType == null || attachmentEntityType.ArName == null ? "" : attachmentEntityType.ArName.ToString()
					}).ToListAsync();
			}
							
    }
}