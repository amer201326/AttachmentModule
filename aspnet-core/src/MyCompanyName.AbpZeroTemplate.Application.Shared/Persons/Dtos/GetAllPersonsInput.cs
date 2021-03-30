using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.Persons.Dtos
{
    public class GetAllPersonsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string nameFilter { get; set; }



    }
}