using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elliptical.Mvc;
using EllipticalTemplate.Models;
using EllipticalTemplate.Infrastructure;

namespace EllipticalTemplate.Controllers
{
    public class HomeController : AppMvcController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View().WithActiveMenuItem("About");
        }

        public ActionResult Contact()
        {
           
            var contact = new ContactViewModel();
            return View(contact).WithActiveMenuItem("Contact");
        }

        public ActionResult Support()
        {
            var support = new SupportViewModel();
            return View(support);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Support(SupportViewModel support)
        {
            if(ModelState.IsValid)
            {
                
                return RedirectToAction("Index")
                       .WithSuccess("Support Form Successfully Submitted");
            }
            else
            {
                ViewBag.Title = "Support";
                return View(new SupportViewModel())
                      .WithError("Model Validation Error");
            }
            
        }

        [ActionName("Privacy-Policy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }


        [ActionName("Security-Policy")]
        public ActionResult SecurityPolicy()
        {
            return View();
        }

        [ActionName("Get-Started")]
        public ActionResult GetStarted()
        {
            return View().WithActiveMenuItem("GetStarted");
        }
    }
}