
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class CreateOrEditAttachmentEntityTypeDto : EntityDto<int?>
    {

		[Required]
		public string ArName { get; set; }
		
		
		[Required]
		public string EnName { get; set; }
		
		
		[Required]
		public string Folder { get; set; }
		
		
		 public int? ParentTypeId { get; set; }
		 
		 
    }
}