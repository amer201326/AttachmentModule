using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Exporting
{
    public class AttachmentTypesExcelExporter : NpoiExcelExporterBase, IAttachmentTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentTypeForViewDto> attachmentTypes)
        {
            return CreateExcelPackage(
                "AttachmentTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("AttachmentTypes"));

                    AddHeader(
                        sheet,
                        L("ArName"),
                        L("EnName"),
                        L("MaxSize"),
                        L("AllowedExtensions"),
                        L("MaxAttachments"),
                        (L("AttachmentEntityType")) + L("ArName")
                        );

                    AddObjects(
                        sheet, 2, attachmentTypes,
                        _ => _.AttachmentType.ArName,
                        _ => _.AttachmentType.EnName,
                        _ => _.AttachmentType.MaxSize,
                        _ => _.AttachmentType.AllowedExtensions,
                        _ => _.AttachmentType.MaxAttachments,
                        _ => _.AttachmentEntityTypeArName
                        );

					
					
                });
        }
    }
}
