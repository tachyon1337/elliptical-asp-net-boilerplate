using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elliptical.Mvc.Identity
{
    public class ApiLoginViewModel : IApiLoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}