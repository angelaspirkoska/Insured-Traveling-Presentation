using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class SelectTag
    {
        public static string Select(string id, string name, bool isRequired, bool isMultiple, string cssClass, List<string> options)
        {
            return Select(id, name, isRequired, isMultiple, cssClass, options, null);
        }
        public static string Select(string id, string name, bool isRequired, bool isMultiple, string cssClass, List<string> options, object htmlAttributes)
        {
            var builder = new TagBuilder("select");
            builder.GenerateId(id);
            builder.MergeAttribute("name", name, true);
            if (isRequired)
                builder.MergeAttribute("required", string.Empty);
            if (isMultiple)
                builder.MergeAttribute("multiple", string.Empty);
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);

            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            foreach (string option in options)
                builder.InnerHtml += option;


            return builder.ToString(TagRenderMode.Normal);
        }
    }
}