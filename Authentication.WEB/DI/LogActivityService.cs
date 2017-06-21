using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class LogActivityService : ILogActivityService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
       public void AddLog(log_activities LogActivity)
       {

           _db.log_activities.Add(LogActivity);
            _db.SaveChanges();
        }

        public IQueryable<log_activities> GetAllLogs()
        {
            IQueryable<log_activities> logs = _db.log_activities;
            return logs;
        }
    }
}