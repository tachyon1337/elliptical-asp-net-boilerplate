using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;


namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        public IDisposable UIHistory(string viewSelector, string appId, string appImportHref)
        {
            const string tagName="ui-history";
            const string importUrl = "ui-history";
            var attributes = new ElementAttributes();
            var link = html5ImportString(importUrl, null);
            var appLink = html5ImportString(appImportHref, null);
            var tag = historyElement(viewSelector,appId, attributes);
            var writer = helper.ViewContext.Writer;
            var output = link + Environment.NewLine + appLink + Environment.NewLine + tag;
            writer.Write(output);
            return new HtmlTagContainer(writer, tagName);

        }

        public IDisposable UIHistory(string viewSelector, string appId, string appImportHref, ElementAttributes attributes)
        {
            const string tagName = "ui-history";
            const string importUrl = "ui-history";
            var link = html5ImportString(importUrl, null);
            var appLink = html5ImportString(appImportHref, null);
            var tag = historyElement(viewSelector, appId, attributes);
            var writer = helper.ViewContext.Writer;
            var output = link + Environment.NewLine + appLink + Environment.NewLine + tag;
            writer.Write(output);
            return new HtmlTagContainer(writer, tagName);

        }

        private string historyElement(string viewSelector,string appId, ElementAttributes attributes)
        {
            const string tagName="ui-history";
            var dict = attributes.Attributes;
            dict.Add("view-selector", viewSelector);
            dict.Add("app", appId);
            return componentElement(tagName, attributes);
        }
    }

}