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
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IHtmlString HiddenFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            
            const string templateSection = null;
            var name = ExpressionHelper.GetExpressionText(expression);
            var tag = elementInputHiddenFor(name,templateSection);
            return new HtmlString(tag);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="templateSection"></param>
        /// <returns></returns>
        public IHtmlString HiddenFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            string templateSection)
        {
            
            var name = ExpressionHelper.GetExpressionText(expression);
            var tag = elementInputHiddenFor(name,templateSection);
            return new HtmlString(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHtmlString Hidden(string name,string value)
        {
            const string id = null;
            var tag = elementInputHidden(name,id, value);
            return new HtmlString(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHtmlString Hidden(string name,string id, string value)
        {
            var tag = elementInputHidden(name, id, value);
            return new HtmlString(tag);
        }

        private string elementInputHiddenFor(string name,string templateSection)
        {
            const string type = "hidden";
            const string tag = "input";
            const string bindAttribute = "value";
            string id = name;
            var attr = new Dictionary<string, string>();
            name = name.ToCamelCaseFromProperCase();
            attr.Add("type", type);
            var dbName = name;
            if (templateSection != null)
            {
                dbName = templateSection + "." + name;
            }

            attr.Add("name", name);
            attr.Add("id", id);
            attr.Add("data-bind", bindAttribute + ":" + dbName);
            return HtmlTagBuilder.BeginHtmlTag(tag, attr, true, true);
        }

        private string elementInputHidden(string name,string id, string value)
        {
            const string type = "hidden";
            const string tag = "input";
            value = value ?? "";
            var attr = new Dictionary<string, string>();
            if(name==null && id==null)
            {
                throw new Exception("Hidden element requires either a name or id attribute");
            }
            if(name==null)
            {
                name = id;
            }
            attr.Add("type", type);
            attr.Add("name", name);
            attr.Add("value", value);
            if(id !=null)
            {
                attr.Add("id", id);
            }
            return HtmlTagBuilder.BeginHtmlTag(tag, attr, true, true);
        }
    }
}