using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        public IHtmlString InputIconFor<TModel, TProperty>(TModel model,
            Expression<Func<TModel, TProperty>> expression, string btnValue, ElementAttributes attributes)
        {
            
            const string templateSection = null;
            const string btnCssClass = null;
            const string iconCssClass = null;
            bool required = attributes.Required;
            string value = attributes.Value;
            string cssClass = attributes.CssClass;
            string placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            string controller = attributes.Controller;
            var name = ExpressionHelper.GetExpressionText(expression);
            var inputTag = elementInputIconFor(name, templateSection, required, cssClass, placeholder, value,readOnly,enabled,btnValue,
                btnCssClass, iconCssClass,controller);
            return new HtmlString(inputTag);
        }

        public IHtmlString InputIconFor<TModel, TProperty>(TModel model,
            Expression<Func<TModel, TProperty>> expression, string btnValue,string templateSection, ElementAttributes attributes,
            string btnCssClass, string iconCssClass)
        {
            
            bool required = attributes.Required;
            string value = attributes.Value;
            var name = ExpressionHelper.GetExpressionText(expression);
            string cssClass = attributes.CssClass;
            string placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            string controller = attributes.Controller;
            var inputTag = elementInputIconFor(name, templateSection, required, cssClass, placeholder, value,readOnly,enabled,btnValue,
                btnCssClass, iconCssClass,controller);
            return new HtmlString(inputTag);
        }

        public IHtmlString InputIcon(string btnValue, ElementAttributes attributes)
        {
            
            const string templateSection = null;
            const string btnCssClass = null;
            const string iconCssClass = null;
            string name = attributes.Name;
            string value = attributes.Value;
            bool required = attributes.Required;
            string cssClass = attributes.CssClass;
            string placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            string controller = attributes.Controller;
            var inputTag = elementInputIcon(name, templateSection, required, cssClass, placeholder, value,readOnly,enabled,btnValue,
                btnCssClass, iconCssClass,controller);
            return new HtmlString(inputTag);
        }

        public IHtmlString InputIcon(string btnValue, ElementAttributes attributes, string btnCssClass, string iconCssClass)
        {
            
            const string templateSection = null;
            string name = attributes.Name;
            string value = attributes.Value;
            bool required = attributes.Required;
            string cssClass = attributes.CssClass;
            string placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            string controller = attributes.Controller;
            var inputTag = elementInputIcon(name, templateSection, required, cssClass, placeholder, value,readOnly,enabled,btnValue,
                btnCssClass, iconCssClass,controller);
            return new HtmlString(inputTag);
        }

        private string elementInputIconFor(string name, string templateSection, bool required,
            string cssClass, string placeholder,string value,bool readOnly, bool enabled,string btnValue, string btnCssClass, string iconCssClass,string controller)
        {
            const string tag = "input";
            var uiAttr = new Dictionary<string, string>();
            var btnAttr = new Dictionary<string, string>();
            var spanAttr = new Dictionary<string, string>();
            if (cssClass != null)
            {
                uiAttr.Add("class", cssClass);
            }
            if (controller != null)
            {
                uiAttr.Add("controller", controller);
            }
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-input-addon", uiAttr, true);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-input-addon", true);
            var input = elementInputFor(tag, "text", name, templateSection, null, placeholder, required,value,readOnly,enabled,
                new Dictionary<string, string>());

            if (btnCssClass != null)
            {
                btnAttr.Add("class", btnCssClass);
            }

            var btnTag = HtmlTagBuilder.BeginHtmlTag("ui-button", btnAttr, true);
            var btnEndTag = HtmlTagBuilder.EndHtmlTag("ui-button", true);

            if (iconCssClass != null)
            {
                spanAttr.Add("class", iconCssClass);
            }

            var spanTag = HtmlTagBuilder.BeginHtmlTag("span", spanAttr, true);

            if (btnValue != null)
            {
                spanTag += btnValue;
            }

            var spanEndTag = HtmlTagBuilder.EndHtmlTag("span", true);

            return uiTag + input + btnTag + spanTag + spanEndTag + btnEndTag + uiEndTag;
        }

        private string elementInputIcon(string name, string id, bool required,
            string cssClass, string placeholder, string value,bool readOnly,bool enabled, string btnValue, string btnCssClass, string iconCssClass,string controller)
        {
            const string tag = "input";
            var uiAttr = new Dictionary<string, string>();
            var btnAttr = new Dictionary<string, string>();
            var spanAttr = new Dictionary<string, string>();
            if (cssClass != null)
            {
                uiAttr.Add("class", cssClass);
            }
            if (controller != null)
            {
                uiAttr.Add("controller", controller);
            }
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-input-addon", uiAttr, true);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-input-addon", true);
            var input = elementInput(tag, "text", name, id, null, placeholder, required, value,readOnly,enabled,new Dictionary<string, string>());

            if (btnCssClass != null)
            {
                btnAttr.Add("class", btnCssClass);
            }

            var btnTag = HtmlTagBuilder.BeginHtmlTag("ui-button", btnAttr, true);
            var btnEndTag = HtmlTagBuilder.EndHtmlTag("ui-button", true);

            if (iconCssClass != null)
            {
                spanAttr.Add("class", iconCssClass);
            }

            var spanTag = HtmlTagBuilder.BeginHtmlTag("span", spanAttr, true);

            if (btnValue != null)
            {
                spanTag += btnValue;
            }

            var spanEndTag = HtmlTagBuilder.EndHtmlTag("span", true);

            return uiTag + input + btnTag + spanTag + spanEndTag + btnEndTag + uiEndTag;
        }
    }
}