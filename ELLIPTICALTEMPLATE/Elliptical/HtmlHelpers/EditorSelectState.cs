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
        public IHtmlString SelectStateFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            const string tagName = "state-select";
            var attributes = new FormAttributes();
            var name = ExpressionHelper.GetExpressionText(expression);
            var tag = elementSelectStateFor(tagName,name,attributes);
            return new HtmlString(tag);
        }

        public IHtmlString SelectStateFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, FormAttributes attributes)
        {
            const string tagName = "state-select";
            var name = ExpressionHelper.GetExpressionText(expression);
            var tag = elementSelectStateFor(tagName, name, attributes);
            return new HtmlString(tag);
        }

        public IHtmlString SelectState(string name)
        {
            const string tagName = "state-select";
            var attributes = new FormAttributes();
            var tag = elementSelectState(tagName, name, attributes);
            return new HtmlString(tag);
        }

        public IHtmlString SelectState(string name, FormAttributes attributes)
        {
            const string tagName = "state-select";
            var tag = elementSelectState(tagName, name, attributes);
            return new HtmlString(tag);
        }


        private string elementSelectStateFor(string tagName,string name,FormAttributes attributes)
        {
            const string templateSection=null;
            name = name.ToCamelCaseFromProperCase();
            var dictionary = new Dictionary<string, string> {{"html5-imported", "true"}, {"id", name}, {"field", name}};
            string cssClass = attributes.CssClass ?? "";
            cssClass += " {" + name + "_error}";
            dictionary.Add("class", cssClass);
            string import = html5ImportString("state-select", null);
            string tag = HtmlTagBuilder.BeginHtmlTag(tagName, dictionary, true,true);
            string hiddenTag = elementInputHiddenFor(name, templateSection);
            return import + tag + hiddenTag;
        }

        private string elementSelectState(string tagName, string name, FormAttributes attributes)
        {
            const string templateSection = null;
            name = name.ToCamelCaseFromProperCase();
            var dictionary = new Dictionary<string, string> { { "html5-imported", "true" }, { "id", name }, { "field", name } };
            if(attributes.CssClass !=null)
            {
                dictionary.Add("class", attributes.CssClass);
            }
            string import = html5ImportString("state-select", null);
            string tag = HtmlTagBuilder.BeginHtmlTag(tagName, dictionary, true, true);
            string hiddenTag = elementInputHiddenFor(name, templateSection);
            return import + tag + hiddenTag;
        }
    }
}