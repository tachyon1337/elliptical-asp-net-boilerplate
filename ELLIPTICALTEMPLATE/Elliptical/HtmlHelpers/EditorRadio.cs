using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {

        public IHtmlString RadioFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression)
        {

            const bool required = false;
            const bool readOnly = false;
            const bool enabled = true;
            const string cssClass = null;
            const string containerCssClass = null;
            var metaModel = expression.ToMetaSelectModel();
            string radioTag = elementRadioGroupFor(metaModel, required, cssClass, containerCssClass,readOnly,enabled, new Dictionary<string, string>(), RadioGroupAlignment.Horizontal);
            return new HtmlString(radioTag);
        }
        
        public IHtmlString RadioFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression, RadioGroupAlignment align)
        {

            const bool required = false;
            const bool readOnly = false;
            const bool enabled = true;
            const string cssClass = null;
            const string containerCssClass = null;
            var metaModel = expression.ToMetaSelectModel();
            string radioTag = elementRadioGroupFor(metaModel, required, cssClass,containerCssClass,readOnly,enabled, new Dictionary<string, string>(), align);
            return new HtmlString(radioTag);
        }

        public IHtmlString RadioFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression, RadioGroupAlignment align, ElementAttributes attributes)
        {

            const string containerCssClass = null;
            bool required = attributes.Required;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            string cssClass = attributes.CssClass;
            var metaModel = expression.ToMetaSelectModel();
            var attr = attributes.Attributes;
            string radioTag = elementRadioGroupFor(metaModel, required, cssClass,containerCssClass,readOnly,enabled, attr, align);
            return new HtmlString(radioTag);
        }

        public IHtmlString RadioFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression, RadioGroupAlignment align, ElementAttributes attributes, ElementAttributes containerAttributes)
        {

            string containerCssClass = containerAttributes.CssClass;
            bool required = attributes.Required;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            string cssClass = attributes.CssClass;
            var metaModel = expression.ToMetaSelectModel();
            var attr = attributes.Attributes;
            string radioTag = elementRadioGroupFor(metaModel, required, cssClass,containerCssClass,readOnly,enabled, attr, align);
            return new HtmlString(radioTag);
        }


        private string elementRadioFor(string id,string text, string name, string templateSection, string cssClass,
            bool required,bool readOnly,bool enabled, Dictionary<string, string> attributes)
        {
            name = name.ToCamelCaseFromProperCase();
            id = id.ToCamelCaseFromProperCase();
            text = text.ToCamelCaseFromProperCase();
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-radio", attributes, true);
            var input = elementInputRadioFor(id, name, cssClass, templateSection, required,readOnly,enabled);
            var lblAttr = new Dictionary<string, string> {{"for", "{" + id + "}"}};
            var lblTag = HtmlTagBuilder.BeginHtmlTag("label", lblAttr);
            lblTag += "{" + text + "}";
            lblTag += HtmlTagBuilder.EndHtmlTag("label", true);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-radio", true);

            return uiTag + input + lblTag + uiEndTag;
        }

        private string elementInputRadioFor(string id, string name, string cssClass,
            string templateSection,
            bool required, bool readOnly,bool enabled)
        {
            const string type = "radio";
            const string tag = "input";
            const string bindAttribute = "value";
           
            var attr = new Dictionary<string, string> {{"type", type}};
            var dbName = name;
            if (templateSection != null)
            {
                dbName = templateSection + "." + name;
            }

            attr.Add("name", name);
            attr.Add("id", "{" + id + "}");
            attr.Add("value", "{" + id + "}");
           
            
            if(cssClass !=null)
            {
                attr.Add("class", cssClass);
            }
           
            if (required)
            {
                attr.Add("required", "true");
            }

            if (readOnly)
            {
                attr.Add("readonly", "true");
            }
            if (!enabled)
            {
                attr.Add("@disabled", "disabled");
            }
            string output=HtmlTagBuilder.BeginHtmlTag(tag, attr,true);
            return output;
        }

        private string elementRadioGroupFor(MetaSelectModel metaModel, bool required, string cssClass,string containerCssClass,bool readOnly,bool enabled, Dictionary<string, string> attributes, RadioGroupAlignment align)
        {
            string optionsModel = metaModel.Options;
            optionsModel = optionsModel.ToCamelCaseFromProperCase();
            string textProp = metaModel.TextProperty;
            textProp = textProp.ToCamelCaseFromProperCase();
            string valueProp = metaModel.ValueProperty;
            valueProp = valueProp.ToCamelCaseFromProperCase();
            string selectedProp = metaModel.SelectedProperty;
            selectedProp = selectedProp.ToCamelCaseFromProperCase();

            containerCssClass = containerCssClass ?? "";
            var containerAttributes = new Dictionary<string, string>();
            string dbName = selectedProp;

            if(align==RadioGroupAlignment.Horizontal)
            {
                containerCssClass += containerCssClass + " wrap ";
            }
            containerCssClass += "{" + selectedProp + "_error}";
            containerAttributes.Add("class", containerCssClass);
            containerAttributes.Add("id", selectedProp);
            
            string containerTag = HtmlTagBuilder.BeginHtmlTag("ui-radio-list", containerAttributes, true);
            string templateSection = "{#" + optionsModel + "}" + Environment.NewLine;
            string liTag = HtmlTagBuilder.BeginHtmlTag("li", new Dictionary<string,string>(), true);
            string uiRadioTag = elementRadioFor(valueProp, textProp,selectedProp, null, cssClass, required,readOnly,enabled, attributes);
            string endLiTag = HtmlTagBuilder.EndHtmlTag("li", true);
            string endTemplateSection = "{/" + optionsModel + "}" + Environment.NewLine;
            string endContainerTag = HtmlTagBuilder.EndHtmlTag("ui-radio-list", true);

            string output= containerTag + templateSection + liTag + uiRadioTag + endLiTag + endTemplateSection + endContainerTag;
            return output;
        }
    }
}