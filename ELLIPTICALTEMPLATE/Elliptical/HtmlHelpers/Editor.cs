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
        /// Editor For 2-way data bound Form Element
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IHtmlString EditorFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            HtmlFormElement type)
        {
            const bool readOnly=false;
            const bool enabled = true;
            const string templateSection = null;
            const bool required = false;
            const string cssClass = null;
            const string placeholder = null;
            const string value = null;
            var name = ExpressionHelper.GetExpressionText(expression);
            var attr = new Dictionary<string, string>();
            var tag = editorFor(name, type, templateSection, cssClass, placeholder, required,value,readOnly,enabled, attr);

            return new HtmlString(tag);
        }

        /// <summary>
        /// Editor For 2-way data bound Form Element
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <param name="templateSection"></param>
        /// <returns></returns>
        public IHtmlString EditorFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            HtmlFormElement type, string templateSection)
        {
            const bool readOnly = false;
            const bool required = false;
            const bool enabled = true;
            const string cssClass = null;
            const string placeholder = null;
            const string value = null;
            var name = ExpressionHelper.GetExpressionText(expression);
            var attr = new Dictionary<string, string>();
            var tag = editorFor(name, type, templateSection, cssClass, placeholder, required,value,readOnly,enabled, attr);

            return new HtmlString(tag);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IHtmlString EditorFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            HtmlFormElement type, ElementAttributes attributes)
        {
            const string templateSection = null;
            const string value = null;
            var name = ExpressionHelper.GetExpressionText(expression);
            var required = attributes.Required;
            var cssClass = attributes.CssClass;
            var placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            var attr = attributes.Attributes;
            var tag = editorFor(name, type, templateSection, cssClass, placeholder, required,value,readOnly,enabled, attr);

            return new HtmlString(tag);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <param name="templateSection"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IHtmlString EditorFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            HtmlFormElement type, string templateSection, ElementAttributes attributes)
        {
            
            const string value = null;
            var name = ExpressionHelper.GetExpressionText(expression);
            var required = attributes.Required;
            var cssClass = attributes.CssClass;
            var placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            var attr = attributes.Attributes;
            var tag = editorFor(name, type, templateSection, cssClass, placeholder, required,value,readOnly,enabled, attr);

            return new HtmlString(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IHtmlString Editor(HtmlFormElement type, ElementAttributes attributes)
        {
            
            string value = attributes.Value;
            string id = attributes.Id;
            var name = attributes.Name;
            var required = attributes.Required;
            var cssClass = attributes.CssClass;
            var placeholder = attributes.Placeholder;
            bool readOnly = attributes.ReadOnly;
            bool enabled = attributes.Enabled;
            var attr = attributes.Attributes;
            var tag = editor(name, id, type, cssClass, placeholder, required, value,readOnly,enabled, attr);

            return new HtmlString(tag);
        }


        private string editorFor(string name, HtmlFormElement type, string templateSection, string cssClass,
            string placeholder, bool required,string value,bool readOnly,bool enabled, Dictionary<string, string> attributes)
        {
            switch (type)
            {
                case HtmlFormElement.TextArea:
                    return elementInputFor("textarea", type.ToString(), name, templateSection, cssClass, placeholder,
                        required,value,readOnly,enabled, attributes);

                case HtmlFormElement.Checkbox:
                    return elementCheckboxFor("input", type.ToString(), name, templateSection, cssClass, placeholder,
                        required,value,readOnly, enabled,attributes);

                case HtmlFormElement.InputIcon:
                    string btnValue = null;
                    string btnCssClass = null;
                    string iconCssClass = null;
                    string controller = null;
                    if (attributes.ContainsKey("btn-value"))
                    {
                        btnValue = attributes["btn-value"];
                    }
                    if (attributes.ContainsKey("btn-class"))
                    {
                        btnCssClass = attributes["btn-class"];
                    }
                    if (attributes.ContainsKey("icon-class"))
                    {
                        iconCssClass = attributes["icon-class"];
                    }
                    if (attributes.ContainsKey("controller"))
                    {
                        controller = attributes["controller"];
                    }
                    return elementInputIconFor(name, templateSection, required, cssClass, placeholder, value,readOnly,enabled,btnValue,
                        btnCssClass, iconCssClass,controller);


                default:
                    return elementInputFor("input", type.ToString(), name, templateSection, cssClass, placeholder,
                        required,value,readOnly,enabled, attributes);
            }
        }

        
        private string editor(string name,string id, HtmlFormElement type, string cssClass,
           string placeholder, bool required, string value, bool readOnly,bool enabled,Dictionary<string, string> attributes)
        {
            switch (type)
            {
                case HtmlFormElement.TextArea:
                    return elementInput("textarea", type.ToString(), name, id, cssClass, placeholder,
                        required, value,readOnly,enabled, attributes);

                case HtmlFormElement.Checkbox:
                    return elementCheckbox("input", type.ToString(), name, id, cssClass, placeholder,
                        required, value,readOnly,enabled, attributes);

                case HtmlFormElement.InputIcon:
                    string btnValue = null;
                    string btnCssClass = null;
                    string iconCssClass = null;
                    string controller = null;
                    if (attributes.ContainsKey("btn-value"))
                    {
                        btnValue = attributes["btn-value"];
                    }
                    if (attributes.ContainsKey("btn-class"))
                    {
                        btnCssClass = attributes["btn-class"];
                    }
                    if (attributes.ContainsKey("icon-class"))
                    {
                        iconCssClass = attributes["icon-class"];
                    }
                    if (attributes.ContainsKey("controller"))
                    {
                        controller = attributes["controller"];
                    }
                    return elementInputIcon(name, id, required, cssClass, placeholder, value,readOnly,enabled,btnValue,
                        btnCssClass, iconCssClass,controller);


                default:
                    return elementInput("input", type.ToString(), name, id, cssClass, placeholder,
                        required, value,readOnly, enabled,attributes);
            }
        }
    }

}