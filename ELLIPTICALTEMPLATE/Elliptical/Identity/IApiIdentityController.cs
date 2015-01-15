using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Elliptical.Mvc.Identity
{
    public interface IApiIdentityController
    {
        IHttpActionResult GetErrorResult(IdentityResult result);
        IHttpActionResult ValidLogin(ILoginProfileViewModel loginProfile);
        IHttpActionResult LockedAccount();
        IHttpActionResult LoginValidationError();
        IHttpActionResult ApiResponse<T>(string message, T model);
        IHttpActionResult ApiResponse<T>(string message, T model, HttpStatusCode statusCode);
        IHttpActionResult ApiResponse(string message, HttpStatusCode statusCode);
    }
}