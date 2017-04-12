using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface IEventsService
    {
        List<@event> GetEventsForUser(string username);
    }
}