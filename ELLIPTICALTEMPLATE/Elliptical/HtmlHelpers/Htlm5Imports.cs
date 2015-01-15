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
        ///  HTML5 Import Tag
        /// </summary>
        /// <param name="url">Relative Url path to the imported document</param>
        /// <returns>MvcHtmlString</returns>
        public IHtmlString HTML5Import(string url)
        {
            return hTML5Import(url, null);
        }


        /// <summary>
        /// HTML5 Import Tag
        /// </summary>
        /// <param name="url">Relative Url path to the imported document</param>
        /// <param name="property">the tag property value...required if placed in the HTML body</param>
        /// <returns>MvcHtmlString</returns>
        public IHtmlString HTML5Import(string url, string id)
        {
            return hTML5Import(url, id);
        }


        /// <summary>
        /// Serializes a model to a HTML5 imported document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="scope"></param>
        /// <param name="url"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public IHtmlString Import<T>(T model, string scope, string url)
        {
            string property = "elliptical";
            url = this.getImportUrl(url);
            var jsObject = Json.SerializeObject(model);
            var script = scriptObject(this.helper, jsObject, scope);
            string link = "<link rel='import' href='" + url + "' property='" + property + "'/>";
            return new HtmlString(script + "\n" + link);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="scope"></param>
        /// <param name="quoteName"></param>
        /// <param name="camelCase"></param>
        /// <param name="url"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public IHtmlString Import<T>(T model, string scope, bool quoteName, bool camelCase, string url)
        {
            string import = this.elementImport(model, scope, quoteName, camelCase, url);
            return new HtmlString(import);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="scope"></param>
        /// <param name="quoteName"></param>
        /// <param name="camelCase"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private string elementImport<T>(T model, string scope, bool quoteName, bool camelCase, string url)
        {
            string property = "elliptical";
            url = this.getImportUrl(url);
            var jsObject = Json.SerializeObject(model, quoteName, camelCase);
            var script = scriptObject(this.helper, jsObject, scope);
            string link = "<link rel='import' href='" + url + "' property='" + property + "'/>";
            return script + Environment.NewLine + link + Environment.NewLine;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IHtmlString hTML5Import(string url, string id)
        {
            string link = this.html5ImportString(url, id);
            return new MvcHtmlString(link);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string html5ImportString(string url, string id)
        {
            string property = "elliptical";
            string link;
            url = getImportUrl(url);
            if (property == null && id == null)
            {
                link = "<link rel='import' href='" + url + "'/>";
            }
            else if (property == null)
            {
                link = "<link rel='import' href='" + url + "' id='" + id + "'/>";
            }
            else if (id == null)
            {
                link = "<link rel='import' href='" + url + "' property='" + property + "'/>";
            }
            else
            {
                link = "<link rel='import' href='" + url + "' id='" + id + "' property='" + property + "'/>";
            }

            return link;
        }

        private string getImportUrl(string url)
        {
            const string componentRoot = "/Content/components/";
            string firstChr = url.FirstChars(1);
            bool hasSlash = url.InString("/");
            bool hasDot = url.InString(".");
            if (!hasDot && !hasSlash)
            {
                return componentRoot + url + "/" + url + ".html";
            }
            else if (firstChr != "/" && !hasDot && hasSlash)
            {
                var file = url.ToStringLastPart('/');
                return componentRoot + url + "/" + file + ".html";
            }
            else if (firstChr != "/")
            {
                return componentRoot + "/" + url;
            }
            else
            {
                return url;
            }
        }
    }
}