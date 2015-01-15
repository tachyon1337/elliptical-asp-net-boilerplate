using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Elliptical.Mvc.Identity;
using System.Configuration;
using System.Net;
using SendGrid;
using System.Net.Mail;

namespace EllipticalTemplate.Identity
{
    public class SendGridEmailService : IEmailService
    {
        const string id = "SendGrid";
        public string Id
        {
            get
            {
                return id;
            }
        }

        public string User
        {
            get
            {
                return ConfigurationManager.AppSettings["SendGrid.User"];
            }
        }

        public string Key
        {
            get
            {
                return ConfigurationManager.AppSettings["SendGrid.Key"];
            }
        }

        public string FromAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["Manage.FromAddress"];
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
            var mail = new SendGridMessage();
            mail.From = new MailAddress(this.FromAddress);
            mail.AddTo(message.Destination);
            mail.Subject = message.Subject;
            mail.Text = message.Body;
            var credentials = new NetworkCredential(this.User, this.Key);
            var transportWeb = new Web(credentials);

            return transportWeb.DeliverAsync(mail);
        }
    }
}