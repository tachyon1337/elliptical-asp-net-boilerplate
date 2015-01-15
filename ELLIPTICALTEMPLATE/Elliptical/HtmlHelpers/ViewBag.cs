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
        /// <summary>
        /// Client-side ViewBag...Serializes server-side view models to the browser context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>MvcHtmlString Script Object</returns>
        public IHtmlString ViewBag<T>(T model)
        {
            var jsObject = Json.SerializeObject(model);
            var script = scriptObject(this.helper, jsObject, null);
            return new MvcHtmlString(script);
        }


        /// <summary>
        /// Client-side ViewBag...Serializes server-side view models to the browser context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="modelName">The ViewBag Property Name for the Model</param>
        /// <returns>MvcHtmlString Script Object</returns>
        public IHtmlString ViewBag<T>(T model, string modelName)
        {
            var jsObject = Json.SerializeObject(model);
            var script = scriptObject(this.helper, jsObject, modelName);

            return new MvcHtmlString(script);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="quoteName"></param>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        public IHtmlString ViewBag<T>(T model, bool quoteName, bool camelCase)
        {
            var jsObject = Json.SerializeObject(model, quoteName, camelCase);
            var script = scriptObject(this.helper, jsObject, null);
            return new MvcHtmlString(script);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="modelName"></param>
        /// <param name="quoteName"></param>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        public IHtmlString ViewBag<T>(T model, string modelName, bool quoteName, bool camelCase)
        {
            var jsObject = Json.SerializeObject(model, quoteName, camelCase);
            var script = scriptObject(this.helper, jsObject, modelName);
            return new MvcHtmlString(script);
        }


        private string viewBagScript<T>(T model, string modelName)
        {
            var jsObject = Json.SerializeObject(model);
            var script = scriptObject(this.helper, jsObject, modelName);
            return script;
        }

        /// <summary>
        /// returns script tag containing json serialized model  
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="jsObject"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        private string scriptObject(HtmlHelper helper, IHtmlString jsObject, string modelName)
        {
            var script = "<script type='text/javascript'>window.$$=window.$$ || {};";
            if (modelName == null)
            {
                script += "window.$$.elliptical=window.$$.elliptical || {};window.$$.elliptical.context=" + helper.Raw(jsObject) + ";</script>";
            }
            else
            {
                script += "window.$$.elliptical=window.$$.elliptical || {};window.$$.elliptical.context=window.$$.elliptical.context || {};";
                script += "window.$$.elliptical.context." + modelName + "=" + helper.Raw(jsObject) + ";</script>";
            }

            return script;
        }

    }
}