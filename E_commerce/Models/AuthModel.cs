using System.Collections.Generic;
using System;

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
        public DateTime Expireson { get; set; }

    }
}
