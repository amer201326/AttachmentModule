using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Test.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}