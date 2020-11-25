using Microsoft.AspNetCore.Http;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class RaterRequestModel : Raters
    {
        public List<IFormFile> UploadedFiles { get; set; }
    }
    public class RaterResponseModel : Raters
    {
        public Dictionary<string, bool> ApplyTo { get; set; }
    }
}
