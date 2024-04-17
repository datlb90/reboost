using System;
using System.ComponentModel.DataAnnotations;

namespace Reboost.Shared
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
