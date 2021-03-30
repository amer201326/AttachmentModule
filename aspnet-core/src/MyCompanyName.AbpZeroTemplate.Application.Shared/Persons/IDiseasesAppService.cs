using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;


namespace MyCompanyName.AbpZeroTemplate.Persons
{
    public interface IDiseasesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDiseaseForViewDto>> GetAll(GetAllDiseasesInput input);

        Task<GetDiseaseForViewDto> GetDiseaseForView(int id);

		Task<GetDiseaseForEditOutput> GetDiseaseForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDiseaseDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDiseasesToExcel(GetAllDiseasesForExcelInput input);

		
		Task<List<DiseasePersonLookupTableDto>> GetAllPersonForTableDropdown();
		
    }
}