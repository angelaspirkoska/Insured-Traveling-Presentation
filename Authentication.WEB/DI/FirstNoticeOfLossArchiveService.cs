using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class FirstNoticeOfLossArchiveService : IFirstNoticeOfLossArchiveService
    {
        private readonly InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

        public int Archive(first_notice_of_loss_archive archiveFnol)
        {
            _db.first_notice_of_loss_archive.Add(archiveFnol);
            _db.SaveChanges();
            return archiveFnol.ID;
        }

        public List<first_notice_of_loss_archive> GetFNOLArchiveByFNOLId(int fnolID)
        {
            var archived = _db.first_notice_of_loss_archive.Where(x => x.fnol_ID == fnolID);
            return archived.OrderByDescending(x => x.Created_Datetime).ToList();
        }

        public first_notice_of_loss_archive GetFNOLArchivedById(int ID)
        {
            return _db.first_notice_of_loss_archive.Where(x => x.ID == ID).FirstOrDefault();
        }
    }
}