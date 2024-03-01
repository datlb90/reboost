using System;
namespace Reboost.Service.ZaloPay
{
    public class ZaloPayRequestModel
    {
        public string userId { get; set; }
        public int amount { get; set; }
        public int submissionId { get; set; }
        public string reviewType { get; set; }
        public int status { get; set; }
        public string ipAddress { get; set; }
        public string feedbackLanguage { get; set; }
        public string specialRequest { get; set; }
    }
}

