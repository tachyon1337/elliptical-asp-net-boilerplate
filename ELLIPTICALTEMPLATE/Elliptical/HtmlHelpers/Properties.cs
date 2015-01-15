using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using Elliptical.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Reflection;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        public HtmlHelper helper { get; set; }

        /// <summary>
        /// Constructor for the Elliptical HtmlHelpers class
        /// </summary>
        /// <param name="helper"></param>
        public HtmlHelpers(HtmlHelper helper)
        {
            this.helper = helper;
        }
    }
}