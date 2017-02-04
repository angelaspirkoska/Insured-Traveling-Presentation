﻿using InsuredTraveling.Hubs.PipelineModules;
using InsuredTraveling.Providers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;


[assembly: OwinStartup("InsuredTravelingStartup", typeof(InsuredTraveling.Startup))]
namespace InsuredTraveling
{
    public class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.HubPipeline.AddModule(new ExceptionPipelineModule());
            var config = new HttpConfiguration();            
            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.MapSignalR();
        }

        public void ConfigureOAuth(IAppBuilder app)
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
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

    }
}