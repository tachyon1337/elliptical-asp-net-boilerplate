using System.Collections.Generic;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        
        
        
        private string elementCheckboxFor(string tag, string type, string name, string templateSection,
            string cssClass, string placeholder, bool required, string value,bool readOnly,bool enabled, Dictionary<string, string> attributes)
        {
            var nameAttr = name.ToCamelCaseFromProperCase();
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-checkbox", attributes, true);
            var lblAttr = new Dictionary<string, string> {{"for", nameAttr}};
            var lblTag = HtmlTagBuilder.BeginHtmlTag("label", lblAttr);
            lblTag += name.ToPhraseCase();
            lblTag += HtmlTagBuilder.EndHtmlTag("label", true);
            var input = elementInputFor(tag, type, name, templateSection, cssClass, placeholder, required, value,readOnly,enabled,
                new Dictionary<string, string>());
            var hidden = elementInputHiddenFor(name, templateSection);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-checkbox", true);

            return uiTag + input + lblTag + hidden + uiEndTag;
        }

        private string elementCheckbox(string tag, string type, string name, string id,
            string cssClass, string placeholder, bool required, string value,bool readOnly,bool enabled, Dictionary<string, string> attributes)
        {
            var nameAttr = name.ToCamelCaseFromProperCase();
            var uiTag = HtmlTagBuilder.BeginHtmlTag("ui-checkbox", attributes, true);
            var lblAttr = new Dictionary<string, string> {{"for", nameAttr}};
            var lblTag = HtmlTagBuilder.BeginHtmlTag("label", lblAttr);
            lblTag += name.ToPhraseCase();
            lblTag += HtmlTagBuilder.EndHtmlTag("label", true);
            var input = elementInput(tag, type, name, id, cssClass, placeholder, required, value,readOnly,enabled,
                new Dictionary<string, string>());
            var hidden = elementInputHidden(name, id, value);
            var uiEndTag = HtmlTagBuilder.EndHtmlTag("ui-checkbox", true);

            return uiTag + input + lblTag + hidden + uiEndTag;
        }
    }
}