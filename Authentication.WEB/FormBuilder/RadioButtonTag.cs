using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class RadioButtonTag
    {
        public static string RadioButton(string id, string name, string value, bool isChecked, string cssClass)
        {
            return RadioButton(id, name, value,isChecked, cssClass, null);
        }
        public static string RadioButton( string id, string name, string value, bool isChecked, string cssClass, object htmlAttributes)
        {
            var builder = new TagBuilder("input");

            builder.GenerateId(id);
            builder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Radio));
            builder.MergeAttribute("name", name, true);
            builder.MergeAttribute("value", value);
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