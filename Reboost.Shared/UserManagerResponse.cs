using System;
using System.Collections.Generic;

namespace Reboost.Shared
{
    public class UserManagerResponse
    {
       
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public UserLoginModel user { get; set; }

    }

    public class UserLoginModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeAccountId { get; set; }
        public bool IsSubcribed { get; set; }
        public int FreeToken { get; set; }
        public int PremiumToken { get; set; }
        public UserSubscription Subscription { get; set; }
    }
}
