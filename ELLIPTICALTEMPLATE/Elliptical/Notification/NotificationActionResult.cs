using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public class NotificationActionResult : ActionResult
    {
        public NotificationActionResult(ActionResult innerResult, string cssClass, string message)
        {
            InnerResult = innerResult;
            CssClass = cssClass;
            Message = message;
        }

        public ActionResult InnerResult { get; set; }
        public string CssClass { get; set; }
        public string Message { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var alerts = context.Controller.TempData.GetNotifications();
            alerts.Add(new Notification(CssClass, Message));
            InnerResult.ExecuteResult(context);
        }
    }
}