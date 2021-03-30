using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using System.Collections.Generic;

using Abp.Extensions;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Diseases
{
    public class CreateOrEditDiseaseModalViewModel
    {
       public CreateOrEditDiseaseDto Disease { get; set; }

	   		public string Personname { get; set;}


       public List<DiseasePersonLookupTableDto> DiseasePersonList { get; set;}


	   public bool IsEditMode => Disease.Id.HasValue;

        public AttachmentTypeDto AttachmentType { get; internal set; }
    }
}