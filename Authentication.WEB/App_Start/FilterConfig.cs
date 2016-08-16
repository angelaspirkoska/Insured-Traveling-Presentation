using System.Web.Mvc;
using InsuredTraveling.Filters;

namespace InsuredTraveling
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RecaptchaFilter());
        }
    }
}
