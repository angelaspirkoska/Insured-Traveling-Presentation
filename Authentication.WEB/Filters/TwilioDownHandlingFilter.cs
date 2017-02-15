using Authentication.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{
    public class TwilioDownHandlingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebRequest request = WebRequest.Create("https://www.twilio.com/");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
            {
                try
                {
                    string body = "The www.twilio.com site is down and insuredtraveling is facing problem with normaly running the site";
                    MailService mailService = new MailService("info@optimalreinsurance.com");
                    mailService.setSubject("InsuredTraveling - ErrorHandler");
                    mailService.setBodyText(body, true);
                    mailService.sendMail();
                }
                catch (Exception e)
                {
                    var url2 = new UrlHelper(filterContext.RequestContext);
                    var errorUrl2 = url2.Content("~/Error/Mail");
                    filterContext.HttpContext.Response.Redirect(errorUrl2, true);
                }
                var url = new UrlHelper(filterContext.RequestContext);
                var errorUrl = url.Content("~/Error/Index");
                filterContext.HttpContext.Response.Redirect(errorUrl, true);
            }
        }

    }
}
