using System.Collections.Specialized;
using Elliptical.Mvc.Identity;

namespace EllipticalTemplate.Models
{
    public class AccountMessageId
    {
        private static NameValueCollection messages
        {
            get { return ApplicationIdentity.MessageSettings; }
        }

        public static string ConfirmEmailSubject
        {
            get { return messages["Account.ConfirmEmail.Subject"]; }
        }

        public static string ConfirmEmailBody
        {
            get { return messages["Account.ConfirmEmail.Body"]; }
        }

        public static string ConfirmEmailNotice
        {
            get { return messages["Account.ConfirmEmail.Notice"]; }
        }

        public static string ConfirmEmailCookie
        {
            get { return messages["Account.ConfirmEmail.Cookie"]; }
        }

        public static string ForgotPasswordSubject
        {
            get { return messages["Account.ForgotPassword.Subject"]; }
        }

        public static string ForgotPasswordBody
        {
            get { return messages["Account.ForgotPassword.Body"]; }
        }

        public static string ForgotPasswordNoUserAccount
        {
            get { return messages["Account.ForgotPassword.UsernameDoesNotExist"]; }
        }

        public static string AccountLocked
        {
            get { return messages["Account.Login.AccountLocked"]; }
        }

        public static string InvalidCode
        {
            get { return messages["Account.Login.InvalidCode"]; }
        }

        public static string InvalidProvider
        {
            get { return messages["Account.Login.ProviderFailure"]; }
        }

        public static string LoginSuccess
        {
            get { return messages["Account.Login.Success"]; }
        }

        public static string LoginFailure
        {
            get { return messages["Account.Login.Failure"]; }
        }

        public static string UserNotVerified
        {
            get { return messages["Account.Login.UserNotVerified"]; }
        }

        public static string ValidationError
        {
            get { return messages["Account.Login.ValidationError"]; }
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