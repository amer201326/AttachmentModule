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
	[AbpAuthorize(AppPermissions.Pages_AttachmentFiles)]
    public class AttachmentFilesAppService : AbpZeroTemplateAppServiceBase, IAttachmentFilesAppService
    {
		 private readonly IRepository<AttachmentFile, long> _attachmentFileRepository;
		 private readonly IAttachmentFilesExcelExporter _attachmentFilesExcelExporter;
		 private readonly IRepository<AttachmentType,int> _lookup_attachmentTypeRepository;
		 

		  public AttachmentFilesAppService(IRepository<AttachmentFile, long> attachmentFileRepository, IAttachmentFilesExcelExporter attachmentFilesExcelExporter , IRepository<AttachmentType, int> lookup_attachmentTypeRepository) 
		  {
			_attachmentFileRepository = attachmentFileRepository;
			_attachmentFilesExcelExporter = attachmentFilesExcelExporter;
			_lookup_attachmentTypeRepository = lookup_attachmentTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetAttachmentFileForViewDto>> GetAll(GetAllAttachmentFilesInput input)
         {
			
			var filteredAttachmentFiles = _attachmentFileRepository.GetAll()
						.Include( e => e.AttachmentTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.PhysicalName.Contains(input.Filter) || e.OriginalName.Contains(input.Filter) || e.ObjectId.Contains(input.Filter) || e.Path.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhysicalNameFilter),  e => e.PhysicalName == input.PhysicalNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OriginalNameFilter),  e => e.OriginalName == input.OriginalNameFilter)
						.WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
						.WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ObjectIdFilter),  e => e.ObjectId == input.ObjectIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter),  e => e.Path == input.PathFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeArNameFilter), e => e.AttachmentTypeFk != null && e.AttachmentTypeFk.ArName == input.AttachmentTypeArNameFilter);

			var pagedAndFilteredAttachmentFiles = filteredAttachmentFiles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var attachmentFiles = from o in pagedAndFilteredAttachmentFiles
                         join o1 in _lookup_attachmentTypeRepository.GetAll() on o.AttachmentTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttachmentFileForViewDto() {
							AttachmentFile = new AttachmentFileDto
							{
                                PhysicalName = o.PhysicalName,
                                OriginalName = o.OriginalName,
                                Size = o.Size,
                                ObjectId = o.ObjectId,
                                Path = o.Path,
                                Id = o.Id
							},
                         	AttachmentTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
						};

            var totalCount = await filteredAttachmentFiles.CountAsync();

            return new PagedResultDto<GetAttachmentFileForViewDto>(
                totalCount,
                await attachmentFiles.ToListAsync()
            );
         }
		 
		 public async Task<GetAttachmentFileForViewDto> GetAttachmentFileForView(long id)
         {
            var attachmentFile = await _attachmentFileRepository.GetAsync(id);

            var output = new GetAttachmentFileForViewDto { AttachmentFile = ObjectMapper.Map<AttachmentFileDto>(attachmentFile) };

		    if (output.AttachmentFile.AttachmentTypeId != null)
            {
                var _lookupAttachmentType = await _lookup_attachmentTypeRepository.FirstOrDefaultAsync((int)output.AttachmentFile.AttachmentTypeId);
                output.AttachmentTypeArName = _lookupAttachmentType?.ArName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Edit)]
		 public async Task<GetAttachmentFileForEditOutput> GetAttachmentFileForEdit(EntityDto<long> input)
         {
            var attachmentFile = await _attachmentFileRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAttachmentFileForEditOutput {AttachmentFile = ObjectMapper.Map<CreateOrEditAttachmentFileDto>(attachmentFile)};

		    if (output.AttachmentFile.AttachmentTypeId != null)
            {
                var _lookupAttachmentType = await _lookup_attachmentTypeRepository.FirstOrDefaultAsync((int)output.AttachmentFile.AttachmentTypeId);
                output.AttachmentTypeArName = _lookupAttachmentType?.ArName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAttachmentFileDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Create)]
		 protected virtual async Task Create(CreateOrEditAttachmentFileDto input)
         {
            var attachmentFile = ObjectMapper.Map<AttachmentFile>(input);

			
			if (AbpSession.TenantId != null)
			{
				attachmentFile.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _attachmentFileRepository.InsertAsync(attachmentFile);
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Edit)]
		 protected virtual async Task Update(CreateOrEditAttachmentFileDto input)
         {
            var attachmentFile = await _attachmentFileRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, attachmentFile);
         }

		 [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _attachmentFileRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAttachmentFilesToExcel(GetAllAttachmentFilesForExcelInput input)
         {
			
			var filteredAttachmentFiles = _attachmentFileRepository.GetAll()
						.Include( e => e.AttachmentTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.PhysicalName.Contains(input.Filter) || e.OriginalName.Contains(input.Filter) || e.ObjectId.Contains(input.Filter) || e.Path.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhysicalNameFilter),  e => e.PhysicalName == input.PhysicalNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OriginalNameFilter),  e => e.OriginalName == input.OriginalNameFilter)
						.WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
						.WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ObjectIdFilter),  e => e.ObjectId == input.ObjectIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter),  e => e.Path == input.PathFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeArNameFilter), e => e.AttachmentTypeFk != null && e.AttachmentTypeFk.ArName == input.AttachmentTypeArNameFilter);

			var query = (from o in filteredAttachmentFiles
                         join o1 in _lookup_attachmentTypeRepository.GetAll() on o.AttachmentTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetAttachmentFileForViewDto() { 
							AttachmentFile = new AttachmentFileDto
							{
                                PhysicalName = o.PhysicalName,
                                OriginalName = o.OriginalName,
                                Size = o.Size,
                                ObjectId = o.ObjectId,
                                Path = o.Path,
                                Id = o.Id
							},
                         	AttachmentTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
						 });


            var attachmentFileListDtos = await query.ToListAsync();

            return _attachmentFilesExcelExporter.ExportToFile(attachmentFileListDtos);
         }


			[AbpAuthorize(AppPermissions.Pages_AttachmentFiles)]
			public async Task<List<AttachmentFileAttachmentTypeLookupTableDto>> GetAllAttachmentTypeForTableDropdown()
			{
				return await _lookup_attachmentTypeRepository.GetAll()
					.Select(attachmentType => new AttachmentFileAttachmentTypeLookupTableDto
					{
						Id = attachmentType.Id,
						DisplayName = attachmentType == null || attachmentType.ArName == null ? "" : attachmentType.ArName.ToString()
					}).ToListAsync();
			}
							
    }
}