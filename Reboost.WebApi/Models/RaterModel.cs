using Microsoft.AspNetCore.Http;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.WebApi.Models
{
    public class RaterModel : Rater
    {
        public List<IFormFile> IELTSCertificatePhotos { get; set; }
        public List<IFormFile> TOEFLCertificatePhotos { get; set; }
        public List<IFormFile> IDCardPhotos { get; set; }
        public string ScoreJSON { get; set; }
    }
}
