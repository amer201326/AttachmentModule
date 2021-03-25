
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class CreateOrEditAttachmentFileDto : EntityDto<long?>
    {

		[Required]
		public string PhysicalName { get; set; }
		
		
		[Required]
		public string OriginalName { get; set; }
		
		
		public long Size { get; set; }
		
		
		[Required]
		public string ObjectId { get; set; }
		
		
		[Required]
		public string Path { get; set; }
		
		
		 public int AttachmentTypeId { get; set; }
		 
		 
    }
}