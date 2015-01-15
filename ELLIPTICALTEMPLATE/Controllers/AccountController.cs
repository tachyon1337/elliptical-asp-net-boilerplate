using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Elliptical.Mvc;
using Elliptical.Mvc.Identity;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ApplicationIdentity = Elliptical.Mvc.Identity.ApplicationIdentity;

//using System.Security.Claims;

namespace EllipticalTemplate.Controllers
{
    [Authorize]
    public class AccountController : IdentityController
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly ILoginProfileViewModel loginProfile;
        private readonly ApplicationSignInManager signInManager;
        private readonly ApplicationUserManager userManager;
        private readonly string xsrfKey = "XsrfId";

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



        /* GET: /Account/Register */
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }



        /* POST: /Account/Register */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email};
                user = await user.CopyPropertiesFromAsync(model,true);
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (confirmEmail)
                    {
                        var subject = AccountMessageId.ConfirmEmailSubject;
                        var body = AccountMessageId.ConfirmEmailBody;
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code},
                            Request.Url.Scheme);
                        body += Environment.NewLine + "Please confirm your account by clicking <a href=\"" + callbackUrl +
                                "\">here</a>";
                        try
                        {
                            await userManager.SendEmailAsync(user.Id, subject, body);
                            return RedirectToLocal("/Account/RegisterConfirmEmailInfo").WithCookie(AccountMessageId.ConfirmEmailCookie, "true");
                        }catch(Exception ex)
                        {
                            return View(model).WithError(ex.Message);
                        }
                    }
                    else
                    {
                        await signInManager.SignInAsync(user, false, false);
                    }

                    loginProfile.FirstName = model.FirstName;
                    loginProfile.LastName = model.LastName;
                    return RedirectToLocal().WithProfileCookie(loginProfile);
                }

                return View(model).WithError(result.Errors.FirstOrDefault());
            }

            /* If we got this far, something failed, redisplay form */
            return View(model).WithError(AccountMessageId.ModelError);
        }



        /* GET: /Account/RegisterConfirmEmailInfo */
        [AllowAnonymous]
        public ActionResult RegisterConfirmEmailInfo(string returnUrl)
        {
           if(Request.Cookies[AccountMessageId.ConfirmEmailCookie]==null)
           {
               return RedirectToAction("Login", "Account").WithError("Invalid Request");
           }
           ClearIdentityCookie(new HttpCookie(AccountMessageId.ConfirmEmailCookie));
           ViewBag.Message = AccountMessageId.ConfirmEmailNotice;
           return View();
        }



        /* GET: /Account/Login */
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }



        /* POST: /Account/Login */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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
                    return RedirectToLocal(returnUrl).WithProfileCookie(loginProfile);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, model.RememberMe});
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", AccountMessageId.LoginFailure);
                    return View(model).WithError(AccountMessageId.LoginFailure);
            }
        }

        /* GET: /Account/ConfirmEmail */
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        /* GET: /Account/ForgotPassword */
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }


        /* POST: /Account/ForgotPassword */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user.Id)))
                {
                   
                    return View(new ForgotPasswordViewModel()).WithError(AccountMessageId.ForgotPasswordNoUserAccount);
                }

                /* Send an email with this link */
                var code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                var subject = AccountMessageId.ForgotPasswordSubject;
                var body = AccountMessageId.ForgotPasswordBody;
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code },
                    Request.Url.Scheme);
                body += Environment.NewLine + "Please reset your password by clicking <a href=\"" + callbackUrl +
                        "\">here</a>";
                await userManager.SendEmailAsync(user.Id, subject, body);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            /* If we got this far, something failed, redisplay form */
            return View(model);
        }



        /* GET: /Account/ForgotPasswordConfirmation */
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }



        /* GET: /Account/ResetPassword */
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }



        /* POST: /Account/ResetPassword */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                /* Don't reveal that the user does not exist */
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }



        /* GET: /Account/ResetPasswordConfirmation */
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        /* GET: /Account/VerifyCode */
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            /* Require that the user has already logged in via username/password or external login */
            if (!await signInManager.HasBeenVerifiedAsync())
            {
                return View("Error").WithError(AccountMessageId.UserNotVerified);
            }
            var user = await userManager.FindByIdAsync(await signInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await userManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }


        /* POST: /Account/VerifyCode */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            /* The following code protects for brute force attacks against the two factor codes. 
               If a user enters incorrect codes for a specified amount of time then the user account 
               will be locked out for a specified amount of time. 
               You can configure the account lockout settings in IdentityConfig
             */
            var result =
                await
                    signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe,
                        model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    var userId = signInManager.GetVerifiedUserId();
                    var currentUser = userManager.FindById(userId);
                    loginProfile.FirstName = currentUser.FirstName;
                    loginProfile.LastName = currentUser.LastName;
                    return RedirectToLocal(model.ReturnUrl).WithProfileCookie(loginProfile).WithSuccess(AccountMessageId.LoginSuccess);
                case SignInStatus.LockedOut:
                    return View(model).WithWarning(AccountMessageId.AccountLocked);
                default:
                    ModelState.AddModelError("", AccountMessageId.InvalidCode);
                    return View(model).WithError(AccountMessageId.InvalidCode);
            }
        }


        /* GET: /Account/SendCode */
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await signInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error").WithError(AccountMessageId.UserNotVerified);
            }
            var userFactors = await userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return
                View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }



        /* POST: /Account/SendCode */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model).WithError(AccountMessageId.ValidationError);
            }

            /* Generate the token and send it */
            if (!await signInManager.SendTwoFactorCodeAsync(model.Provider))
            {
                return View(model).WithError(AccountMessageId.Error);
            }
            return RedirectToAction("VerifyCode",
                new { Provider = model.Provider, model.ReturnUrl, model.RememberMe });
        }



        /* POST: /Account/ExternalLogin */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            /* Request a redirect to the external login provider */
            if(provider==null)
            {
                RedirectToAction("Login").WithError(AccountMessageId.InvalidProvider);
            }
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }



        /* GET: /Account/ExternalLoginCallback */
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login").WithError(AccountMessageId.UserNotVerified);
            }

            /* Sign in the user with this external login provider if the user already has a login */
            var result = await signInManager.ExternalSignInAsync(loginInfo,false);
            switch (result)
            {
                case SignInStatus.Success:
                    /* redirect to get the local identity before proceeding to returnUrl */
                    return RedirectToAction("ExternalLoginRedirect", new { ReturnUrl = returnUrl });
                case SignInStatus.LockedOut:
                    return RedirectToAction("Login").WithWarning(AccountMessageId.AccountLocked);
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                case SignInStatus.Failure:
                    
                default:
                    /* If the user does not have an account, then prompt the user to create an account */
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }



        /* POST: /Account/ExternalLoginConfirmation */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                /* Get the information about the user from the external login provider */
                var info = await authenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure").WithError(AccountMessageId.LoginFailure);
                }
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email, FirstName=model.FirstName, LastName=model.LastName};
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false, false);
                        var currentUser = userManager.FindByEmail(model.Email);
                        loginProfile.FirstName = currentUser.FirstName;
                        loginProfile.LastName = currentUser.LastName;
                        return RedirectToLocal(returnUrl).WithProfileCookie(loginProfile).WithSuccess(AccountMessageId.LoginSuccess);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model).WithError(AccountMessageId.Error);
        }


        /* GET: /Account/ExternalLoginFailure */
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /* GET: /Account/ExternalLoginRedirect
         * this allows us to get the local identity after an external login
         */
        public ActionResult ExternalLoginRedirect(string returnUrl)
        {
            var userId = User.Identity.GetUserId();
            var currentUser = userManager.FindById(userId);
            loginProfile.FirstName = currentUser.FirstName;
            loginProfile.LastName = currentUser.LastName;
            return RedirectToLocal(returnUrl).WithProfileCookie(loginProfile).WithSuccess(AccountMessageId.LoginSuccess);

        }


        /* GET: /Account/LogOff */
        public ActionResult LogOff()
        {
            authenticationManager.SignOut();
            ClearIdentityCookie();
            return RedirectToAction("Index", "Home");
        }



       
    }
}