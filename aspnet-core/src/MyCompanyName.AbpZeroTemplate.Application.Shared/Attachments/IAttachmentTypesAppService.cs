using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;


namespace MyCompanyName.AbpZeroTemplate.Attachments
{
    public interface IAttachmentTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAttachmentTypeForViewDto>> GetAll(GetAllAttachmentTypesInput input);

        Task<GetAttachmentTypeForViewDto> GetAttachmentTypeForView(int id);

		Task<GetAttachmentTypeForEditOutput> GetAttachmentTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAttachmentTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAttachmentTypesToExcel(GetAllAttachmentTypesForExcelInput input);

		
		Task<List<AttachmentTypeAttachmentEntityTypeLookupTableDto>> GetAllAttachmentEntityTypeForTableDropdown();
		
    }
}