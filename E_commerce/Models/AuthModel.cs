﻿using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace EcomMakeUp.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get;  set; }
        public string IdUser { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
        public string Token { get; set; }
        //public DateTime Expireson { get; set; }
        [JsonIgnore]//مش لازم ترجع من الباقى
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirat { get; set; }

    }
}
