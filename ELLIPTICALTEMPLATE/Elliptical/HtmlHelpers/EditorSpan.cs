using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IHtmlString SpanFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            const string cssClass = null;
            const string templateSection = null;
            const bool enableBinding = true;
            var attr = new Dictionary<string, string>();
            var spanTag = elementSpanFor(name, templateSection, cssClass,enableBinding, attr);
            return new HtmlString(spanTag);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IHtmlString SpanFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            ElementAttributes attributes)
        {
            const string templateSection = null;
            var name = ExpressionHelper.GetExpressionText(expression);
            var cssClass = attributes.CssClass;
            var attr = attributes.Attributes;
            bool enableBinding = attributes.EnableClientBinding;
            var spanTag = elementSpanFor(name, templateSection, cssClass,enableBinding, attr);
            return new HtmlString(spanTag);
        }

        public IHtmlString SpanFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            string templateSection, ElementAttributes attributes)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var cssClass = attributes.CssClass;
            var attr = attributes.Attributes;
            bool enableBinding = attributes.EnableClientBinding;
            var spanTag = elementSpanFor(name, templateSection, cssClass,enableBinding, attr);
            return new HtmlString(spanTag);
        }

        public IHtmlString Span(string text,ElementAttributes attributes)
        {
            var cssClass = attributes.CssClass;
            var attr = attributes.Attributes;
            var spanTag = elementSpan(text,cssClass, attr);
            return new HtmlString(spanTag);
        }

        private string elementSpanFor(string name, string templateSection, string cssClass,bool enableBinding,
            Dictionary<string, string> attributes)
        {
            var spanAttr = attributes;
            if (cssClass != null)
            {
                attributes.Add("class", cssClass);
            }
            name = name.ToCamelCaseFromProperCase();
            var dbName = name;
            if (templateSection != null)
            {
                dbName = templateSection + "." + name;
            }
            if(enableBinding)
            {
                spanAttr.Add("data-bind", "text:" + dbName);
            }
            var spanTag = HtmlTagBuilder.BeginHtmlTag("span", spanAttr);
            spanTag += "{" + name + "} &nbsp;";
            var spanEndTag = HtmlTagBuilder.EndHtmlTag("span", true);

            return spanTag + spanEndTag;
        }

        private string elementSpan(string text,string cssClass,Dictionary<string, string> attributes)
        {
            var spanAttr = attributes;
            if (cssClass != null)
            {
                attributes.Add("class", cssClass);
            }

            var spanTag = HtmlTagBuilder.BeginHtmlTag("span", spanAttr);
            spanTag += text;
            var spanEndTag = HtmlTagBuilder.EndHtmlTag("span", true);

            return spanTag + spanEndTag;
        }
    }
}