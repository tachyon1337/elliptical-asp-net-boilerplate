using System.Collections.Generic;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public static class NotificationExtensions
    {
        private const string Notifications = "_Notifications";

        public static List<Notification> GetNotifications(this TempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Notifications))
            {
                tempData[Notifications] = new List<Notification>();
            }

            return (List<Notification>) tempData[Notifications];
        }

        public static ActionResult WithSuccess(this ActionResult result, string message)
        {
            return new NotificationActionResult(result, "success", message);
        }

        public static ActionResult WithInfo(this ActionResult result, string message)
        {
            return new NotificationActionResult(result, "info", message);
        }

        public static ActionResult WithWarning(this ActionResult result, string message)
        {
            return new NotificationActionResult(result, "warning", message);
        }

        public static ActionResult WithError(this ActionResult result, string message)
        {
            return new NotificationActionResult(result, "error", message);
        }
    }
}