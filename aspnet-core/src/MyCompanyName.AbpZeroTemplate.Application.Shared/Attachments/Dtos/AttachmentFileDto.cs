
using System;
using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class AttachmentFileDto : EntityDto<long>
    {
		public string PhysicalName { get; set; }

		public string OriginalName { get; set; }

		public long Size { get; set; }

		public string ObjectId { get; set; }

		public string Path { get; set; }


		 public int AttachmentTypeId { get; set; }

		 
    }
}