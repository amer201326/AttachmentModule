using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Test
{
	[Table("TestEntities")]
    public class TestEntity : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string ArName { get; set; }
		

    }
}