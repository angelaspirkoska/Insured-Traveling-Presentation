using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ErrorLogService: IErrorLogService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public void AddLog(elmah_error ErrorLog)
        {

            _db.elmah_error.Add(ErrorLog);
            _db.SaveChanges();
        }

        public IQueryable<elmah_error> GetAllErrorLogs()
        {
            IQueryable<elmah_error> errors = _db.elmah_error;
            return errors;
        }
    }
}