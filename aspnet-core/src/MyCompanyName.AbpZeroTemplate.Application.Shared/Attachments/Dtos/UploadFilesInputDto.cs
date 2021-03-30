using Abp.Application.Services.Dto;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class UploadFilesInputDto
    {

        public string FileName { get; set; }

        public string FileToken { get; set; }
        public byte[] FileByte { get; set; }
        public int Version { get; set; }


    }
}
