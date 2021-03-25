using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentFiles
{
    public class CreateOrEditAttachmentFileModalViewModel
    {
       public CreateOrEditAttachmentFileDto AttachmentFile { get; set; }

	   		public string AttachmentTypeArName { get; set;}


       public List<AttachmentFileAttachmentTypeLookupTableDto> AttachmentFileAttachmentTypeList { get; set;}


	   public bool IsEditMode => AttachmentFile.Id.HasValue;
    }
}