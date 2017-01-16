using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class CheckBoxTag
    {
        public static string CheckBox(string id, string name, bool isChecked, string cssClass)
        {
            return CheckBox(id, name, isChecked, cssClass, null);
        }
        public static string CheckBox( string id, string name, bool isChecked, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("input");

            builder.GenerateId(id);
            builder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.CheckBox));
            builder.MergeAttribute("name", name, true);
            if (isChecked)
            {
                builder.MergeAttribute("checked", string.Empty);
            }
            if (cssClass != null && cssClass != string.Empty)
                builder.AddCssClass(cssClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return builder.ToString(TagRenderMode.SelfClosing);

        }
    }
}