using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Elliptical.Mvc.Identity;

namespace EllipticalTemplate.Identity
{
    public class SmsService : ISmsService
    {
        const string id = "MockSmsService";
        public string Id
        {
            get
            {
                return Id;
            }
        }
        
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}