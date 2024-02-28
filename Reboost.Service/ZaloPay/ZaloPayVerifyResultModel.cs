using System;
namespace Reboost.Service.ZaloPay
{
    public class ZaloPayVerifyResultModel
    {
        public string userId { get; set; }
        public string appid { get; set; }
        public string apptransid { get; set; }
        public string pmcid { get; set; }
        public string bankcode { get; set; }
        public string amount { get; set; }
        public string discountamount { get; set; }
        public string status { get; set; }
        public string checksum { get; set; }
    }
}

