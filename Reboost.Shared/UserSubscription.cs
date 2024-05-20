using System;
namespace Reboost.Shared
{
    public class UserSubscription
    {
        public string userId { get; set; }
        public int planId { get; set; }
        public int duration { get; set; }
        public int proratedAmount { get; set; }
    }
}

