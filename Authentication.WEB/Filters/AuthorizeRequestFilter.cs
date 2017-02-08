using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace InsuredTraveling.Filters
{
    public class AuthorizeRequestFilter : ActionFilterAttribute
    {
      
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var cookieToken = HttpContext.Current.Request.Cookies["token"];
            if (cookieToken == null) return;
            var t = cookieToken.Value;
            var s = new string[1];
            s[0] = "Bearer " + t;
            var context = HttpContext.Current.GetOwinContext();
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers.Add("Authorization", s);
            }
        }
    }
}