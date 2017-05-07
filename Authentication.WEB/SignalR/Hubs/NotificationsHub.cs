using InsuredTraveling.DI;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.SignalR.Hubs
{
    public class NotificationsHub : Hub
    {

        public void AddNotification(int notificationId)
        {
            var notificationService = new NotificationService();
            var notification = notificationService.GetNotificationById(notificationId);

            var result = new StringBuilder();

            var h3 = new TagBuilder("h3");
            h3.InnerHtml = notification.Title;
            result.Append(h3);

            var p = new TagBuilder("p");
            p.InnerHtml = notification.Content;
            result.Append(p);

            var small = new TagBuilder("small");
            small.InnerHtml = notification.CreatedDate.ToString("dd.MMM.yyyy | HH:mm");
            result.Append(small);

            Clients.All.showNotification(result.ToString());
        }
    }
}