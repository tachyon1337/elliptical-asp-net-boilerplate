using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {

        /// <summary>
        /// </summary>
        /// <param name="btnValue"></param>
        /// <returns></returns>
        public IHtmlString Submit(string btnValue)
        {
            const bool submitLabel = true;
            const string cssClass = null;
            const bool enabled = true;
            return new HtmlString(elementSubmit(cssClass, btnValue,enabled,submitLabel));
        }

        /// <summary>
        /// </summary>
        /// <param name="btnValue"></param>
        /// <param name="btnCssClass"></param>
        /// <returns></returns>
        public IHtmlString Submit(string btnValue, string btnCssClass)
        {
            const bool submitLabel = true;
            const bool enabled = true;
            return new HtmlString(elementSubmit(btnCssClass, btnValue,enabled,submitLabel));
        }

        public IHtmlString Submit(string btnValue, bool enabled)
        {
            const bool submitLabel = true;
            const string cssClass = null;
            return new HtmlString(elementSubmit(cssClass, btnValue, enabled,submitLabel));
        }

        public IHtmlString Submit(string btnValue, string btnCssClass,bool enabled)
        {
            const bool submitLabel = true;
            return new HtmlString(elementSubmit(btnCssClass, btnValue,enabled,submitLabel));
        }

        public IHtmlString Submit(string btnValue, string btnCssClass, bool enabled,bool templateBind)
        {
            return new HtmlString(elementSubmit(btnCssClass, btnValue, enabled, templateBind));
        }

        public IHtmlString Submit(string btnValue, bool enabled, bool templateBind)
        {
            const string cssClass = null;
            return new HtmlString(elementSubmit(cssClass, btnValue, enabled, templateBind));
        }

        private string elementSubmit(string cssClass, string btnValue, bool enabled, bool submitLabel)
        {
            var uiAttr = new Dictionary<string, string>();
            var btnAttr = new Dictionary<string, string>();
            uiAttr.Add("class", "full-width");
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-flex-container", uiAttr, true);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-flex-container", true);

            if (cssClass == null)
            {
                cssClass = "ui-button secondary large margin";
            }
            else
            {
                cssClass = "ui-button " + cssClass;
            }
            btnAttr.Add("class", cssClass);
            if(!enabled)
            {
                btnAttr.Add("@disabled", "disabled");
            }
            var btnTag = HtmlTagBuilder.BeginHtmlTag("button", btnAttr);
            btnTag += btnValue;
            var btnEndTag = HtmlTagBuilder.EndHtmlTag("button", true);
            if(submitLabel)
            {
                var templateSection = "{#submitLabel}" + Environment.NewLine;
                var endtemplateSection = "{/submitLabel}" + Environment.NewLine;
                var label = "<label class='ui-semantic-label {css} {cssDisplay}'>{message|s}&nbsp;</label>" +
                            Environment.NewLine;
                return uiTag + btnTag + btnEndTag + uiEndTag + templateSection + label + endtemplateSection;
            }
            else
            {
                return uiTag + btnTag + btnEndTag + uiEndTag;
            }
           
        }

    }
}