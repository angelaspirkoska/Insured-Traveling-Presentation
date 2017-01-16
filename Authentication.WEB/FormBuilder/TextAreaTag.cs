using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class TextAreaTag
    {
        public static string TextArea(string id, string name, string cssClass)
        {
            return TextArea(id, name, cssClass, null);
        }

        public static string TextArea(string id, string name, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("textarea");
            builder.GenerateId(id);          
            builder.MergeAttribute("name", name, true);
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.Normal);
        }

    }
}