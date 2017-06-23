using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using InsuredTraveling.DI;
using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;


namespace InsuredTraveling.Filters
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        { 
		using (InsuredTravelingEntity baza = new InsuredTravelingEntity())
		{
			ActionLog1 log = new ActionLog1()
			{
                username = System.Web.HttpContext.Current.User.Identity.Name,
                controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                action = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged By: Custom Action Filter)"),
                ip_address = filterContext.HttpContext.Request.UserHostAddress,
                datetime = filterContext.HttpContext.Timestamp.ToUniversalTime(),
                date = System.Web.HttpContext.Current.Timestamp.Date.ToShortDateString(),
                time = System.Web.HttpContext.Current.Timestamp.TimeOfDay.ToString("g")
            };
                //context.log_activities.
                //context.SaveChanges();
                var log1 = Mapper.Map<ActionLog1, log_activities>(log);
                LogActivityService nasd = new LogActivityService();
                nasd.AddLog(log1);
                OnActionExecuting(filterContext);
		}
	}

    }
}