using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
   public interface ILogActivityService
    {
        IQueryable<log_activities> GetAllLogs();
       void AddLog(log_activities LogActivity);
    }

}
