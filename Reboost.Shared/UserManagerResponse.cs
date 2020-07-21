﻿using System;
using System.Collections.Generic;

namespace Reboost.Shared
{
    public class UserManagerResponse
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
