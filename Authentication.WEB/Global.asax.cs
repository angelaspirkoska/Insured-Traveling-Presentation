﻿using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Authentication.WEB;
using InsuredTraveling.App_Start;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Threading;
using System.Globalization;
using System.Web.Security;
using System.Web.Http.Dispatcher;
using System.Configuration;
using InsuredTraveling.Schedulers;

namespace InsuredTraveling
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());
            SetupDependencyInjection();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomViewEngine());
            AutoMapperInitializer.Initialize();
            ModelBinders.Binders[typeof(DateTime)] = new DateTimeModelBinder(ConfigurationManager.AppSettings["DateFormat"]);
            ModelBinders.Binders[typeof(DateTime?)] = new DateTimeModelBinder(ConfigurationManager.AppSettings["DateFormat"]);
            JobScheduler.Start();
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
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User == null) return;
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
            if (!(HttpContext.Current.User.Identity is FormsIdentity)) return;
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
