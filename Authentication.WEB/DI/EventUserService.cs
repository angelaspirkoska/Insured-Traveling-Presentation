using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
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
                if (event_us.EventID == eventName)
                {
                    foreach (aspnetuser user in Alluser)
                    {
                        if ( event_us.UserID == user.Id ) 
                        {
                            
                            oneUser.ID = user.UserName;
                            oneUser.LastName = user.LastName;
                            oneUser.FirstName = user.FirstName;
                            oneUser.Email = user.Email;
                            oneUser.CreatedOn = user.CreatedOn.ToString();
                            oneUser.ActiveInactive = user.Active.ToString();
                            listOfUsersEvent.Add(oneUser);
                        }
                    }
                   
                }
            }

            return listOfUsersEvent;
        }

        public List<event_users> GetAllOkSetups()
        {
            return _db.event_users.ToList();
        }
    }
}