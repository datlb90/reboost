using System;
using System.ComponentModel.DataAnnotations;

namespace Reboost.Shared
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
