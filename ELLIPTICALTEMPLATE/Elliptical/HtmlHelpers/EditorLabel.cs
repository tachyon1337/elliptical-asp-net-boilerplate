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
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IHtmlString LabelFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            const string cssClass = null;
            var attr = new Dictionary<string, string>();
            var labelTag = elementLabelFor(name, cssClass, attr);
            return new HtmlString(labelTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IHtmlString LabelFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            ElementAttributes attributes)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var cssClass = attributes.CssClass;
            var attr = attributes.Attributes;
            var labelTag = elementLabelFor(name, cssClass, attr);
            return new HtmlString(labelTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IHtmlString Label(string name)
        {
            const string cssClass = null;
            var attr = new Dictionary<string, string>();
            var labelTag = elementLabelFor(name, cssClass, attr);
            return new HtmlString(labelTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IHtmlString Label(string name,ElementAttributes attributes)
        {
            var cssClass = attributes.CssClass;
            var attr = attributes.Attributes;
            var labelTag = elementLabelFor(name, cssClass, attr);
            return new HtmlString(labelTag);
        }

        private string elementLabelFor(string name, string cssClass, Dictionary<string, string> attributes)
        {
            var labelAttr = attributes;
            name = name.ToCamelCaseFromProperCase();
            var text = name.ToPhraseCase();
           
            if (cssClass != null)
            {
                attributes.Add("class", cssClass);
            }

            attributes.Add("for", name);
            var labelTag = HtmlTagBuilder.BeginHtmlTag("label", labelAttr);
            labelTag += text;
            var labelEndTag = HtmlTagBuilder.EndHtmlTag("label", true);

            return labelTag + labelEndTag;
        }
    }
}