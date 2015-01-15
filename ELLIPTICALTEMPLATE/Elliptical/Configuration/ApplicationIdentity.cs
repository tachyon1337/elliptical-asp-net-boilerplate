using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Elliptical.Mvc.Configuration;
using Elliptical.Mvc.Configuration.Identity;

namespace Elliptical.Mvc.Identity
{
    public static class ApplicationIdentity
    {
        private static readonly IdentitySection config =
            ConfigurationManager.GetSection("ellipticalIdentity") as IdentitySection;

        public static int MinimumPasswordLength
        {
            get { return Convert.ToInt32(ValidationSettings["MinimumPasswordLength"]); }
        }

        public static bool LockOutAccount
        {
            get { return Convert.ToBoolean(ValidationSettings["UserLockoutEnabledByDefault"]); }
        }

        public static bool ConfirmEmail
        {
            get { return config.ConfirmEmail; }
        }

        public static bool IsAzureWebHost
        {
            get { return config.IsAzureWebHost; }
        }

        public static bool UseAccountApi
        {
            get { return config.UseAccountApi; }
        }

        public static string IdentifierString
        {
            get { return config.IdentifierString; }
        }

        public static bool EnableTwoFactorAuth
        {
            get { return config.EnableTwoFactorAuth; }
        }

        public static bool EnableOAuth
        {
            get { return config.EnableOAuth; }
        }

        public static NameValueCollection ValidationSettings
        {
            get
            {
                var settings = config.Validation.Settings.OfType<Add>();
                var namevalueColl = new NameValueCollection();
                foreach (var s in settings)
                {
                    namevalueColl.Add(s.Key, s.Value);
                }
                return namevalueColl;
            }
        }

        public static NameValueCollection MessageSettings
        {
            get
            {
                var settings = config.Messages.Settings.OfType<Add>();
                var namevalueColl = new NameValueCollection();
                foreach (var s in settings)
                {
                    namevalueColl.Add(s.Key, s.Value);
                }
                return namevalueColl;
            }
        }
    }
}