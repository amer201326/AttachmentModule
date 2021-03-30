using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;


namespace MyCompanyName.AbpZeroTemplate.Persons
{
    public interface IKkkksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetKkkkForViewDto>> GetAll(GetAllKkkksInput input);

        Task<GetKkkkForViewDto> GetKkkkForView(int id);

		Task<GetKkkkForEditOutput> GetKkkkForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditKkkkDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetKkkksToExcel(GetAllKkkksForExcelInput input);

		
		Task<List<KkkkPersonLookupTableDto>> GetAllPersonForTableDropdown();
		
    }
}