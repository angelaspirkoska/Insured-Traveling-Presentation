using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    interface ILogActivityService
    {
        int AddLog(log_activities LogActivity);
    }
}
