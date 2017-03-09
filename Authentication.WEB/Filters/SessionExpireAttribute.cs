using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{

 
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            HttpSessionStateBase session = filterContext.HttpContext.Session;


            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var refresh_tokenCookie = HttpContext.Current.Request.Cookies["refresh_token"];
                var tokenCookie = HttpContext.Current.Request.Cookies["token"];

                if(refresh_tokenCookie == null || tokenCookie == null)
                {
                    var url = new UrlHelper(filterContext.RequestContext);
                    var loginUrl = url.Content("~/Login/Index");
                    filterContext.HttpContext.Response.Redirect(loginUrl, true);
                }else
                {
                    AuthRepository _repo = new AuthRepository();
                     _repo.RefreshToken(refresh_tokenCookie.Value);

                    var cookieToken = HttpContext.Current.Request.Cookies["token"];
                    HttpContext.Current.Request.GetOwinContext().Request.Headers.Remove("Authorization");             
                    var t = EncryptionHelper.Decrypt(HttpUtility.UrlDecode(cookieToken.Value));
                    var s = new string[1];
                    s[0] = "Bearer " + t;
                    HttpContext.Current.Request.GetOwinContext().Request.Headers.Add("Authorization", s);

                    if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var url = new UrlHelper(filterContext.RequestContext);
                        var loginUrl = url.Content("~/Login/Index");
                        filterContext.HttpContext.Response.Redirect(loginUrl, true);
                    }
                    
                }

            }
            
        }
    }
}