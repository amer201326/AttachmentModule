using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentTypes
{
    public class CreateOrEditAttachmentTypeModalViewModel
    {
       public CreateOrEditAttachmentTypeDto AttachmentType { get; set; }

	   		public string AttachmentEntityTypeArName { get; set;}


       public List<AttachmentTypeAttachmentEntityTypeLookupTableDto> AttachmentTypeAttachmentEntityTypeList { get; set;}


	   public bool IsEditMode => AttachmentType.Id.HasValue;
    }
}