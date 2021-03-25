using MyCompanyName.AbpZeroTemplate.Attachments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Attachments
{
	[Table("AttachmentEntityTypes")]
    public class AttachmentEntityType : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string ArName { get; set; }
		
		[Required]
		public virtual string EnName { get; set; }
		
		[Required]
		public virtual string Folder { get; set; }
		

		public virtual int? ParentTypeId { get; set; }
		
        [ForeignKey("ParentTypeId")]
		public AttachmentEntityType ParentTypeFk { get; set; }
		
    }
}