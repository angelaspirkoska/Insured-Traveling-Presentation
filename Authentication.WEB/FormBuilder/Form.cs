using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlTags;

namespace InsuredTraveling.FormBuilder
{
    public class Form
    {
        private HtmlTag FormElement { get; set; }
        public Form()
        {
            FormElement = new HtmlTag("form");
        }
        public void WithAttribute(string attrName, string value)
        {
            FormElement.Attr(attrName, value);
        }

        public void AddElement(HtmlTag tag)
        {
            FormElement.Append(tag);
        }

        public override string ToString()
        {
            return FormElement.ToPrettyString();
        }
    }

    public interface ITagGenerator
    {
        HtmlTag GetTag();
        HtmlTag AddValidationAttributes(Dictionary<string, string> attributes);
    }



    public abstract class   BaseTag : ITagGenerator
    {
        protected HtmlTag _tag;
        protected TagInfo _fieldInfo;
        public BaseTag()
        {

        }
        public BaseTag(string tagName, TagInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
            _tag = new HtmlTag(tagName).Id(fieldInfo.Id).Name(fieldInfo.Name);
            string result;
            var tryGetValue = fieldInfo.Attributes.TryGetValue("class", out result);

            tryGetValue = fieldInfo.Attributes.TryGetValue("css", out result);

            if (tryGetValue)
            {
                _tag.Attr("style",result);
            }
           
        }
        public HtmlTag GetTag()
        {
            return _tag;
        }

        public abstract HtmlTag AddValidationAttributes(Dictionary<string, string> attributes);


    }

    public class FormField : BaseTag
    {
        public FormField(TagInfo tagInfo)
        {
            _tag = new HtmlTag("div");
            _tag.Append(new LabelTag(tagInfo).GetTag());
            _tag.Append(new InputTag(tagInfo).GetTag());
        }

        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            //not sure if any validation
            return _tag;
        }
    }

    public class LabelTag : BaseTag
    {
        public LabelTag(TagInfo tagInfo) : base("label", tagInfo)
        {
            string innerHtml;
            var hasName = tagInfo.Attributes.TryGetValue("name", out innerHtml);
            if (hasName)
            {
                _tag.Text(innerHtml.Replace('_',' '));
                
            }
               
        }
        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            //not sure if any validation
            return _tag;
        }
    }

    public class HeaderTag : BaseTag
    {
        public HeaderTag(TagInfo tagInfo) : base("h1", tagInfo)
        {
            string innerHtml;
            var hasName = tagInfo.Attributes.TryGetValue("name", out innerHtml);
            if (hasName)
                _tag.Text(innerHtml.Replace('_', ' '));
        }
        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            //not sure if any validation
            return _tag;
        }
    }

    public class TextareaTag : BaseTag
    {
        public TextareaTag(TagInfo tagInfo) : base("textarea", tagInfo)
        {

        }

        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            string result;
            var tryGetValue = attributes.TryGetValue("required", out result);

            if (tryGetValue && result.Equals("true"))
            {
                _tag.Attr("required");
            }

            tryGetValue = attributes.TryGetValue("field_size", out result);


            if (tryGetValue && !result.Equals(""))
            {
                _tag.Attr("maxlength", result);
            }

            string isRequired;
            tryGetValue = attributes.TryGetValue("ratingIndicatorIndex", out isRequired);

            if (tryGetValue && isRequired.Equals("true"))
            {
                _tag.Attr("ratingIndicator", "true");
            }
            else
            {
                _tag.Attr("ratingIndicator", "false");
            }

            return _tag;
        }
    }

    public class SelectTag : BaseTag
    {
        public SelectTag(TagInfo tagInfo) : base("select", tagInfo)
        {

            HtmlTags.SelectTag tempSelectTag = new HtmlTags.SelectTag();
            tempSelectTag.Id(tagInfo.Id).Name(tagInfo.Name);
            
            foreach(string option in tagInfo.ListItems)
            {           
                tempSelectTag.Option(option, option);
            }

            string defaultValue;
            var tryGetValue = tagInfo.Attributes.TryGetValue("required", out defaultValue);

            if (tryGetValue && defaultValue.Equals("true"))
            {
                tempSelectTag.SelectByValue(defaultValue);
            }

            string isRequired;
            tryGetValue = tagInfo.Attributes.TryGetValue("ratingIndicatorIndex", out isRequired);

            if (tryGetValue && isRequired.Equals("true"))
            {
                tempSelectTag.Attr("ratingIndicator", "true");
            }
            else
            {
                tempSelectTag.Attr("ratingIndicator", "false");
            }

            _tag = tempSelectTag;
        }
        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            //not sure if any validation
            return _tag;
        }
    }


    public class InputTag : BaseTag
    {
        public InputTag(TagInfo tagInfo) : base("input", tagInfo)
        {
        }
        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            string result;
            var tryGetValue = attributes.TryGetValue("required", out result);

            if (tryGetValue && result.Equals("true"))
            {
                _tag.Attr("required");
            }
            string isRequired;
            tryGetValue = attributes.TryGetValue("ratingIndicatorIndex", out isRequired);

            if (tryGetValue && isRequired.Equals("true"))
            {
                _tag.Attr("ratingIndicator", "true");
            }
            else
            {
                _tag.Attr("ratingIndicator", "false");
            }


            tryGetValue = attributes.TryGetValue("field_size", out result);


            if (tryGetValue && !result.Equals(""))
            {
                _tag.Attr("maxlength", result);
            }

            return _tag;
        }
    }

    public class CheckboxTag : InputTag
    {
        public CheckboxTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "checkbox");
        }
        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            base.AddValidationAttributes(attributes);
            string defaultValue;
            var tryGetValue = attributes.TryGetValue("default", out defaultValue);

            if (tryGetValue)
            {
                _tag.Attr("checked");
            }
            return _tag;
        }
    }
    public class DateTag : InputTag
    {
        public DateTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "date");
        }
    }
    public class EmailTag : InputTag
    {
        public EmailTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "email");
        }
    }
    public class FileTag : InputTag
    {
        public FileTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "file");
        }
    }
    public class TextboxTag : InputTag
    {
        public TextboxTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "text");
        }
    }
    public class AlphanumericSpaceTextBox : TextboxTag
    {
        public AlphanumericSpaceTextBox(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("pattern", "[a-zA-Z0-9\\s]+");
        }
    }
    public class AlphanumericTextBox : TextboxTag
    {
        public AlphanumericTextBox(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("pattern", "[a-zA-Z0-9]+");
        }
    }
    public class NumberTag : InputTag
    {
        public NumberTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "number");
        }

        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            string result;
            var tryGetValue = attributes.TryGetValue("max", out result);
            if (tryGetValue && !result.Equals(""))
            {
                _tag.Attr("max", result);
            }
            return _tag;
        }

    }
    public class RadioButtonTag : InputTag
    {
        public RadioButtonTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "radio");
        }
        public override HtmlTag AddValidationAttributes(Dictionary<string, string> attributes)
        {
            base.AddValidationAttributes(attributes);
            string defaultValue;
            var tryGetValue = attributes.TryGetValue("default", out defaultValue);

            if (tryGetValue)
            {
                _tag.Attr("checked");
            }
            return _tag;
        }
    }
    public class TimeTag : InputTag
    {
        public TimeTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "time");
        }
    }
    public class PasswordTag : InputTag
    {
        public PasswordTag(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.Attr("type", "password");
        }
    }

    public class SubmitButton : InputTag
    {
        public SubmitButton(TagInfo tagInfo) : base(tagInfo)
        {
            _tag.RemoveAttr("id");
            _tag.Attr("type", "submit").Value("Calculate");
            _tag.Attr("formaction", "/AdminPanel/PolicyForm?excelId=" + tagInfo.Id);
        }

    }

    public class TagInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<string> ListItems { get; set; }
        public TagInfo()
        {
            Attributes = new Dictionary<string, string>();
            ListItems = new List<string>();
        }
    }

    public static class TagFactory
    {
        public static ITagGenerator GenerateTagFor(TagInfo tagInfo)
        {
            ITagGenerator tag = null;
            switch (tagInfo.Type)
            {     
                case "password": tag = new PasswordTag(tagInfo); break;
                case "submit": tag = new SubmitButton(tagInfo); break;
                case "label": tag = new LabelTag(tagInfo); break;
                case "header": tag = new HeaderTag(tagInfo); break;
                case "textarea": tag = new TextareaTag(tagInfo); break;
                case "dropdown": tag = new SelectTag(tagInfo); break;
                case "checkbox": tag = new CheckboxTag(tagInfo); break;
                case "date": tag = new DateTag(tagInfo); break;
                case "email": tag = new EmailTag(tagInfo); break;
                case "file": tag = new FileTag(tagInfo); break;
                case "textbox": tag = new TextboxTag(tagInfo); break;
                case "radio": tag = new RadioButtonTag(tagInfo); break;
                case "time": tag = new TimeTag(tagInfo); break;
                case "alphanumericspacetextbox": tag = new AlphanumericSpaceTextBox(tagInfo); break;
                case "alphanumerictextbox": tag = new AlphanumericTextBox(tagInfo); break;
                case "number": tag = new NumberTag(tagInfo); break;

                default: return null;
            }

            tag.AddValidationAttributes(tagInfo.Attributes);
            return tag;
        }

        public static HtmlTag GenerateWrappedTagFor(TagInfo tagInfo)
        {
            var a = TagFactory.GenerateTagFor(tagInfo).GetTag();
            var wrapper = new HtmlTag("div");
            string res;
            var tryGetValue = tagInfo.Attributes.TryGetValue("class", out res);

            if (tryGetValue)
            {
                wrapper.AddClasses(res);
            }

            wrapper.Append(a);

            return wrapper;
            
        }

       }
}