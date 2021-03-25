using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.Test.Exporting
{
    public interface ITestEntitiesExcelExporter
    {
        FileDto ExportToFile(List<GetTestEntityForViewDto> testEntities);
    }
}