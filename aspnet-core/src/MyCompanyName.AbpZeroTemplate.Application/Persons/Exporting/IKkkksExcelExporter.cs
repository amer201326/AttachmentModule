using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.Persons.Exporting
{
    public interface IKkkksExcelExporter
    {
        FileDto ExportToFile(List<GetKkkkForViewDto> kkkks);
    }
}