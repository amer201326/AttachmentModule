using MyCompanyName.AbpZeroTemplate.Test.Dtos;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.TestEntities
{
    public class CreateOrEditTestEntityModalViewModel
    {
       public CreateOrEditTestEntityDto TestEntity { get; set; }

	   
       
	   public bool IsEditMode => TestEntity.Id.HasValue;
    }
}