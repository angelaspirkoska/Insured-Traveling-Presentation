using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{
    public class RoleAuthorize : AuthorizeAttribute
    {
        InsuredTravelingEntity context = new InsuredTravelingEntity();
        private readonly string[] allowedroles;

        public RoleAuthorize(params string[] roles)
        {
            allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var role in allowedroles)
            {
                var user = httpContext.User;
                var selectedUser = context.aspnetusers.Single(m => m.UserName == user.Identity.Name);
                if(selectedUser.aspnetroles.Count == 0)
                {
                    authorize = false;
                    return authorize;
                }
                authorize = selectedUser.aspnetroles.FirstOrDefault().Name == role;          
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        public bool IsUser(string role)
        {
            bool authorize = false;
            var user = System.Web.HttpContext.Current.User;
            var selectedUser = context.aspnetusers.Single(m => m.UserName == user.Identity.Name);
            if (selectedUser.aspnetroles.Count == 0)
            {
                authorize = false;
                return authorize;
            }
            authorize = selectedUser.aspnetroles.FirstOrDefault().Name == role;
            
            return authorize;
        }
    }
}