using System.Collections.Generic;

namespace Elliptical.Mvc
{
    public partial class HtmlHelpers
    {
        
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="templateSection"></param>
        /// <param name="cssClass"></param>
        /// <param name="placeholder"></param>
        /// <param name="required"></param>
        /// <param name="value"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private string elementInputFor(string tag, string type, string name, string templateSection,
            string cssClass, string placeholder, bool required, string value,bool readOnly,bool enabled, Dictionary<string, string> attributes)
        {
            var attr = new Dictionary<string, string>();
            cssClass = cssClass ?? "";
            type = type.ToLower();

            if (placeholder == null)
            {
                placeholder = name;
            }
            placeholder = placeholder.ToPhraseCase();
            name = name.ToCamelCaseFromProperCase();
            var bindAttribute = "value";
            if (type.ToLower() == "radio" || type.ToLower() == "checkbox")
            {
                bindAttribute = "checked";
            }
            var dbName = name;
            if (templateSection != null)
            {
                dbName = templateSection + "." + dbName;
            }
            if (type != "textarea")
            {
                attr.Add("type", type);
            }
            if (type.ToLower() != "checkbox")
            {
                attr.Add("name", name);
            }
            attr.Add("id", name);
            attr.Add("value", "{" + name + "}");

            if (cssClass != "")
            {
                cssClass = cssClass + " {" + name + "_error}";
            }
            else
            {
                cssClass = "{" + name + "_error}";
            }

            if(!enabled)
            {
                attr.Add("@disabled", "disabled");
            }

            attr.Add("class", cssClass);
            placeholder = "{@placeholder value=\"{" + name + "_placeholder}\" defaultValue=\"" + placeholder + "\" /}";
            attr.Add("data-bind", bindAttribute + ":" + dbName);
            if (type.ToLower() != "radio" && type.ToLower() != "checkbox")
            {
                attr.Add("@placeholder", placeholder);
            }
            if (required)
            {
                attr.Add("required", "true");
            }
            if(readOnly)
            {
                attr.Add("readonly", "true");
            }

            foreach (var pair in attributes)
            {
                attr.Add(pair.Key, pair.Value);
            }

            var inputTag = "";
            if (type != "textarea")
            {
                inputTag = HtmlTagBuilder.BeginHtmlTag(tag, attr, true, true);
                return inputTag;
            }
            inputTag = HtmlTagBuilder.BeginHtmlTag(tag, attr);
            var inputEndTag = HtmlTagBuilder.EndHtmlTag(tag, true);
            return inputTag + inputEndTag;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="cssClass"></param>
        /// <param name="placeholder"></param>
        /// <param name="required"></param>
        /// <param name="value"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private string elementInput(string tag, string type, string name, string id, string cssClass, string placeholder,
            bool required, string value,bool readOnly,bool enabled, Dictionary<string, string> attributes)
        {
            var attr = new Dictionary<string, string>();
            type = type.ToLower();
            value = value ?? "";
            if (placeholder == null)
            {
                placeholder = name;
            }
            placeholder = placeholder.ToPhraseCase();
            if (name != null)
            {
                name = name.ToCamelCaseFromProperCase();
            }
            if (id == null && name != null)
            {
                id = name;
            }

            if (type != "textarea")
            {
                attr.Add("type", type);
            }
            if (type.ToLower() != "checkbox" && name != null)
            {
                attr.Add("name", name);
            }
            if (id != null)
            {
                attr.Add("id", id);
            }

            attr.Add("value", value);

            if (cssClass != null)
            {
                attr.Add("class", cssClass);
            }
          
            if (type.ToLower() != "radio" && type.ToLower() != "checkbox")
            {
                attr.Add("@placeholder", placeholder);
            }
            if (required)
            {
                attr.Add("required", "true");
            }
            if (readOnly)
            {
                attr.Add("readonly", "true");
            }
            foreach (var pair in attributes)
            {
                attr.Add(pair.Key, pair.Value);
            }
            if (!enabled)
            {
                attr.Add("@disabled", "disabled");
            }
            var inputTag = "";
            if (type != "textarea")
            {
                inputTag = HtmlTagBuilder.BeginHtmlTag(tag, attr, true, true);
                return inputTag;
            }
            inputTag = HtmlTagBuilder.BeginHtmlTag(tag, attr);
            var inputEndTag = HtmlTagBuilder.EndHtmlTag(tag, true);
            return inputTag + inputEndTag;
        }
    }
}