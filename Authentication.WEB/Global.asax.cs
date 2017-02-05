using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Authentication.WEB;
using InsuredTraveling.App_Start;
using Autofac;
using Autofac.Integration.Mvc;
using InsuredTraveling.Filters;
using System;
using System.Threading;
using System.Globalization;

namespace InsuredTraveling
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SetupDependencyInjection();

            //AreaRegistration.RegisterAllAreas();
            ////FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new CustomViewEngine());
            //AutoMapperInitializer.Initialize();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            }
            else
            {
               lang = SiteLanguages.GetDefaultLanguage();
               new SiteLanguages().SetLanguage(lang);
            }          
        }

        private void SetupDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule(new ServicesRegistration("InsuredTravelingEntity"));
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
