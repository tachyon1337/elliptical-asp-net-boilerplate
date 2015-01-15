using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Elliptical.Mvc;

namespace EllipticalTemplate.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Comments { get; set; }
        [Required]
        public string Phone { get; set; }
        [SelectRequired]
        public string ReferralSource { get; set; }
        public ICollection<SelectListItem> ReferralSources { get; set; }
        [Required]
        public string ContactOption { get; set; }
        public ICollection<SelectListItem> ContactOptions { get; set; }
        public bool ContactMeOnWeekends { get; set; }

        public ContactViewModel()
        {
            Name = null;
            Email = null;
            Comments = null;
            Phone = null;
            ReferralSource = "Select";
            ReferralSources = new List<SelectListItem> 
            { 
                new SelectListItem { Text = "Google", Value = "Google" },
                new SelectListItem { Text = "LinkedIn", Value = "LinkedIn" }, 
                new SelectListItem { Text = "Ad", Value = "Ad" } ,
                new SelectListItem { Text = "Other", Value = "Other" }
            };
            ContactOptions = new List<SelectListItem> 
            { 
                new SelectListItem { Text = "Email", Value = "Email" },
                new SelectListItem { Text = "Phone", Value = "Phone" },
               
            };
        }
    }

    public class SubscribeViewModel
    {
        public string Email { get; set; }
    }
}