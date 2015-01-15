using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Elliptical.Mvc.Identity;
using Twilio;
using System.Diagnostics;

namespace EllipticalTemplate.Identity
{
    public class TwilioSmsService : ISmsService
    {

        const string id = "Twilio";
        public string Id
        {
            get
            {
                return id;
            }
        }
       
        public string Sid
        {
            get
            {
                return ConfigurationManager.AppSettings["Twilio.Sid"];
            }
        }

        public string Token
        {
            get
            {
                return ConfigurationManager.AppSettings["Twilio.Token"];
            }
        }

        public string FromPhone
        {
            get
            {
                return ConfigurationManager.AppSettings["Manage.PhoneNumber"];
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
            var Twilio = new TwilioRestClient(this.Sid, this.Token);
            var result = Twilio.SendMessage(this.FromPhone, message.Destination, message.Body);

            // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.WriteLine(result.Status);

            // Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
        }

    }
}