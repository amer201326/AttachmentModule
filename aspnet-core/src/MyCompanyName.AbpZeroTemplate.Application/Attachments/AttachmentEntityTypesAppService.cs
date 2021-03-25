using MyCompanyName.AbpZeroTemplate.Attachments;


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
	[AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes)]
    public class AttachmentEntityTypesAppService : AbpZeroTemplateAppServiceBase, IAttachmentEntityTypesAppService
    {
		 private readonly IRepository<AttachmentEntityType> _attachmentEntityTypeRepository;
		 private readonly IAttachmentEntityTypesExcelExporter _attachmentEntityTypesExcelExporter;
		 private readonly IRepository<AttachmentEntityType,int> _lookup_attachmentEntityTypeRepository;
		 

		  public AttachmentEntityTypesAppService(IRepository<AttachmentEntityType> attachmentEntityTypeRepository, IAttachmentEntityTypesExcelExporter attachmentEntityTypesExcelExporter , IRepository<AttachmentEntityType, int> lookup_attachmentEntityTypeRepository) 
		  {
			_attachmentEntityTypeRepository = attachmentEntityTypeRepository;
			_attachmentEntityTypesExcelExporter = attachmentEntityTypesExcelExporter;
			_lookup_attachmentEntityTypeRepository = lookup_attachmentEntityTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetAttachmentEntityTypeForViewDto>> GetAll(GetAllAttachmentEntityTypesInput input)
         {
			
			var filteredAttachmentEntityTypes = _attachmentEntityTypeRepository.GetAll()
						.Include( e => e.ParentTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.Folder.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter),  e => e.ArName == input.ArNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter),  e => e.EnName == input.EnNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FolderFilter),  e => e.Folder == input.FolderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentEntityTypeArNameFilter), e => e.ParentTypeFk != null && e.ParentTypeFk.ArName == input.AttachmentEntityTypeArNameFilter);

			var pagedAndFilteredAttachmentEntityTypes = filteredAttachmentEntityTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var attachmentEntityTypes = from o in pagedAndFilteredAttachmentEntityTypes
                         join o1 in _lookup_attachmentEntityTypeRepository.GetAll() on o.ParentTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttachmentEntityTypeForViewDto() {
							AttachmentEntityType = new AttachmentEntityTypeDto
							{
                                ArName = o.ArName,
                                EnName = o.EnName,
                                Folder = o.Folder,
                                Id = o.Id
							},
                         	AttachmentEntityTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
						};

            var totalCount = await filteredAttachmentEntityTypes.CountAsync();

            return new PagedResultDto<GetAttachmentEntityTypeForViewDto>(
                totalCount,
                await attachmentEntityTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetAttachmentEntityTypeForViewDto> GetAttachmentEntityTypeForView(int id)
         {
            var attachmentEntityType = await _attachmentEntityTypeRepository.GetAsync(id);

            var output = new GetAttachmentEntityTypeForViewDto { AttachmentEntityType = ObjectMapper.Map<AttachmentEntityTypeDto>(attachmentEntityType) };

		    if (output.AttachmentEntityType.ParentTypeId != null)
            {
                var _lookupAttachmentEntityType = await _lookup_attachmentEntityTypeRepository.FirstOrDefaultAsync((int)output.AttachmentEntityType.ParentTypeId);
                output.AttachmentEntityTypeArName = _lookupAttachmentEntityType?.ArName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Edit)]
		 public async Task<GetAttachmentEntityTypeForEditOutput> GetAttachmentEntityTypeForEdit(EntityDto input)
         {
            var attachmentEntityType = await _attachmentEntityTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAttachmentEntityTypeForEditOutput {AttachmentEntityType = ObjectMapper.Map<CreateOrEditAttachmentEntityTypeDto>(attachmentEntityType)};

		    if (output.AttachmentEntityType.ParentTypeId != null)
            {
                var _lookupAttachmentEntityType = await _lookup_attachmentEntityTypeRepository.FirstOrDefaultAsync((int)output.AttachmentEntityType.ParentTypeId);
                output.AttachmentEntityTypeArName = _lookupAttachmentEntityType?.ArName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAttachmentEntityTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Create)]
		 protected virtual async Task Create(CreateOrEditAttachmentEntityTypeDto input)
         {
            var attachmentEntityType = ObjectMapper.Map<AttachmentEntityType>(input);

			
			if (AbpSession.TenantId != null)
			{
				attachmentEntityType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _attachmentEntityTypeRepository.InsertAsync(attachmentEntityType);
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditAttachmentEntityTypeDto input)
         {
            var attachmentEntityType = await _attachmentEntityTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, attachmentEntityType);
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _attachmentEntityTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAttachmentEntityTypesToExcel(GetAllAttachmentEntityTypesForExcelInput input)
         {
			
			var filteredAttachmentEntityTypes = _attachmentEntityTypeRepository.GetAll()
						.Include( e => e.ParentTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.Folder.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter),  e => e.ArName == input.ArNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter),  e => e.EnName == input.EnNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FolderFilter),  e => e.Folder == input.FolderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentEntityTypeArNameFilter), e => e.ParentTypeFk != null && e.ParentTypeFk.ArName == input.AttachmentEntityTypeArNameFilter);

			var query = (from o in filteredAttachmentEntityTypes
                         join o1 in _lookup_attachmentEntityTypeRepository.GetAll() on o.ParentTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttachmentEntityTypeForViewDto() { 
							AttachmentEntityType = new AttachmentEntityTypeDto
							{
                                ArName = o.ArName,
                                EnName = o.EnName,
                                Folder = o.Folder,
                                Id = o.Id
							},
                         	AttachmentEntityTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
						 });


            var attachmentEntityTypeListDtos = await query.ToListAsync();

            return _attachmentEntityTypesExcelExporter.ExportToFile(attachmentEntityTypeListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes)]
         public async Task<PagedResultDto<AttachmentEntityTypeAttachmentEntityTypeLookupTableDto>> GetAllAttachmentEntityTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_attachmentEntityTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.ArName != null && e.ArName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var attachmentEntityTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<AttachmentEntityTypeAttachmentEntityTypeLookupTableDto>();
			foreach(var attachmentEntityType in attachmentEntityTypeList){
				lookupTableDtoList.Add(new AttachmentEntityTypeAttachmentEntityTypeLookupTableDto
				{
					Id = attachmentEntityType.Id,
					DisplayName = attachmentEntityType.ArName?.ToString()
				});
			}

            return new PagedResultDto<AttachmentEntityTypeAttachmentEntityTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}