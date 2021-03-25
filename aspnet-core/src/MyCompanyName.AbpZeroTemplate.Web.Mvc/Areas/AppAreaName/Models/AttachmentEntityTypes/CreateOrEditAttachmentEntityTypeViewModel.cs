using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentEntityTypes
{
    public class CreateOrEditAttachmentEntityTypeModalViewModel
    {
       public CreateOrEditAttachmentEntityTypeDto AttachmentEntityType { get; set; }

	   		public string AttachmentEntityTypeArName { get; set;}


       
	   public bool IsEditMode => AttachmentEntityType.Id.HasValue;
    }
}