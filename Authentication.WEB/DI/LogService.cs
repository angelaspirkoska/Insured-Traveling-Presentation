using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class LogService : ILogService
    {
        private InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddFNOL_log(first_notice_of_loss_log firstNoticeOfLossLog, string ipAddress, int idFnol)
        {
            try
            {
                firstNoticeOfLossLog.IPaddress = ipAddress;
                firstNoticeOfLossLog.Id_first_notice_of_loss = idFnol;
                _db.first_notice_of_loss_log.Add(firstNoticeOfLossLog);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return -1;
            }
        
            return firstNoticeOfLossLog.ID;
        }

       
    }

}