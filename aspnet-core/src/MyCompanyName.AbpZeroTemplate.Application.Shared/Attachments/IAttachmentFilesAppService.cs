using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;


namespace MyCompanyName.AbpZeroTemplate.Attachments
{
    public interface IAttachmentFilesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAttachmentFileForViewDto>> GetAll(GetAllAttachmentFilesInput input);

        Task<GetAttachmentFileForViewDto> GetAttachmentFileForView(long id);

		Task<GetAttachmentFileForEditOutput> GetAttachmentFileForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditAttachmentFileDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetAttachmentFilesToExcel(GetAllAttachmentFilesForExcelInput input);

		
		Task<List<AttachmentFileAttachmentTypeLookupTableDto>> GetAllAttachmentTypeForTableDropdown();
		
    }
}