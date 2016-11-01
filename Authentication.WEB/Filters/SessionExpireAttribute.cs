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
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated && session.IsNewSession )
            {
                //Redirect
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/Login/Index");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }

        }
    }
}