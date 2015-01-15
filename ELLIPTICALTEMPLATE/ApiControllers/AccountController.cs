using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Elliptical.Mvc;
using Elliptical.Mvc.Identity;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ApplicationIdentity = Elliptical.Mvc.Identity.ApplicationIdentity;

namespace EllipticalTemplate.ApiControllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiIdentityController
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly ILoginProfileViewModel loginProfile;
        private readonly ApplicationSignInManager signInManager;
        private readonly ApplicationUserManager userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager, ILoginProfileViewModel loginProfile)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authenticationManager = authenticationManager;
            this.loginProfile = loginProfile;
        }

        private bool confirmEmail
        {
            get { return ApplicationIdentity.ConfirmEmail; }
        }

        /* POST api/Account/Register */
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser {UserName = model.Email};
            user = await user.CopyPropertiesFromAsync(model,true);
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return ApiResponse(model, HttpStatusCode.Unauthorized);
            }
            if (confirmEmail)
            {
                var subject = AccountMessageId.ConfirmEmailSubject;
                var body = AccountMessageId.ConfirmEmailBody;
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Link("Default",
                    new {Controller = "Account", Action = "ConfirmEmail", userId = user.Id, code});
                body += Environment.NewLine + "Please confirm your account by clicking <a href=\"" + callbackUrl +
                        "\">here</a>";
                await userManager.SendEmailAsync(user.Id, subject, body);
                return ApiResponse("/Account/RegisterConfirmEmailInfo", model, HttpStatusCode.RedirectMethod);
            }
            else
            {
                await signInManager.SignInAsync(user, false, false);
            }
            return ApiResponse(model, HttpStatusCode.OK);
        }

        /* POST api/Account/Login */
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(ApiLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return LoginValidationError();
            }

            /* This doesn't count login failures towards account lockout
               To enable password failures to trigger account lockout, change to shouldLockout: true
             */
            var result =
                await
                    signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                        ApplicationIdentity.LockOutAccount);
            switch (result)
            {
                case SignInStatus.Success:
                    var currentUser = userManager.FindByEmail(model.Email);
                    loginProfile.FirstName = currentUser.FirstName;
                    loginProfile.LastName = currentUser.LastName;
                    return ValidLogin(loginProfile);
                case SignInStatus.LockedOut:
                    return LockedAccount();
                case SignInStatus.RequiresVerification:
                    return SendCode(model);
                case SignInStatus.Failure:
                default:
                    return ApiResponse("Invalid login attempt",model, HttpStatusCode.Unauthorized);
            }
        }
    }
}