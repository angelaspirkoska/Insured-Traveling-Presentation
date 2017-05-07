using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class NotificationService : INotificationService
    {
        private readonly InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public notification AddNotification(string title, string content)
        {
            var notification = new notification
            {
                CreatedDate = DateTime.Now,
                Title = title,
                Content = content,
                URL = "/Kanban/Index"
            };
            _db.notifications.Add(notification);
            _db.SaveChanges();

            return notification;
        }

        public notificationuser AddUserNotification(int notificationId, string userId)
        {
            var notificationuser = new notificationuser
            {
                UserId = userId,
                NotificationId = notificationId,
                IsOpened = false
            };
            _db.notificationusers.Add(notificationuser);
            _db.SaveChanges();

            return notificationuser;
        }

        public notification GetNotificationById(int notificationId)
        {
            return _db.notifications.FirstOrDefault(x => x.Id == notificationId);
        }
    }
}