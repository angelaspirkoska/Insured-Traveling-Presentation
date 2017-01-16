using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class DateTag
    {
        public static string Date(string id, string name, string cssClass)
        {
            return Date(id, name, cssClass, null);
        }
        public static string Date(string id, string name, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("input");
            builder.GenerateId(id);
            builder.MergeAttribute("type", "date");
            builder.MergeAttribute("name", name, true);
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}