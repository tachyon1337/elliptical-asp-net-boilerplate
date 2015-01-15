using System.Collections.Generic;
using System.Web.Mvc;
using Elliptical.Mvc.Identity;
namespace Elliptical.Mvc
{
    public static class CookieExtensions
    {
        
        public static ActionResult WithCookie(this ActionResult result, string name,string value)
        {
            return new CookieActionResult<string>(result,name,value);
        }

        public static ActionResult WithCookie<T>(this ActionResult result, string name, T value)
        {
            return new CookieActionResult<T>(result, name, value);
        }

        public static ActionResult WithProfileCookie(this ActionResult result, ILoginProfileViewModel model )
        {
            var profile = new InternalLoginProfileViewModel { FirstName = model.FirstName, LastName = model.LastName };
            return new CookieActionResult<InternalLoginProfileViewModel>(result,"profile", profile);
        }
       
    }
}