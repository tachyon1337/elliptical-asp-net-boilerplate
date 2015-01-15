using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Elliptical.Mvc;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace EllipticalTemplate.Infrastructure
{
    public abstract class AppApiController : ApiController
    {
        private IAuthenticationManager authenticationManager;
        private ApplicationUserManager userManager;

        public void initializeAuthManager(ApplicationUserManager userManager,
            IAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.authenticationManager = authenticationManager;
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
            authenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent},
                await user.GenerateUserIdentityAsync(userManager));
        }

        public IHttpActionResult ApiResponse<T>(T model)
        {
            return Content(HttpStatusCode.OK, model);
        }

        public IHttpActionResult ApiResponse<T>(T model, HttpStatusCode statusCode)
        {
            return Content(statusCode, model);
        }

        public IHttpActionResult ApiResponse<T>(string message, T model)
        {
            var response = new ApiResponse<T> {Message = message, Model = model};
            return Content(HttpStatusCode.OK, response);
        }

        public IHttpActionResult ApiResponse<T>(string message, T model, HttpStatusCode statusCode)
        {
            var response = new ApiResponse<T> {Message = message, Model = model};
            return Content(statusCode, response);
        }
    }
}