using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using AutoMapper;
using InsuredTraveling.DI;
using InsuredTraveling.Models;


namespace InsuredTraveling.Filters
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
	{
		// TODO: Add your action filter's tasks here

		// Log Action Filter call
		using (InsuredTravelingEntity baza = new InsuredTravelingEntity())
		{
			ActionLog1 log = new ActionLog1()
			{
				controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
				action = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged By: Custom Action Filter)"),
				ip_address = filterContext.HttpContext.Request.UserHostAddress,
				datetime = filterContext.HttpContext.Timestamp
               
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