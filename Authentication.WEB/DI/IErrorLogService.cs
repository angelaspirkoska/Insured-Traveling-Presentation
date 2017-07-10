using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface IErrorLogService
    {
        IQueryable<elmah_error> GetAllErrorLogs();

         void AddLog(elmah_error ErrorLog);
    }
}