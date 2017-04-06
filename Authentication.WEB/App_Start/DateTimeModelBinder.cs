using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;


namespace InsuredTraveling.App_Start
{
    public class DateTimeModelBinder : IModelBinder
    {
        private readonly string _customFormat;

        public DateTimeModelBinder(string customFormat)
        {
            this._customFormat = customFormat;
        }
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            DateTime dateTime;
            string dateTimeFormat = null;

            if (_customFormat.ToLower().Contains("yy"))
                dateTimeFormat = _customFormat.Replace("yy", "yyyy");

            if(String.IsNullOrEmpty(value.AttemptedValue))
            {
                return null;
            }
            var isDate = DateTime.TryParseExact(value.AttemptedValue, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

            if (!isDate)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, new Exception());
                return DateTime.UtcNow;
            }

            return dateTime;
        }
    }
}
