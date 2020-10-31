using System;
namespace Reboost.Shared
{
    public class ExternalLoginModel
    {
        public string provider { get; set; }
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public string returnUrl { get; set; }
    }
}
