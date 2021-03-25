using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class GetAttachmentEntityTypeForEditOutput
    {
		public CreateOrEditAttachmentEntityTypeDto AttachmentEntityType { get; set; }

		public string AttachmentEntityTypeArName { get; set;}


    }
}