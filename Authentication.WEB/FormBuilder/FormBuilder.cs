using HtmlTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.FormBuilder
{
    public class FormBuilder
    {
        private Form Form { get; set; }
        public FormBuilder()
        {
            Form = new Form();
        }

        public FormBuilder SetName(string name)
        {
            Form.WithAttribute("name", name);
            return this;
        }

        public FormBuilder SetMethod(string methodType)
        {
            Form.WithAttribute("method", methodType);
            return this;
        }

        public FormBuilder SetAction(string action)
        {
            Form.WithAttribute("action", action);
            return this;
        }

        public FormBuilder AddElement(HtmlTag tag)
        {
            Form.AddElement(tag);
            return this;
        }

        public string ToHtmlString()
        {
            return Form.ToString();
        }
    }
}
