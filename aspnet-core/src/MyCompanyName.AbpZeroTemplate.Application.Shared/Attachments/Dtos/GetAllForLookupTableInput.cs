using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}