using MyCompanyName.AbpZeroTemplate.Persons;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Persons
{
	[Table("Diseases")]
    public class Disease : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string name { get; set; }
		

		public virtual int PersonId { get; set; }
		
        [ForeignKey("PersonId")]
		public Person PersonFk { get; set; }
		
    }
}