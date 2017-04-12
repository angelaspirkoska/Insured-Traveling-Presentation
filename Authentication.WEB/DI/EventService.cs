using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Filters;

namespace InsuredTraveling.DI
{
    public class EventService : IEventsService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        RoleAuthorize _roleAuthorize = new RoleAuthorize();

        public List<@event> GetEventsForUser(string username)
        {
            var user = _db.aspnetusers.Where(x => x.UserName == username).FirstOrDefault();
            if (user == null)
                return null;
            if (_roleAuthorize.IsUser("Sava_normal", username))
            {
                return _db.events.Where(x => x.Type == false).ToList();

            }else if (_roleAuthorize.IsUser("Sava_Sport_VIP", username) || _roleAuthorize.IsUser("Sava_Sport+", username))
            {
                return _db.events.ToList();
            }

            return null;
        }

    }
}