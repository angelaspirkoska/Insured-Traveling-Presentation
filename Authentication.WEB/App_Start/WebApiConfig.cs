using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using InsuredTraveling;
using Authentication.WEB.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System;
using System.Net;
using System.Text;
using InsuredTraveling.App_Start;
using InsuredTraveling.Controllers.API;

namespace Authentication.WEB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(MobileApiController).Assembly);
            //builder.RegisterApiControllers(typeof(HalkbankPaymentApiController).Assembly);
            builder.RegisterApiControllers(typeof(NewsApiController).Assembly);
            builder.RegisterApiControllers(typeof(ChatController).Assembly);
            
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterModule(
                new ServicesRegistration("InsuredTravelingEntity")); 

            var container = builder.Build();
            config.Filters.Add(new RequireHttpsAttribute());
            config.DependencyResolver = new AutofacWebApiDependencyResolver(
                container);
        }
    }
}
