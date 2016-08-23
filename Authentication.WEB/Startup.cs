﻿using InsuredTraveling.Providers;
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
            var config = new HttpConfiguration();            
            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(150),
                Provider = new SimpleAuthorizationServerProvider()
                //RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

             // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

    }
}