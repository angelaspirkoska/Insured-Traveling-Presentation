using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{
    public class SmarteraspDownHandlingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebRequest request = WebRequest.Create("http://smarterasp.net/");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
            {
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/Login/Index");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }
}