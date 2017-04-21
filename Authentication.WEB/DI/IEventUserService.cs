using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IEventUserService
    {
        int CountPeopleAttending(int eventName);
        List<SearchRegisteredUser> PeoplePerEventAttending(int eventName);
        bool UserIsAttending(string userID, int eventID);
        bool AddUserAttending(string userID, int eventID);
    }
}
