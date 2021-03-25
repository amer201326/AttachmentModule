using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Exporting
{
    public class AttachmentFilesExcelExporter : NpoiExcelExporterBase, IAttachmentFilesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentFilesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentFileForViewDto> attachmentFiles)
        {
            return CreateExcelPackage(
                "AttachmentFiles.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("AttachmentFiles"));

                    AddHeader(
                        sheet,
                        L("PhysicalName"),
                        L("OriginalName"),
                        L("Size"),
                        L("ObjectId"),
                        L("Path"),
                        (L("AttachmentType")) + L("ArName")
                        );

                    AddObjects(
                        sheet, 2, attachmentFiles,
                        _ => _.AttachmentFile.PhysicalName,
                        _ => _.AttachmentFile.OriginalName,
                        _ => _.AttachmentFile.Size,
                        _ => _.AttachmentFile.ObjectId,
                        _ => _.AttachmentFile.Path,
                        _ => _.AttachmentTypeArName
                        );

					
					
                });
        }
    }
}
