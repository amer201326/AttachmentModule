using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Exporting
{
    public class AttachmentEntityTypesExcelExporter : NpoiExcelExporterBase, IAttachmentEntityTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentEntityTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentEntityTypeForViewDto> attachmentEntityTypes)
        {
            return CreateExcelPackage(
                "AttachmentEntityTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("AttachmentEntityTypes"));

                    AddHeader(
                        sheet,
                        L("ArName"),
                        L("EnName"),
                        L("Folder"),
                        (L("AttachmentEntityType")) + L("ArName")
                        );

                    AddObjects(
                        sheet, 2, attachmentEntityTypes,
                        _ => _.AttachmentEntityType.ArName,
                        _ => _.AttachmentEntityType.EnName,
                        _ => _.AttachmentEntityType.Folder,
                        _ => _.AttachmentEntityTypeArName
                        );

					
					
                });
        }
    }
}
