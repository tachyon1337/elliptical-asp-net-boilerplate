using System;
using System.Collections.Generic;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IDisposable Template()
        {
            var writer = helper.ViewContext.Writer;
            var id = "fragment_" + Utils.GenerateRandomString(4);
            var dict = new Dictionary<string, string> {{"id", id}};
            var output = HtmlTagBuilder.BeginHtmlTag("ui-template", dict, true);
            writer.Write(output);
            return new HtmlTagContainer(writer, "ui-template");
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IDisposable Template(string id)
        {
            var writer = helper.ViewContext.Writer;
            var dict = new Dictionary<string, string> {{"id", id}};
            var output = HtmlTagBuilder.BeginHtmlTag("ui-template", dict, true);
            writer.Write(output);
            return new HtmlTagContainer(writer, "ui-template");
        }

        /// <summary>
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public IDisposable TemplateSection(string section)
        {
            var helper = this.helper;
            var writer = helper.ViewContext.Writer;
            var output = "{" + section + "}" + Environment.NewLine;
            writer.Write(output);
            return new HtmlTagContainer(writer, "section");
        }

        /// <summary>
        ///     /
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="importUrl"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IDisposable WebComponent(string tagName, string importUrl, ElementAttributes attributes)
        {
            var link = html5ImportString(importUrl, null);
            var tag = componentElement(tagName, attributes);
            var writer = helper.ViewContext.Writer;
            var output = link + Environment.NewLine + tag;
            writer.Write(output);
            return new HtmlTagContainer(writer, tagName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="tagName"></param>
        /// <param name="importUrl"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IDisposable WebComponent<T>(T model, string tagName, string importUrl, ElementAttributes attributes)
        {
            var link = elementImport(model, attributes.Scope, false, true, importUrl);
            var tag = componentElement(tagName, attributes);
            var writer = helper.ViewContext.Writer;
            var output = link + Environment.NewLine + tag;
            writer.Write(output);
            return new HtmlTagContainer(writer, tagName);
        }

        /// <summary>
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private string componentElement(string tagName, ElementAttributes attributes)
        {
            string link = "";
            var dict = attributes.Attributes;
            if (attributes.CssClass != null)
            {
                dict.Add("class", attributes.CssClass);
            }
            if (attributes.Id != null)
            {
                dict.Add("id", attributes.Id);
            }
            if (attributes.Name != null)
            {
                dict.Add("name", attributes.Name);
            }
            if (attributes.Scope != null)
            {
                dict.Add("scope", attributes.Scope);
            }
            if (attributes.Service != null)
            {
                dict.Add("service", attributes.Service);
            }
            if(attributes.ComponentCss !=null)
            {
                dict.Add("component-css", attributes.ComponentCss);
            }
            if (attributes.DataUpgraded ==true)
            {
                dict.Add("data-upgraded", "true");
            }
            if (attributes.Enabled == false)
            {
                dict.Add("@disabled", "disabled");
            }
            if (attributes.Html5Imported == true)
            {
                dict.Add("html5-imported", "true");
            }
            if (attributes.Label !=null)
            {
                dict.Add("label", attributes.Label);
            }
            if (attributes.Preload == true)
            {
                dict.Add("preload", "true");
            }
            if (attributes.RequireUpgrade == true)
            {
                dict.Add("require-upgrade", "true");
            }
            if (attributes.Message !=null)
            {
                dict.Add("message", attributes.Message);
            }
            if (attributes.Value !=null)
            {
                dict.Add("value", attributes.Value);
            }
            if (attributes.Placeholder !=null)
            {
                dict.Add("placeholder", attributes.Placeholder);
            }
            if (attributes.Required == true)
            {
                dict.Add("required", "true");
            }
            if (attributes.ReadOnly == true)
            {
                dict.Add("readonly", "true");
            }
            if (attributes.Channel != null)
            {
                dict.Add("channel", attributes.Channel);
            }
            if (attributes.Event != null)
            {
                dict.Add("event", attributes.Event);
            }
            if (!attributes.EnableClientBinding)
            {
                dict.Add("data-bind", "false");
            }
            if (attributes.Controller != null)
            {
                dict.Add("controller", attributes.Controller);
            }
            if (attributes.ImportHref != null)
            {
                link = html5ImportString(attributes.ImportHref, null) + Environment.NewLine;
            }
            var tag = HtmlTagBuilder.BeginHtmlTag(tagName, dict, true);

            return link + tag;
        }
    }
}