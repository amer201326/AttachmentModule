
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class CreateOrEditAttachmentTypeDto : EntityDto<int?>
    {

		[Required]
		public string ArName { get; set; }
		
		
		[Required]
		public string EnName { get; set; }
		
		
		public int MaxSize { get; set; }
		
		
		public string AllowedExtensions { get; set; }
		
		
		public int MaxAttachments { get; set; }
		
		
		 public int EntityTypeId { get; set; }
		 
		 
    }
}