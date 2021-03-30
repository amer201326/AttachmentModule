using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Persons.Exporting
{
    public class DiseasesExcelExporter : NpoiExcelExporterBase, IDiseasesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiseasesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiseaseForViewDto> diseases)
        {
            return CreateExcelPackage(
                "Diseases.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Diseases"));

                    AddHeader(
                        sheet,
                        L("name"),
                        (L("Person")) + L("name")
                        );

                    AddObjects(
                        sheet, 2, diseases,
                        _ => _.Disease.name,
                        _ => _.Personname
                        );

					
					
                });
        }
    }
}
