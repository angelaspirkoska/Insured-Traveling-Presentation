using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class OptionTag
    {
        public static string Option(string id, string value, bool isSelected, string innerText, string cssClass)
        {
            return Option(id, value, isSelected, innerText, cssClass, null);
        }

        public static string Option(string id, string value, bool isSelected, string innerText, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("option");
            builder.GenerateId(id);          
            builder.MergeAttribute("value", value, true);
            if (isSelected)
                builder.MergeAttribute("selected", string.Empty);
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.SetInnerText(innerText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.Normal);
        }

    }
}