using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Authentication.WEB;
using InsuredTraveling.App_Start;

namespace InsuredTraveling
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {          
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomViewEngine());
            AutoMapperInitializer.Initialize();
        }
    }
}
