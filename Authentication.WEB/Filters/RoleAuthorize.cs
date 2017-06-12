using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Filters
{
    public static class RoleAuthorize
    {
        public static bool IsUser(string role)
        {
            InsuredTravelingEntity2 context = new InsuredTravelingEntity2();

            string username = GetCurrentLoggedUser();
            bool authorize = false;
            aspnetuser aspnetuser = context.aspnetusers.Where(m => m.UserName == username).FirstOrDefault();
            if (aspnetuser == null)
                return authorize;     
            if (aspnetuser.aspnetroles.Count == 0)
            {
                authorize = false;
                return authorize;
            }
            authorize = aspnetuser.aspnetroles.FirstOrDefault() != null ? aspnetuser.aspnetroles.FirstOrDefault().Name == role : false;
            
            return authorize;
        }
        public static bool IsUser(string role, string username)
        {
            InsuredTravelingEntity2 context = new InsuredTravelingEntity2();

            bool authorize = false;
            aspnetuser aspnetuser = context.aspnetusers.FirstOrDefault(m => m.UserName == username);
            if (aspnetuser == null)
                return authorize;
            if (aspnetuser.aspnetroles.Count == 0)
                return authorize;
            authorize = aspnetuser.aspnetroles.FirstOrDefault() != null ? aspnetuser.aspnetroles.FirstOrDefault().Name == role : false;

            return authorize;
        }
        public static string UserSsn(string username)
        {
            InsuredTravelingEntity2 context = new InsuredTravelingEntity2();

            aspnetuser aspnetuser = context.aspnetusers.FirstOrDefault(m => m.UserName == username);
            if (aspnetuser == null)
                return null;
            else
                return aspnetuser.EMBG.ToString();
        }

        public static string GetCurrentLoggedUser()
        {
            string username = "";
     
            var usernameCookie = HttpContext.Current.Request.Cookies["username"];
            if (usernameCookie != null)
            {
                if (usernameCookie.Value != null)
                {
                    username = usernameCookie.Value;
                   
                }
            }
            return username;
        }

        public static bool IsUserLoggedIn()
        {
            var token = HttpContext.Current.Request.Cookies["token"];
            var expires = HttpContext.Current.Request.Cookies["expires"];
            if(token != null && expires != null)
            {
                if(token.Value != null && expires.Value != null)
                {
                    if(DateTime.UtcNow < Convert.ToDateTime(expires.Value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}