using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class GetAllAttachmentFilesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string PhysicalNameFilter { get; set; }

		public string OriginalNameFilter { get; set; }

		public long? MaxSizeFilter { get; set; }
		public long? MinSizeFilter { get; set; }

		public string ObjectIdFilter { get; set; }

		public string PathFilter { get; set; }


		 public string AttachmentTypeArNameFilter { get; set; }

		 
    }
}