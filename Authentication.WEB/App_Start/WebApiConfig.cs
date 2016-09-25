using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using InsuredTraveling;
using Authentication.WEB.Controllers;

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

            builder.RegisterApiControllers(typeof(InsuredTraveling.Controllers.API.MobileApiController).Assembly);
            builder.RegisterApiControllers(typeof(HalkbankPaymentApiController).Assembly);
            builder.RegisterApiControllers(typeof(NewsApiController).Assembly);
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterModule(
                new ServicesRegistration("InsuredTravelingEntity")); 

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(
                container);
        }
    }
}
