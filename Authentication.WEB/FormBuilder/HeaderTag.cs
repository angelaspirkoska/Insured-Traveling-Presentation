using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class HeaderTag
    {
        public static string Header(string innerText, string cssClass)
        {
            return Header(innerText, cssClass, null);
        }

        public static string Header(string innerText, object htmlAttributes, string cssClass)
        {
            var builder = new TagBuilder("h1");
            builder.SetInnerText(innerText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}