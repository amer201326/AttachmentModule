using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Exporting
{
    public interface IAttachmentTypesExcelExporter
    {
        FileDto ExportToFile(List<GetAttachmentTypeForViewDto> attachmentTypes);
    }
}