﻿
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;

namespace MyCompanyName.AbpZeroTemplate.Persons.Dtos
{
    public class CreateOrEditPersonDto : AttachmentsViewModelWithEntityDto<int?>
    {

		public string name { get; set; }
		
		

    }
}