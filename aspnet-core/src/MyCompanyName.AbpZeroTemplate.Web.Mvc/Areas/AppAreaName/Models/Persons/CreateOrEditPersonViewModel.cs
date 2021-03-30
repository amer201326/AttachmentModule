using MyCompanyName.AbpZeroTemplate.Persons.Dtos;

using Abp.Extensions;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentTypes;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Persons
{
    public class CreateOrEditPersonModalViewModel
    {
        public CreateOrEditPersonDto Person { get; set; }
        public bool IsEditMode => Person.Id.HasValue;
        public AttachmentTypeDto AttachmentType { get; set; }
    }
}