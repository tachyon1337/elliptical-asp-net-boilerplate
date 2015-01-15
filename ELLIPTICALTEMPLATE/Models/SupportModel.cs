using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Elliptical.Mvc;

namespace EllipticalTemplate.Models
{
    public class SupportViewModel
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
        public string IssueType { get; set; }
        public ICollection<SelectListItem> IssueTypes { get; set; }

        public SupportViewModel()
        {
            Name = null;
            Email = null;
            Comments = null;
            Phone = null;
            IssueType = "Select";
            IssueTypes = new List<SelectListItem> 
            { 
                new SelectListItem { Text = "Web Site", Value = "Web Site" },
                new SelectListItem { Text = "Email", Value = "Email" }, 
                new SelectListItem { Text = "Software", Value = "Software" } ,
                new SelectListItem { Text = "Other", Value = "Other" }
            };
        }
    }
}