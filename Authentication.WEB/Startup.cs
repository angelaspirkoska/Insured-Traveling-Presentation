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
using InsuredTraveling.App_Start;

[assembly: OwinStartup("InsuredTravelingStartup", typeof(InsuredTraveling.Startup))]
namespace InsuredTraveling
{
    public class Startup
    {
        #region Parameters
        IAppBuilder _app;
        HttpConfiguration _config;
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }
        #endregion

        public void Configuration(IAppBuilder app)
        {
            _app = app;
            _config = new HttpConfiguration();

            ConfigureOAuth(app);
            ConfigureSignalR();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(AuthContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
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
        }
    }
}