using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface INotificationService
    {
        notification AddNotification(string title, string content);
        notificationuser AddUserNotification(int notificationId, string userId);
        notification GetNotificationById(int notificationId);
    }
}