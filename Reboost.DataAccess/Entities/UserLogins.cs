using System;
namespace Reboost.DataAccess.Entities
{
    public class UserLogins : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime LoginDate { get; set; }
    }
}

