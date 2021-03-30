using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Persons.Exporting
{
    public class KkkksExcelExporter : NpoiExcelExporterBase, IKkkksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public KkkksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetKkkkForViewDto> kkkks)
        {
            return CreateExcelPackage(
                "Kkkks.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Kkkks"));

                    AddHeader(
                        sheet,
                        L("name"),
                        (L("Person")) + L("name")
                        );

                    AddObjects(
                        sheet, 2, kkkks,
                        _ => _.Kkkk.name,
                        _ => _.Personname
                        );

					
					
                });
        }
    }
}
