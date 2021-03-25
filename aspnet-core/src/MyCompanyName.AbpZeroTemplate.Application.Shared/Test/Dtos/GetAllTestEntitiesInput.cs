using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.Test.Dtos
{
    public class GetAllTestEntitiesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ArNameFilter { get; set; }



    }
}