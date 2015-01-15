using System.Threading.Tasks;
using System.Web.Mvc;
using Elliptical.Mvc;
using Elliptical.Mvc.Identity;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace EllipticalTemplate.Infrastructure
{
    public abstract class AppMvcController : Controller
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

        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public bool HasPassword()
        {
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public bool HasPhoneNumber()
        {
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public void ActiveMenuItem(string item)
        {
            ViewData[item] = "active";
        }

        public async Task<ActionResult> ManageViewAsync<T>(T viewModel)
        {
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await userManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactor = await userManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = await userManager.GetLoginsAsync(User.Identity.GetUserId()),
                BrowserRemembered =
                    await authenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId()),
                EnableTwoFactor = ApplicationIdentity.EnableTwoFactorAuth,
                EnableOAuth = ApplicationIdentity.EnableOAuth
            };

            ViewBag.IndexViewModel = model;
            return View(viewModel);
        }

        public async Task<ActionResult> ManageViewAsync<T>(T viewModel, string menuItem)
        {
            ActiveMenuItem(menuItem);
            return await ManageViewAsync(viewModel);
        }

        public async Task<ActionResult> ManageViewAsync<T>(T viewModel, NotificationType notificationType,
            string message)
        {
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await userManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactor = await userManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = await userManager.GetLoginsAsync(User.Identity.GetUserId()),
                BrowserRemembered =
                    await authenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId()),
                EnableTwoFactor = ApplicationIdentity.EnableTwoFactorAuth,
                EnableOAuth = ApplicationIdentity.EnableOAuth
            };

            ViewBag.IndexViewModel = model;
            switch (notificationType)
            {
                case NotificationType.Error:
                    return View(viewModel).WithError(message);

                case NotificationType.Success:
                    return View(viewModel).WithSuccess(message);

                case NotificationType.Warning:
                    return View(viewModel).WithWarning(message);

                default:
                    return View(viewModel).WithInfo(message);
            }
        }

        public async Task<ActionResult> ManageViewAsync<T>(T viewModel, NotificationType notificationType,
            string message, string menuItem)
        {
            ActiveMenuItem(menuItem);
            return await ManageViewAsync(viewModel, message);
        }
    }
}