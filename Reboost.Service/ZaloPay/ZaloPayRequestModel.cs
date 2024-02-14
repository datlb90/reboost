using System;
namespace Reboost.Service.ZaloPay
{
    public class ZaloPayRequestModel
    {
        public string userId { get; set; }
        public int orderId { get; set; }
        public int amount { get; set; }
        public string returnUrl { get; set; }
    }
}

