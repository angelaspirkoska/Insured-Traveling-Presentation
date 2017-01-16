using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InsuredTraveling.FormBuilder
{
    public static class FormBuilder
    {
        public static IHtmlString CreateForm()
        {
            var builder = HeaderTag.Header("bla", null);
            builder += LabelTag.Label("2", "Labela za tekst area", "nekojaRandomKlasa");
            builder += TextAreaTag.TextArea("2","bla", null);
            builder += TextBoxTag.TextBox("2", "bla", null);
            builder += EmailTag.Email("2", "bla", null);
            builder += CheckBoxTag.CheckBox("2", "bla", false, null);
            builder += RadioButtonTag.RadioButton("2", "sex", "male", true, "imeNaKlasa");
            builder += RadioButtonTag.RadioButton("3", "sex", "female", false, null);
            builder += FileTag.File("4", "documents", true, null);
            builder += DateTag.Date("3", "datenekoj", null);
            builder += TimeTag.Time("3", "timnekoj", null);
            List<string> lista = new List<string>();
            var option = OptionTag.Option("3", "valuee", true, "text", null);
            lista.Add(option);
            var option2 = OptionTag.Option("4", "value43e", true, "text4", null);
            lista.Add(option2);
            builder += SelectTag.Select("34", "lista", true, false, null, lista);
            var result = new HtmlString(builder);
            return result;

        }
    }
}