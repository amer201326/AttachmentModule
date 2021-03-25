using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class GetAllAttachmentTypesForExcelInput
    {
		public string Filter { get; set; }

		public string ArNameFilter { get; set; }

		public string EnNameFilter { get; set; }

		public int? MaxMaxSizeFilter { get; set; }
		public int? MinMaxSizeFilter { get; set; }

		public string AllowedExtensionsFilter { get; set; }

		public int? MaxMaxAttachmentsFilter { get; set; }
		public int? MinMaxAttachmentsFilter { get; set; }


		 public string AttachmentEntityTypeArNameFilter { get; set; }

		 
    }
}