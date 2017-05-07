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
        private INotificationService _notificationService;

        public NotificationsHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void AddNotification(int notificationId)
        {
            var notification = _notificationService.GetNotificationById(notificationId);

            var result = new StringBuilder();
            var h3 = new TagBuilder("h3");
            h3.InnerHtml = notification.Title;

            var p = new TagBuilder("p");
            p.InnerHtml = notification.Content;

            var small = new TagBuilder("small");
            small.InnerHtml = notification.CreatedDate.ToString("dd.MMM.yyyy | HH:mm");

            Clients.All.showNotification(result.ToString());
        }
    }
}