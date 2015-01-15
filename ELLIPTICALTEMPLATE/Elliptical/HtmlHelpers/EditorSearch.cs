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
        /// <param name="action"></param>
        /// <param name="appendPageNoToAction"></param>
        /// <returns></returns>
        public IHtmlString Search(string action, bool appendPageNoToAction)
        {
            return new HtmlString(elementSearch(action, appendPageNoToAction));
        }

        private string elementSearch(string action, bool appendPageNoToAction)
        {
            var uiAttr = new Dictionary<string, string>
            {
                {"action", action},
                {"append-page", appendPageNoToAction.ToString()}
            };
            var import = html5ImportString("ui-search", null) + Environment.NewLine;
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-search", uiAttr, true);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-search", true);

            return import + uiTag + uiEndTag;
        }
    }
}