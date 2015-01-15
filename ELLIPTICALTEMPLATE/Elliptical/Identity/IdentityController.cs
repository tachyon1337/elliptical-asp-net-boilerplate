using System;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Elliptical.Mvc.Identity
{
    public class IdentityController : Controller, IIdentityController
    {
        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult RedirectToLocal()
        {
            return RedirectToLocal(null);
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl !=null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        /*public ActionResult RedirectToLocal(string returnUrl, ILoginProfileViewModel profile)
        {
            Response.Cookies["profile"].Value = Mvc.Json.SerializeObjectToString<ILoginProfileViewModel>(profile, true, true);
            return RedirectToLocal(returnUrl);
        }

        public ActionResult RedirectToLocal(ILoginProfileViewModel profile)
        {
            return RedirectToLocal(null, profile);
        }*/

        /*public ActionResult RedirectToLocal(string returnUrl,HttpCookie cookie)
        {
            Response.Cookies[cookie.Name].Value = cookie.Value;
            return RedirectToLocal(returnUrl);
        }*/

        public void ClearIdentityCookie()
        {
            Response.Cookies["profile"].Expires = DateTime.Now.AddDays(-1);
        }

        public void ClearIdentityCookie(HttpCookie cookie)
        {
            Response.Cookies[cookie.Name].Expires = DateTime.Now.AddDays(-1);
        }
    }
}