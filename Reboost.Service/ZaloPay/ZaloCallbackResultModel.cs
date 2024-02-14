using System;
namespace Reboost.Service.ZaloPay
{
    public class ZaloCallbackResultModel
    {
        public int return_code { get; set; }
        public string return_message { get; set; }
    }
}

