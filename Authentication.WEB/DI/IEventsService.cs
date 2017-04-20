using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface IEventsService
    {
        List<@event> GetEventsForUser(string username);
        List<@event> GetEventsBySearchValues(string createdBy, string title, string organizer, string location);
        int AddEvent(@event newEvent);
    }
}