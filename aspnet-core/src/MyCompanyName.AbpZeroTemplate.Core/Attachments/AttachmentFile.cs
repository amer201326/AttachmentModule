using MyCompanyName.AbpZeroTemplate.Attachments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Attachments
{
	[Table("AttachmentFiles")]
    public class AttachmentFile : FullAuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string PhysicalName { get; set; }
		
		[Required]
		public virtual string OriginalName { get; set; }
		
		public virtual long Size { get; set; }
		
		[Required]
		public virtual string ObjectId { get; set; }
		
		[Required]
		public virtual string Path { get; set; }
		
		public virtual int Version { get; set; }
		public string FileToken { get; set; }



		public virtual int AttachmentTypeId { get; set; }
		
        [ForeignKey("AttachmentTypeId")]
		public AttachmentType AttachmentTypeFk { get; set; }
		
    }
}