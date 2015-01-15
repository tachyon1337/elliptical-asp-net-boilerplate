using System.Collections.Specialized;
using Elliptical.Mvc.Identity;

namespace EllipticalTemplate.Models

{
    public static class ManageMessageId
    {
        private static NameValueCollection messages
        {
            get { return ApplicationIdentity.MessageSettings; }
        }

        public static string AddPhoneSuccess
        {
            get { return messages["Manage.AddPhoneSuccess"]; }
        }

        public static string VerifyPhoneNumber
        {
            get { return messages["Manage.VerifyPhoneNumber"]; }
        }

        public static string ChangePasswordSuccess
        {
            get { return messages["Manage.ChangePasswordSuccess"]; }
        }

        public static string SetTwoFactorSuccess
        {
            get { return messages["Manage.SetTwoFactorSuccess"]; }
        }

        public static string EnableTwoFactorAuth
        {
            get { return messages["Manage.EnableTwoFactorAuth"]; }
        }

        public static string DisableTwoFactorAuth
        {
            get { return messages["Manage.DisableTwoFactorAuth"]; }
        }

        public static string SetPasswordSuccess
        {
            get { return messages["Manage.SetPasswordSuccess"]; }
        }

        public static string RemoveLoginSuccess
        {
            get { return messages["Manage.RemoveLoginSuccess"]; }
        }

        public static string RemovePhoneSuccess
        {
            get { return messages["Manage.RemovePhoneSuccess"]; }
        }

        public static string NullPhoneNumber
        {
            get { return messages["Manage.NullPhoneNumber"]; }
        }

        public static string Error
        {
            get { return messages["Error"]; }
        }

        public static string ModelError
        {
            get { return messages["Model.Error"]; }
        }
    }
}