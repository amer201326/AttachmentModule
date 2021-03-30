using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class UploadFilesOutput : ErrorInfo
    {

        public string FileName { get; set; }

        public string FileToken { get; set; }

       
        public UploadFilesOutput()
        {

        }

        public UploadFilesOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }

    }
}
