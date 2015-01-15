using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Elliptical.Mvc.Identity
{
    public interface IIdentityController
    {
        void AddErrors(IdentityResult result);
        ActionResult RedirectToLocal(string returnUrl);
    }
}