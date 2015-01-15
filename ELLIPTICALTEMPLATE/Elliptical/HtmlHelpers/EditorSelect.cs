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
        public IHtmlString SelectFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression)
        {

            const bool required = false;
            const bool enabled = true;
            const string cssClass = null;
            const string placeholder = null;
            const string templateSection = null;
            var metaModel = expression.ToMetaSelectModel();
            string selectTag = this.elementSelectFor(metaModel, templateSection, required, cssClass, placeholder,enabled, new Dictionary<string, string>());
            return new HtmlString(selectTag);
        }

        public IHtmlString SelectFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression, ElementAttributes attributes)
        {

            const string templateSection = null;
            bool required = attributes.Required;
            bool enabled = attributes.Enabled;
            string cssClass = attributes.CssClass;
            string placeholder = attributes.Placeholder;
            var metaModel = expression.ToMetaSelectModel();
            string selectTag = this.elementSelectFor(metaModel, templateSection, required, cssClass, placeholder,enabled, new Dictionary<string, string>());
            return new HtmlString(selectTag);
        }

        public IHtmlString SelectFor<TModel, TResult>(TModel model, Expression<Func<TModel, TResult>> expression,string templateSection, ElementAttributes attributes)
        {

            bool required = attributes.Required;
            bool enabled = attributes.Enabled;
            string cssClass = attributes.CssClass;
            string placeholder = attributes.Placeholder;
            var metaModel = expression.ToMetaSelectModel();
            string selectTag = this.elementSelectFor(metaModel, templateSection, required, cssClass, placeholder,enabled, new Dictionary<string, string>());
            return new HtmlString(selectTag);
        }

        private string elementSelectFor(MetaSelectModel metaModel, string templateSection, bool required, string cssClass, string placeholder,bool enabled, Dictionary<string, string> attributes)
        {
            string optionsModel = metaModel.Options;
            optionsModel = optionsModel.ToCamelCaseFromProperCase();
            string textProp = metaModel.TextProperty;
            textProp = textProp.ToCamelCaseFromProperCase();
            string valueProp = metaModel.ValueProperty;
            valueProp = valueProp.ToCamelCaseFromProperCase();
            string selectedProp = metaModel.SelectedProperty;
            selectedProp = selectedProp.ToCamelCaseFromProperCase();

            if (placeholder == null)
            {
                placeholder = selectedProp;
                placeholder = placeholder.ToPhraseCase();
            }
            var selAttributes = new Dictionary<string, string>();
            string dbName = selectedProp;
            if (templateSection != null)
            {
                dbName = templateSection + "." + selectedProp;
            }
           
            if(required)
            {
                 selAttributes.Add("required", "true");
            }

            if (!enabled)
            {
                selAttributes.Add("@disabled", "disabled");
                attributes.Add("@disabled", "disabled");
                attributes.Add("class", "disabled");
            }
            else
            {
                cssClass = cssClass ?? "";
                cssClass = cssClass + " {" + selectedProp + "_error}";
                attributes.Add("class", cssClass);
                attributes.Add("ea-id", selectedProp);
            }

            selAttributes.Add("name", selectedProp);
            selAttributes.Add("id", selectedProp);
            selAttributes.Add("data-bind", "value:" + dbName);

            string uiSelectTag = HtmlTagBuilder.BeginHtmlTag("ui-select", attributes, true);
            string selTag = HtmlTagBuilder.BeginHtmlTag("select", selAttributes,true);
            string options = "";
            if(placeholder !=null)
            {
                options += "<option value='Select'>" + placeholder.ToOptionSelect() + "</option>" + Environment.NewLine;
            }
            options += "{#" + optionsModel + "}" + Environment.NewLine;
            options += HtmlTagBuilder.BeginHtmlTag("option", new Dictionary<string, string> { { "value", "{" + valueProp + "}" } });
            options += "{" + textProp + "}";
            options += HtmlTagBuilder.EndHtmlTag("option", true);
            options += "{/" + optionsModel + "}" + Environment.NewLine;
            
            string selEndTag = HtmlTagBuilder.EndHtmlTag("select", true);
            string uiSelectEndTag = HtmlTagBuilder.EndHtmlTag("ui-select", true);

            return uiSelectTag + selTag + options + selEndTag + uiSelectEndTag;
        }

    }
}