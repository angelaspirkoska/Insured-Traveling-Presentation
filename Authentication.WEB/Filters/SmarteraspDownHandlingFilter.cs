﻿using Authentication.WEB.Services;
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
                try
                {
                    string body = "The smarterasp.net site is down and insuredtraveling is facing problem with normaly running the site";
                    MailService mailService = new MailService("info@optimalreinsurance.com"); 
                    mailService.setSubject("InsuredTraveling - ErrorHandler");
                    mailService.setBodyText(body, true);
                    mailService.sendMail();
                }
                catch (Exception e)
                {

                }
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/Error/Index");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }
}