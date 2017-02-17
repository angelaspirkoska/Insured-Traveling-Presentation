using Authentication.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{
    public class ZohoDownHandlingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebRequest request = WebRequest.Create("https://mail.zoho.com");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
            {
                var url = new UrlHelper(filterContext.RequestContext);
                var errorUrl = url.Content("~/Error/Mail");
                filterContext.HttpContext.Response.Redirect(errorUrl, true);
            }
        }
    }
}