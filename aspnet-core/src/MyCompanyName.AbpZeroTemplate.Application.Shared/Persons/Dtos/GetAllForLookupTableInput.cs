using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Persons.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}