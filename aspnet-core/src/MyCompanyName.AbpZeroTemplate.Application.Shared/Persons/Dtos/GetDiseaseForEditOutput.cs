using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Persons.Dtos
{
    public class GetDiseaseForEditOutput
    {
		public CreateOrEditDiseaseDto Disease { get; set; }

		public string Personname { get; set;}


    }
}