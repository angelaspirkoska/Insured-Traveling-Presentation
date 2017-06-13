using System;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;

namespace InsuredTraveling.App_Start
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent("<p>Use HTTPS instead of HTTP</p>");

                UriBuilder uriBuilder = new UriBuilder(actionContext.Request.RequestUri);
                uriBuilder.Scheme = Uri.UriSchemeHttps;

                if (actionContext.Request.IsLocal())
                {
                    uriBuilder.Port = 44375;
                }
                else
                {
                    uriBuilder.Port = -1;
                }
                actionContext.Response.Headers.Location = new Uri(uriBuilder.Uri.AbsoluteUri);
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
            base.OnAuthorization(actionContext);
        }
    }
}