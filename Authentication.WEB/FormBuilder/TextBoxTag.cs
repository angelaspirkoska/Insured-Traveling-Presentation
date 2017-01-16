using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class TextBoxTag
    {
        public static string TextBox(string id, string name, string cssClass)
        {
            return TextBox(id, name, cssClass, null);
        }
        public static string TextBox(string id, string name, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("input");
            builder.GenerateId(id);
            builder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
            builder.MergeAttribute("name", name, true);
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}