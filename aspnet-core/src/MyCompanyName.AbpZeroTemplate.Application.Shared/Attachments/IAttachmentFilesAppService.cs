using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;

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
        Task CheckAttachment<T>(int AttachmentTypeId, string ObjectID, AttachmentsViewModelWithEntityDto<T> input, AttachmentUploadType type);
        Task<List<UploadFilesInput>> GetAttacments(string ObjectID, int AttachmentEntityTypeId);
        Task<AttachmentMemoryStream> DownloadAttaachment(string FileToken);
    }
}