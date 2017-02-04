using InsuredTraveling.Hubs.PipelineModules;
using InsuredTraveling.Providers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup("InsuredTravelingStartup", typeof(InsuredTraveling.Startup))]
namespace InsuredTraveling
{
    public class Startup
    {
        IAppBuilder _app;
        HttpConfiguration _config;
        public void Configuration(IAppBuilder app)
        {
            _app = app;
            _config = new HttpConfiguration();

            ConfigureOAuth();

            ConfigureSignalR();

        }

        private void ConfigureSignalR()
        {
            GlobalHost.HubPipeline.AddModule(new ExceptionPipelineModule());

            var hubConfig = new HubConfiguration
            {
                EnableJavaScriptProxies = true,
                EnableDetailedErrors = true,
                EnableJSONP = true,
            };
            _app.UseCors(CorsOptions.AllowAll);
            _app.UseWebApi(_config);
            _app.MapSignalR(hubConfig);
        }

        public void ConfigureOAuth()
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider()
                //RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            _app.UseOAuthAuthorizationServer(oAuthServerOptions);
            _app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}