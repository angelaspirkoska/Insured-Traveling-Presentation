using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.SignalR.Hubs
{
    public class NotificationsHub : Hub
    {
        public void AddNotification(int ticketId)
        {

            Clients.All.showNotification("test");
        }

    }
}