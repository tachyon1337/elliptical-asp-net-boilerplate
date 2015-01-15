using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Elliptical.Mvc.Identity;
using System.Net;
using SendGrid;
using System.Net.Mail;

namespace EllipticalTemplate.Identity
{
    public class EmailService : IEmailService
    {
        const string id = "MockEmailService";
        public string Id
        {
            get
            {
                return Id;
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
           
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }


}