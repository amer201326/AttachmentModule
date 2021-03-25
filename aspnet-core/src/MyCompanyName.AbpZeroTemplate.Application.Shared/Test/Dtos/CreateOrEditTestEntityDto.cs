
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Test.Dtos
{
    public class CreateOrEditTestEntityDto : EntityDto<int?>
    {

		[Required]
		public string ArName { get; set; }
		
		

    }
}