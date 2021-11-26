using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class ContactRequestModel
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
        public List<IFormFile>? UploadedFiles { get; set; }
    }
}
