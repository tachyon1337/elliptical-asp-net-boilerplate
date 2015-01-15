

namespace Elliptical.Mvc
{
    public class Notification
    {
        public string CssClass { get; set; }
		public string Message { get; set; }

		public Notification(string cssClass, string message)
		{
			CssClass = cssClass;
			Message = message;
		}
    }
}