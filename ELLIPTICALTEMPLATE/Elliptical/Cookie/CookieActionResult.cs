using System.Web;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public class CookieActionResult<T> : ActionResult
    {
        private ActionResult innerResult { get; set; }
        private string name { get; set; }
        private string value { get; set; }

        public CookieActionResult(ActionResult innerResult, string name, string value)
        {
            this.innerResult = innerResult;
            this.name = name;
            this.value = value;
        }
        
        public CookieActionResult(ActionResult innerResult, string name, T value)
        {
            this.innerResult = innerResult;
            this.name = name;
            this.value = Json.SerializeObjectToString(value,true,true);
        }

      
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Cookies[name].Value = value;
            innerResult.ExecuteResult(context);
        }
    }
}