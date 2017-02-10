using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{

 
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext filterContext)
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
                    string refresh_tokenNew = await _repo.RefreshToken(refresh_tokenCookie.Value);
                    refresh_tokenCookie.Value = refresh_tokenNew;
                }

            }
            
        }
    }
}