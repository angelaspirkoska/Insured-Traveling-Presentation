using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IFirstNoticeOfLossArchiveService
    {
        int Archive(first_notice_of_loss_archive archiveFnol);
        List<first_notice_of_loss_archive> GetFNOLArchiveByFNOLId(int fnolID);
        first_notice_of_loss_archive GetFNOLArchivedById(int ID);
    }
}
