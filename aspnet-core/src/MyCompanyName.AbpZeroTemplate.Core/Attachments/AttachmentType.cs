using MyCompanyName.AbpZeroTemplate.Attachments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Attachments
{
	[Table("AttachmentTypes")]
    public class AttachmentType : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string ArName { get; set; }
		
		[Required]
		public virtual string EnName { get; set; }
		
		public virtual int MaxSize { get; set; }
		
		public virtual string AllowedExtensions { get; set; }
		
		public virtual int MaxAttachments { get; set; }
		

		public virtual int EntityTypeId { get; set; }
		
        [ForeignKey("EntityTypeId")]
		public AttachmentEntityType EntityTypeFk { get; set; }
		
    }
}