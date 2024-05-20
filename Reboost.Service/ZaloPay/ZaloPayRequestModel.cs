using System;
namespace Reboost.Service.ZaloPay
{
    public class ZaloPayRequestModel
    {
        public string userId { get; set; }
        public int amount { get; set; }
        public int planId { get; set; }
        public int duration { get; set; }
        public string subscriptionType { get; set; }
        public int proratedAmount { get; set; }
        public string ipAddress { get; set; }
    }
}

