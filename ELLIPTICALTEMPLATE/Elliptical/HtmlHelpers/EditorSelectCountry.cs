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
        public IHtmlString SelectCountryFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            const string tagName = "country-select";
            var attributes = new FormAttributes();
            var name = ExpressionHelper.GetExpressionText(expression);
            var tag = elementSelectCountryFor(tagName, name, attributes);
            return new HtmlString(tag);
        }

        public IHtmlString SelectCountryFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, FormAttributes attributes)
        {
            const string tagName = "country-select";
            var name = ExpressionHelper.GetExpressionText(expression);
            var tag = elementSelectCountryFor(tagName, name, attributes);
            return new HtmlString(tag);
        }

        public IHtmlString SelectCountry(string name)
        {
            const string tagName = "country-select";
            var attributes = new FormAttributes();
            var tag = elementSelectCountry(tagName, name, attributes);
            return new HtmlString(tag);
        }

        public IHtmlString SelectCountry(string name, FormAttributes attributes)
        {
            const string tagName = "country-select";
            var tag = elementSelectCountry(tagName, name, attributes);
            return new HtmlString(tag);
        }


        private string elementSelectCountryFor(string tagName, string name, FormAttributes attributes)
        {
            const string templateSection = null;
            name = name.ToCamelCaseFromProperCase();
            var dictionary = new Dictionary<string, string> { { "html5-imported", "true" }, { "id", name }, { "field", name } };
            string cssClass = attributes.CssClass ?? "";
            cssClass += " {" + name + "_error}";
            dictionary.Add("class", cssClass);
            string import = html5ImportString("country-select", null);
            string tag = HtmlTagBuilder.BeginHtmlTag(tagName, dictionary, true, true);
            string hiddenTag = elementInputHiddenFor(name, templateSection);
            return import + tag + hiddenTag;
        }

        private string elementSelectCountry(string tagName, string name, FormAttributes attributes)
        {
            const string templateSection = null;
            name = name.ToCamelCaseFromProperCase();
            var dictionary = new Dictionary<string, string> { { "html5-imported", "true" }, { "id", name }, { "field", name } };
            if (attributes.CssClass != null)
            {
                dictionary.Add("class", attributes.CssClass);
            }
            string import = html5ImportString("country-select", null);
            string tag = HtmlTagBuilder.BeginHtmlTag(tagName, dictionary, true, true);
            string hiddenTag = elementInputHiddenFor(name, templateSection);
            return import + tag + hiddenTag;
        }
    }
}