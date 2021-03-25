
using System;
using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class AttachmentTypeDto : EntityDto
    {
		public string ArName { get; set; }

		public string EnName { get; set; }

		public int MaxSize { get; set; }

		public string AllowedExtensions { get; set; }

		public int MaxAttachments { get; set; }


		 public int EntityTypeId { get; set; }

		 
    }
}