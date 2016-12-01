using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using InsuredTraveling.Filters;

namespace InsuredTraveling.Hubs
{
    public class ChatHub : Hub
    {

        public override Task OnConnected()
        {
            RoleAuthorize r = new RoleAuthorize();
           var username = System.Web.HttpContext.Current.User.Identity.Name;
            if (r.IsUser("admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);
            }
            else if(r.IsUser("end user"))
            {
                //Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);
            }
            return base.OnConnected();
        }
        public void SendToAdmins(string message)
        {

        }
    }
}