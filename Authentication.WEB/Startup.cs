using InsuredTraveling.SignalR.PipelineModules;
using InsuredTraveling.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using System.Reflection;
using Microsoft.AspNet.SignalR.Hubs;

[assembly: OwinStartup("InsuredTravelingStartup", typeof(InsuredTraveling.Startup))]
namespace InsuredTraveling
{

    public class Startup
    {
        public static string PublicClientId { get; private set; }
        IAppBuilder _app;
        HttpConfiguration _config;

        public void Configuration(IAppBuilder app)
        {
            _app = app;
            _config = new HttpConfiguration();

            ConfigureOAuth();

            ConfigureSignalR();
        }

        public void ConfigureOAuth()
        {
            _app.UseCookieAuthentication(new CookieAuthenticationOptions());
            _app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            PublicClientId = "self";
            var OAuthOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            //_app.UseOAuthBearerTokens(OAuthOptions);

            _app.UseOAuthAuthorizationServer(OAuthOptions);
            _app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureSignalR()
        {
            var builder = new ContainerBuilder();

            var hubConfig = new HubConfiguration
            {
                EnableJavaScriptProxies = true,
                EnableDetailedErrors = true
            };

            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            _app.UseAutofacMiddleware(container);
            hubConfig.Resolver = new AutofacDependencyResolver(container);

            _app.UseCors(CorsOptions.AllowAll);
            _app.UseWebApi(_config);
            _app.MapSignalR(hubConfig);

            var hubPipeline = hubConfig.Resolver.Resolve<IHubPipeline>();
            hubPipeline.AddModule(new ExceptionPipelineModule());
            hubPipeline.AddModule(new LoggingPipelineModule());

            //GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);
            //GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
            //GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);
        }
    }
}