using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Test.Exporting
{
    public class TestEntitiesExcelExporter : NpoiExcelExporterBase, ITestEntitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TestEntitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTestEntityForViewDto> testEntities)
        {
            return CreateExcelPackage(
                "TestEntities.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("TestEntities"));

                    AddHeader(
                        sheet,
                        L("ArName")
                        );

                    AddObjects(
                        sheet, 2, testEntities,
                        _ => _.TestEntity.ArName
                        );

					
					
                });
        }
    }
}
