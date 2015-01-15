using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Elliptical.Mvc;
using EllipticalTemplate.Infrastructure;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Elliptical.Mvc.Identity;
using AutoMapper;
using ApplicationIdentity = Elliptical.Mvc.Identity.ApplicationIdentity;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Models;


namespace EllipticalTemplate.ApiControllers
{
    [RoutePrefix("api/Manage")]
    public class ManageController : AppApiController
    {
        
        private readonly IAuthenticationManager authenticationManager;
        private readonly ApplicationUserManager userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, ILoginProfileViewModel loginProfile)
        {
            this.userManager = userManager;
            this.authenticationManager = authenticationManager;
            this.initializeAuthManager(userManager, authenticationManager);
        }
        

        /* POST: /api/Manage/Profile */
        [Route("Profile")]
        public async Task<IHttpActionResult> Profile(ProfileViewModel profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userManager.FindById(User.Identity.GetUserId());
                    user = await user.CopyPropertiesFromAsync(profile);
                    await userManager.UpdateAsync(user);
                    
                    return ApiResponse(profile);
                }
                return ApiResponse(profile, HttpStatusCode.BadRequest);
            }catch(Exception)
            {
                return ApiResponse(profile, HttpStatusCode.InternalServerError);
            }
        }


        /* POST: /api/Manage/ChangePassword */
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            await SignInAsync(user, isPersistent: false);
                        }
                        return ApiResponse(model);
                    }
                }
               
                return ApiResponse(model, HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return ApiResponse(model, HttpStatusCode.InternalServerError);
            }
        }


        /* POST: /api/Manage/SetPassword */
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            await SignInAsync(user, isPersistent: false);
                        }
                        return ApiResponse(model);
                    }
                }
                return ApiResponse(model, HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return ApiResponse(model, HttpStatusCode.InternalServerError);
            }
        }
    }
}
