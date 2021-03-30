using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Kkkks
{
    public class CreateOrEditKkkkModalViewModel
    {
       public CreateOrEditKkkkDto Kkkk { get; set; }

	   		public string Personname { get; set;}


       public List<KkkkPersonLookupTableDto> KkkkPersonList { get; set;}


	   public bool IsEditMode => Kkkk.Id.HasValue;
    }
}