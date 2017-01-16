using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class LabelTag
    {
        public static string Label(string forField, string innerText, string cssClass)
        {
            return Label(forField, innerText, cssClass, null);
        }

        public static string Label(string forField, string innerText, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("label");        
            builder.MergeAttribute("for", forField, true);
            builder.SetInnerText(innerText);
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.Normal);
        }

    }
}