using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class GetAllAttachmentEntityTypesForExcelInput
    {
		public string Filter { get; set; }

		public string ArNameFilter { get; set; }

		public string EnNameFilter { get; set; }

		public string FolderFilter { get; set; }


		 public string AttachmentEntityTypeArNameFilter { get; set; }

		 
    }
}