using System;
using EllipticalTemplate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ApplicationIdentity = Elliptical.Mvc.Identity.ApplicationIdentity;
using Elliptical.Mvc.Identity;

namespace EllipticalTemplate.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IEmailService emailService,
            ISmsService smsService)
            : base(store)
        {
            //ApplicationIdentity settings
            var validationSettings = ApplicationIdentity.ValidationSettings;
            var identifier = ApplicationIdentity.IdentifierString;
            var isAzureWebHost = ApplicationIdentity.IsAzureWebHost;
            
            

            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = Convert.ToBoolean(validationSettings["AllowOnlyAlphanumericUserNames"]),
                RequireUniqueEmail = Convert.ToBoolean(validationSettings["RequireUniqueEmail"])
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = Convert.ToInt32(validationSettings["MinimumPasswordLength"]),
                RequireNonLetterOrDigit = Convert.ToBoolean(validationSettings["RequireNonLetterOrDigit"]),
                RequireDigit = Convert.ToBoolean(validationSettings["RequireDigit"]),
                RequireLowercase = Convert.ToBoolean(validationSettings["RequireLowercase"]),
                RequireUppercase = Convert.ToBoolean(validationSettings["RequireUppercase"])
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = Convert.ToBoolean(validationSettings["UserLockoutEnabledByDefault"]);
            DefaultAccountLockoutTimeSpan =
                TimeSpan.FromMinutes(Convert.ToInt32(validationSettings["DefaultAccountLockoutTimeSpan"]));
            MaxFailedAccessAttemptsBeforeLockout =
                Convert.ToInt32(validationSettings["MaxFailedAccessAttemptsBeforeLockout"]);

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = emailService;
            SmsService = smsService;

            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                var dataProtector = dataProtectionProvider.Create(identifier);

                UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtector);
            }

            //alternatively use this if you are running in Azure Web Sites
            if (isAzureWebHost)
            {
                UserTokenProvider = new EmailTokenProvider<ApplicationUser, string>();
            }

            
        }

       
    }
}