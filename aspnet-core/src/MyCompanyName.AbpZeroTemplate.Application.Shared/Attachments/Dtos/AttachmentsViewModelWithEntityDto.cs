using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Test.Dtos
{
    public class AttachmentsViewModelWithEntityDto<T> : EntityDto<T>
    {
        public List<UploadFilesInput> Attachments = new List<UploadFilesInput>();
        public List<UploadFilesInput> AttachmentsToDelete { get; set; }

    }
}
