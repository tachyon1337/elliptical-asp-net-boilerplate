using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IHtmlString AntiForgeryToken()
        {
            //var helper = this.helper;
            var token = helper.AntiForgeryToken().ToHtmlString();
            return new HtmlString(token);
        }

        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public IHtmlString AntiForgeryToken(string elementId)
        {
            return new HtmlString(antiForgeryToken(elementId));
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IHtmlString ValidationSummary()
        {
            return new HtmlString(validationSummary(null, null));
        }

        public IHtmlString ValidationSummary(string message)
        {
            return new HtmlString(validationSummary(null, message));
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public IHtmlString ValidationSummary(string message, string cssClass)
        {
            return new HtmlString(validationSummary(cssClass, message));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IDisposable BeginForm<T>(T model, FormAttributes attributes)
        {
            //var helper = this.helper;
            bool writeFormTag = includeFormTag(attributes);
            var tagName = "ui-form";
            var viewBagScript = this.viewBagScript(model, attributes.Scope);
            var validationSummary = this.validationSummary(null, null);
            var writer = helper.ViewContext.Writer;
            var output = viewBagScript + Environment.NewLine;
            output += validationSummary + Environment.NewLine;
            output += buildCustomFormElement(model, tagName, attributes,writeFormTag);

            writer.Write(output);
            return new FormContainer(writer, tagName, writeFormTag);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="tagName"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IDisposable BeginForm<T>(T model, string tagName, FormAttributes attributes)
        {
            //var helper = this.helper;
            bool writeFormTag = includeFormTag(attributes);
            var viewBagScript = this.viewBagScript(model, attributes.Scope);
            var validationSummary = this.validationSummary(null, null);
            var writer = helper.ViewContext.Writer;
            var output = viewBagScript + Environment.NewLine;
            output += validationSummary + Environment.NewLine;
            output += buildCustomFormElement(model, tagName, attributes,writeFormTag);

            writer.Write(output);
           
            return new FormContainer(writer, tagName, writeFormTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IDisposable BeginForm(FormAttributes attributes)
        {
            string tagName = "ui-form";
            bool writeFormTag = includeFormTag(attributes);
            if (attributes.Scope == null)
            {
                attributes.EnableClientBinding = false;
            }
            var validationSummary = this.validationSummary(null, null);
            var writer = helper.ViewContext.Writer;
            string output = validationSummary + Environment.NewLine;
            output += buildCustomFormElement(tagName, attributes, writeFormTag);

            writer.Write(output);

            return new FormContainer(writer, tagName, writeFormTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IDisposable BeginForm(string tagName, FormAttributes attributes)
        {
            bool writeFormTag = includeFormTag(attributes);
            if (attributes.Scope == null)
            {
                attributes.EnableClientBinding = false;
            }
            var validationSummary = this.validationSummary(null, null);
            var writer = helper.ViewContext.Writer;
            string output = validationSummary + Environment.NewLine;
            output += buildCustomFormElement(tagName, attributes, writeFormTag);
            writer.Write(output);

            return new FormContainer(writer, tagName, writeFormTag);
        }


        private string buildCustomFormElement<T>(T model, string tagName, FormAttributes attributes, bool writeFormTag)
        {
           
            if (attributes.Schema == null)
            {
                attributes.Schema = model.GetType().ToString().ToStringLastPart('.');
            }

            return buildCustomFormElement(tagName, attributes, writeFormTag);
        }
       

        private string buildCustomFormElement(string tagName, FormAttributes attributes, bool writeFormTag)
        {
            var c = new CustomFormElement();
            var submit = true;
            string service = null;
            var name = attributes.Name;
            string schema = attributes.Schema;
            string role = attributes.Role;
            var processingMessage = attributes.ProcessingMessage;
            var successMessage = attributes.SuccessMessage;
            var redirect = attributes.ReturnUrl;
            var redirectLabel = attributes.RedirectLabel;
            var slideIn = attributes.SlideNotification;
            var importLink = "";
            var formTag = "";
            var customTag = "";
            var forgeryToken = "";
            var elementId = attributes.Id;
            var tagOutput = "";


            if (attributes.Action == null)
            {
                submit = false;
                role = "form";
                service = attributes.Service ?? attributes.Schema;
            }else
            {
                if(redirect !=null)
                {
                    attributes.Action = attributes.Action + "?" + HttpUtility.UrlEncode(redirect);
                }
            }

            
            if(attributes.ImportHref !=null)
            {
                importLink = html5ImportString(attributes.ImportHref, null);
            }

            if (tagName == "ui-form")
            {
                if (name == null)
                {
                    name = "form-component";
                }
                if (attributes.ImportHref == null)
                {
                    attributes.ImportHref = "/Content/Components/ui-form/ui-form.html";
                }
                
                importLink = html5ImportString(attributes.ImportHref, null) + Environment.NewLine;
            }

            if (writeFormTag)
            {
                forgeryToken = helper.AntiForgeryToken().ToHtmlString();
                
                formTag = beginFormTag(attributes.Action, attributes.Method, attributes.Name,attributes.EnableClientBinding);
                c.Tag = tagName;
                c.Name = name;
                c.Scope = attributes.Scope;
                c.Service = service;
                c.Schema = schema;
                c.CssClass = attributes.CssClass;
                c.Role = role;
                c.ProcessingMessage = processingMessage;
                c.SuccessMessage = successMessage;
                c.Redirect = redirect;
                c.RedirectLabel = redirectLabel;
                c.SlideInNotify = slideIn;
                c.ActionSubmit = submit;
                c.Action = attributes.Action;
                c.Channel = attributes.Channel;
                c.Event = attributes.Event;
                customTag = beginCustomFormTag(c);

                tagOutput = importLink + customTag + formTag + forgeryToken + Environment.NewLine;
            }
            else
            {
                c.Tag = tagName;
                c.Name = name;
                c.Scope = attributes.Scope;
                c.Service = service;
                c.Schema = schema;
                c.CssClass = attributes.CssClass;
                c.Role = role;
                c.ProcessingMessage = processingMessage;
                c.SuccessMessage = successMessage;
                c.Redirect = redirect;
                c.RedirectLabel = redirectLabel;
                c.SlideInNotify = slideIn;
                c.ActionSubmit = submit;
                c.Action = attributes.Action;
                c.Channel = attributes.Channel;
                c.Event = attributes.Event;

                if (elementId == null)
                {
                    elementId = "form" + Utils.GenerateRandomString(4);
                }
                c.Id = elementId;
                forgeryToken = antiForgeryToken(elementId);
                customTag = beginCustomFormTag(c);

                tagOutput = importLink + forgeryToken + Environment.NewLine + customTag + Environment.NewLine;
            }

            return tagOutput;
        }



        private bool includeFormTag(FormAttributes attributes)
        {
            if(attributes.IncludeFormTag)
            {
                return true;
            }
            if (attributes.ImportHref == null)
            {
                return true;
            }
            return false;
        }

        private string beginCustomFormTag(CustomFormElement c)
        {
            var id = c.Id;
            var tag = c.Tag;
            var name = c.Name;
            var scope = c.Scope;
            var service = c.Service;
            var schema = c.Schema;
            var cssClass = c.CssClass;
            var role = c.Role;
            var processingMessage = c.ProcessingMessage;
            var successMessage = c.SuccessMessage;
            var redirect = c.Redirect;
            var redirectLabel = c.RedirectLabel;
            var slideIn = c.SlideInNotify;
            var submit = c.ActionSubmit;
            var action = c.Action;
            var channel = c.Channel;


            var dictionary = new Dictionary<string, string>();
            if (id != null)
            {
                dictionary.Add("id", id);
            }
            if (name != null)
            {
                dictionary.Add("name", name);
            }
            if (scope != null)
            {
                dictionary.Add("scope", scope);
            }
            if (service != null)
            {
                dictionary.Add("service", service);
            }
            if (action != null)
            {
                dictionary.Add("action", action);
            }
            if (schema != null)
            {
                dictionary.Add("schema", schema);
            }
            if (cssClass != null)
            {
                dictionary.Add("class", cssClass);
            }
            if (role != null)
            {
                dictionary.Add("role", role);
            }
            if (processingMessage != null)
            {
                dictionary.Add("processing-message", processingMessage);
            }
            if (successMessage != null)
            {
                dictionary.Add("success-message", successMessage);
            }
            if (redirect != null)
            {
                dictionary.Add("return-url", redirect);
            }
            if (redirectLabel != null)
            {
                dictionary.Add("redirect-label", redirectLabel);
            }

            if (slideIn)
            {
                dictionary.Add("slide-in", "true");
            }
            if (submit)
            {
                dictionary.Add("action-submit", "true");
            }
            if(channel !=null)
            {
                dictionary.Add("channel", channel);
            }
            if(c.Event !=null)
            {
                dictionary.Add("event", c.Event);
            }

            return HtmlTagBuilder.BeginHtmlTag(tag, dictionary, true);
        }

        private string beginFormTag(string action, HtmlFormMethod method, string name,bool enableClientBinding)
        {
            var tag = "form";
            var strMethod = method.ToString();
            var id = "fragment-" + Utils.GenerateRandomString(6);
            System.Diagnostics.Trace.WriteLine(id);
            var dictionary = new Dictionary<string, string>();
            if (name != null)
            {
                dictionary.Add("name", name);
            }
            if (action != null)
            {
                dictionary.Add("action", action);
            }
            if(!enableClientBinding)
            {
                dictionary.Add("class", "visible");
            }
            dictionary.Add("method", strMethod);
            dictionary.Add("id", id);
            return HtmlTagBuilder.BeginHtmlTag(tag, dictionary, true);
        }

        private string antiForgeryToken(string elementId)
        {
            //var helper = this.helper;
            var token = helper.AntiForgeryToken().ToHtmlString();
            var formToken = "id" + Utils.GenerateRandomString(4);
            System.Diagnostics.Trace.WriteLine(formToken);
            var form = "<form id='" + formToken + "'>" + Environment.NewLine;
            form += token + Environment.NewLine;
            form += "</form>" + Environment.NewLine;
            var script = "var input_" + formToken + " =$('#" + formToken + "').find('input');" + Environment.NewLine;
            script += "var clone_" + formToken + "=input_" + formToken + ".clone();" + Environment.NewLine;
            script += "var f_" + elementId + " =$('#" + elementId + "').find('form');" + Environment.NewLine;
            script += "f_" + elementId + ".append(clone_" + formToken + ");" + Environment.NewLine;
            script += "$('#" + formToken + "').remove();" + Environment.NewLine;
            var setTimeoutScript = ScriptService.SetTimeout(script, 750);
            var scriptSrc = ScriptService.EllipticalReady(setTimeoutScript, true);
            var output = form + scriptSrc;

            return output;
        }

        private string validationSummary(string cssClass, string msg)
        {
            //var helper = this.helper;
            if (msg == null)
            {
                msg = "";
            }
            if (cssClass == null)
            {
                cssClass = "error";
            }
            if (!helper.ViewData.ModelState.IsValid)
            {
                return "<blockquote class='" + cssClass + "' data-id='validation-summary'>" +
                       helper.ValidationSummary(msg) + "</blockquote>";
            }

            return "";
        }
    }

    internal class CustomFormElement
    {
        public string Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Service { get; set; }
        public string Action { get; set; }
        public string Schema { get; set; }
        public string CssClass { get; set; }
        public string Role { get; set; }
        public string Channel { get; set; }
        public string Event { get; set; }
        public string ProcessingMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Redirect { get; set; }
        public string RedirectLabel { get; set; }
        public bool SlideInNotify { get; set; }
        public bool ActionSubmit { get; set; }
    }
}