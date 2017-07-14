using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface ILogService
    {
         int AddFNOL_log(first_notice_of_loss_log firstNoticeOfLossLog, string ipAddress, int idFnol);
 
    }
}