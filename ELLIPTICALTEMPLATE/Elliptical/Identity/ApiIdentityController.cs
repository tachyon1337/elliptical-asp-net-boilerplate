using System;
using System.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Elliptical.Mvc.Identity
{
    public abstract class ApiIdentityController : ApiController, IApiIdentityController
    {
        private NameValueCollection configMessages
        {
            get { return ApplicationIdentity.MessageSettings; }
        }

        public IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    
                    ModelState.AddModelError("",result.Errors.FirstOrDefault());
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        public IHttpActionResult ValidLogin(ILoginProfileViewModel loginProfile)
        {
            var response = new ApiResponse<ILoginProfileViewModel> {Message = "OK", Model = loginProfile};
            return Content(HttpStatusCode.OK, response);
        }

        public IHttpActionResult LockedAccount()
        {
            var msg = configMessages["Login.AccountLocked"];
            var response = new ApiResponse<string> {Message = msg, Model = null};
            return Content(HttpStatusCode.Unauthorized, response);
        }

        public IHttpActionResult LoginValidationError()
        {
            var response = new ApiResponse<string> {Message = configMessages["Login.ValidationError"], Model = null};
            return Content(HttpStatusCode.BadRequest, response);
        }

        public IHttpActionResult SendCode(IApiLoginViewModel model)
        {
            return SendCode(model, "Account", "SendCode");
        }

        public IHttpActionResult SendCode(IApiLoginViewModel model,string controller,string action)
        {
            
            string location = "/" + controller + "/" + action;
            if (model.ReturnUrl != null)
            {
                location += "?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl) + "&RememberMe=" + model.RememberMe;
            }
            else
            {
                location += "?RememberMe=" + model.RememberMe;
            }

            return Content(HttpStatusCode.RedirectMethod, location);

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

        public IHttpActionResult ApiResponse(string message, HttpStatusCode statusCode)
        {
            var response = new ApiResponse<string> {Message = message, Model = null};
            return Content(statusCode, response);
        }
    }
}