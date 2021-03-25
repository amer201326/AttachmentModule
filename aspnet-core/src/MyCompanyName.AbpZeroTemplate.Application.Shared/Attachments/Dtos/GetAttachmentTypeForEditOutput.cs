using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class GetAttachmentTypeForEditOutput
    {
		public CreateOrEditAttachmentTypeDto AttachmentType { get; set; }

		public string AttachmentEntityTypeArName { get; set;}


    }
}