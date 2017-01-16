using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class FileTag
    {
        public static string File(string id, string name, bool isMultiple, string cssClass)
        {
            return File(id, name, isMultiple, cssClass, null);
        }
        public static string File(string id, string name, bool isMultiple, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("input");
            builder.GenerateId(id);
            builder.MergeAttribute("type", "file");
            builder.MergeAttribute("name", name, true);
            
            if (isMultiple)
            {
                builder.MergeAttribute("multiple", string.Empty);
            }
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}