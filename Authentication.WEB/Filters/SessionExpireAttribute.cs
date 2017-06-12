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
            if (!RoleAuthorize.IsUserLoggedIn())
            {
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/Login/Index");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }
}