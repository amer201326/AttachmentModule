using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;


namespace MyCompanyName.AbpZeroTemplate.Attachments
{
    public interface IAttachmentEntityTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAttachmentEntityTypeForViewDto>> GetAll(GetAllAttachmentEntityTypesInput input);

        Task<GetAttachmentEntityTypeForViewDto> GetAttachmentEntityTypeForView(int id);

		Task<GetAttachmentEntityTypeForEditOutput> GetAttachmentEntityTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAttachmentEntityTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAttachmentEntityTypesToExcel(GetAllAttachmentEntityTypesForExcelInput input);

		
		Task<PagedResultDto<AttachmentEntityTypeAttachmentEntityTypeLookupTableDto>> GetAllAttachmentEntityTypeForLookupTable(GetAllForLookupTableInput input);
		
    }
}