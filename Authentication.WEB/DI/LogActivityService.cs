using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class LogActivityService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
       public void AddLog(log_activities LogActivity)
        {
            
            _db.log_activities.Add(LogActivity);
            _db.SaveChanges();
        }
    }
}