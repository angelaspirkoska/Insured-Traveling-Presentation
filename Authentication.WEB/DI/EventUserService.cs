using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class EventUserService : IEventUserService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        public int CountPeopleAttending(int eventName)
        {
           
            int counter = 0;
            List<event_users> ev = _db.event_users.ToList();
            foreach (event_users event_us in ev)
            {
                if (event_us.EventID == eventName)
                {
                    counter++;
                }
              
            }
            return counter;
        }
        public List<SearchRegisteredUser> PeoplePerEventAttending(int eventName)
        {

            List<SearchRegisteredUser> listOfUsersEvent = new List<SearchRegisteredUser>();

            List<event_users> ev = _db.event_users.ToList();

            List<aspnetuser> Alluser = _db.aspnetusers.ToList();
            SearchRegisteredUser oneUser = new SearchRegisteredUser();

            foreach (event_users event_us in ev)
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;

                if (event_us.EventID == eventName)
                {
                    foreach (aspnetuser user in Alluser)
                    {
                        if ( event_us.UserID == user.Id ) 
                        {
                            
                            oneUser.Username = user.UserName;
                            oneUser.LastName = user.LastName;
                            oneUser.FirstName = user.FirstName;
                            oneUser.Email = user.Email;
                            oneUser.CreatedOn = user.CreatedOn.Value.ToString(dateTimeFormat , CultureInfo.InvariantCulture);
                            oneUser.ActiveInactive = user.Active.ToString();
                            listOfUsersEvent.Add(oneUser);
                        }
                    }
                   
                }
            }

            return listOfUsersEvent;
        }

        public bool UserIsAttending(string userID, int eventID)
        {
            List<event_users> ev = _db.event_users.ToList();
           
            foreach (event_users e in ev)
            {
                if (e.UserID == userID && e.EventID == eventID)
                {
                    return true;
                }
            }

            
            return false;
        }

        public bool AddUserAttending(string userID, int eventID)
        {
            event_users ev1 = new event_users();
            ev1.EventID = eventID;
            ev1.UserID = userID;
            try
            {
                _db.event_users.Add(ev1);
                _db.SaveChanges();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public List<event_users> GetAllOkSetups()
        {
            return _db.event_users.ToList();
        }
    }
}