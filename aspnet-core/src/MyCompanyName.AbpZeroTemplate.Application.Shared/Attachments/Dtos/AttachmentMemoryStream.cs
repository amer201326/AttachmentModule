using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.Attachments.Dtos
{
    public class AttachmentMemoryStream
    {
        public MemoryStream Memory { get; set; }
        public string ApplicationType { get; set; }
        public string FileName { get; set; }
    }
}
