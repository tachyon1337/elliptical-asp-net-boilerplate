using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Elliptical.Mvc;
using Elliptical.Mvc.Identity;
using AutoMapper;
using EllipticalTemplate.Models;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Infrastructure;


namespace EllipticalTemplate.Controllers
{
    [Authorize]
    public class ManageController : AppMvcController
    {
        private const string XsrfKey = "XsrfId";
        private readonly ApplicationUserManager userManager;
        private readonly IAuthenticationManager authenticationManager;

        public ManageController(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, ILoginProfileViewModel loginProfile)
        {
            this.userManager = userManager;
            this.authenticationManager = authenticationManager;
            this.initializeAuthManager(userManager, authenticationManager);
        }


       
       /*  GET: /Manage/Index */
       public async Task<ActionResult> Index()
       {
           var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
           Mapper.CreateMap<ApplicationUser, ProfileViewModel>();
           var profile = Mapper.Map<ProfileViewModel>(user);
           return await ManageViewAsync(profile,"Index");
       }



       /* GET: /Manage/ChangePassword */
       public async Task<ActionResult> ChangePassword()
       {
           return await ManageViewAsync(new ChangePasswordViewModel(),"Password");
       }



       /* POST: /Manage/ChangePassword */
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
       {
           string error = "";
           if (!ModelState.IsValid)
           {
               return await ManageViewAsync(model, NotificationType.Error, ManageMessageId.ModelError);
           }
           var result = await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
           if (result.Succeeded)
           {
               var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
               if (user != null)
               {
                   await SignInAsync(user, isPersistent: false);
               }
               return RedirectToAction("Index").WithSuccess(ManageMessageId.ChangePasswordSuccess);
           }
           /* If we got this far, something failed, redisplay form */
           error = result.Errors.FirstOrDefault();
           return await ManageViewAsync(model, NotificationType.Error, error,"Password");
       }




       /* GET: /Manage/SetPassword */
       public async Task<ActionResult> SetPassword()
       {
           return await ManageViewAsync(new SetPasswordViewModel(),"Password");
       }



       /* POST: /Manage/SetPassword */
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
       {
           string error="";
           if (!ModelState.IsValid)
           {
               return await ManageViewAsync(model, NotificationType.Error, ManageMessageId.ModelError);
           }
           var result = await userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
           if (result.Succeeded)
           {
               var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
               if (user != null)
               {
                   await SignInAsync(user, isPersistent: false);
               }
               return RedirectToAction("Index").WithSuccess(ManageMessageId.SetPasswordSuccess);
           }

           /* If we got this far, something failed, redisplay form */
           error = result.Errors.FirstOrDefault();
           return await ManageViewAsync(model, NotificationType.Error, error,"Password");
       }



       /* GET: /Manage/AddPhoneNumber */
       public async Task<ActionResult> AddPhoneNumber()
       {
           var model = new AddPhoneNumberViewModel();
           var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
           model.Number = user.PhoneNumber ?? user.Phone;
           return await ManageViewAsync(model,"AddNumber");
       }



       /* POST: /Manage/AddPhoneNumber */
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
       {
           if (!ModelState.IsValid)
           {
               return await ManageViewAsync(model, NotificationType.Error, ManageMessageId.ModelError,"AddNumber");
           }
           // Generate the token and send it
           var code = await userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
           if (userManager.SmsService != null)
           {
               var message = new IdentityMessage
               {
                   Destination = model.Number,
                   Body = "Your security code is: " + code
               };
               await userManager.SmsService.SendAsync(message);
           }
           return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number }).WithInfo(ManageMessageId.VerifyPhoneNumber);
       }


       /* GET: /Manage/VerifyPhoneNumber */
       public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
       {
           var code = await userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
           // Send an SMS through the SMS provider to verify the phone number
           //return phoneNumber == null ? View(new VerifyPhoneNumberViewModel()).WithError(ManageMessageId.NullPhoneNumber) : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });

           if (phoneNumber == null)
           {
               return await ManageViewAsync(new VerifyPhoneNumberViewModel(), NotificationType.Error, ManageMessageId.NullPhoneNumber, "AddNumber");
           }
           else
           {
               return await ManageViewAsync(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber }, "AddNumber");
           }
       }



       /* POST: /Manage/VerifyPhoneNumber */
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
       {
           string error = "";
           if (!ModelState.IsValid)
           {
               return await ManageViewAsync(model, NotificationType.Error, ManageMessageId.ModelError);
           }
           var result = await userManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
           if (result.Succeeded)
           {
               var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
               if (user != null)
               {
                   await SignInAsync(user, isPersistent: false);
               }
               return RedirectToAction("Index").WithSuccess(ManageMessageId.AddPhoneSuccess);
           }
           /* If we got this far, something failed, redisplay form */
           error = result.Errors.FirstOrDefault();
           return await ManageViewAsync(model, NotificationType.Error, error);
       }



       /* GET: /Manage/RemovePhoneNumber */
       public async Task<ActionResult> RemovePhoneNumber()
       {
           var result = await userManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
           if (!result.Succeeded)
           {
               return RedirectToAction("Index").WithError(ManageMessageId.Error);
           }
           var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
           if (user != null)
           {
               await SignInAsync(user, isPersistent: false);
           }
           return RedirectToAction("Index").WithSuccess(ManageMessageId.RemovePhoneSuccess);
       }



       /* GET: /Manage/ManageLogins */
       public async Task<ActionResult> ManageLogins()
       {
          
           var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
           if (user == null)
           {
               return await ManageViewAsync(new ManageLoginsViewModel(),NotificationType.Error,"Null User Error");
           }
           var userLogins = await userManager.GetLoginsAsync(User.Identity.GetUserId());
           var otherLogins = authenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
           ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
           var model=new ManageLoginsViewModel
           {
               CurrentLogins = userLogins,
               OtherLogins = otherLogins
           };

           return await ManageViewAsync(model,"Logins");
       }



       /* POST: /Manage/LinkLogin */
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult LinkLogin(string provider)
       {
           /* Request a redirect to the external login provider to link a login for the current user */
           try
           {
               return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId(), XsrfKey);
           }catch(Exception ex)
           {
               return RedirectToAction("ManageLogins").WithError(ex.Message);
           }
           
       }



       /* GET: /Manage/LinkLoginCallback */
       public async Task<ActionResult> LinkLoginCallback()
       {
           var loginInfo = await authenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
           if (loginInfo == null)
           {
               return RedirectToAction("ManageLogins").WithError(ManageMessageId.Error);
           }
           var result = await userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
           return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins").WithError(ManageMessageId.Error);
       }
        


        /* GET: /Manage/RemoveLogin */
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = userManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }



        /* POST: /Manage/RemoveLogin */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await userManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("ManageLogins").WithSuccess(ManageMessageId.RemoveLoginSuccess);
            }
            else
            {
                return RedirectToAction("ManageLogins").WithError("Error removing login");
            }
        }


        /* GET: /Manage/SmsAuthentication */
        public async Task<ActionResult> SmsAuthentication()
        {
            return await ManageViewAsync<string>(null,"Authentication");
        }


        
        /* POST: /Manage/EnableTwoFactorAuthentication */
        [HttpPost]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage").WithSuccess(ManageMessageId.EnableTwoFactorAuth);
        }


        
        /* POST: /Manage/DisableTwoFactorAuthentication */
        [HttpPost]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage").WithSuccess(ManageMessageId.DisableTwoFactorAuth);
        }

       
    }
}