using System.Web;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public class ActiveMenuItemActionResult : ActionResult
    {
        private ActionResult innerResult { get; set; }
        private string item { get; set; }
       

        public ActiveMenuItemActionResult(ActionResult innerResult, string item)
        {
            this.innerResult = innerResult;
            this.item = item;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.ViewData[item] = "active";
            innerResult.ExecuteResult(context);
        }
    }
}