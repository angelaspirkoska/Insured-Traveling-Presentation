using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public class FirstNoticeOfLossService : IFirstNoticeOfLossService
    {
        private InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public void Add(first_notice_of_loss FirstNoticeOfLoss)
        {
            _db.first_notice_of_loss.Add(FirstNoticeOfLoss);
            _db.SaveChanges();
        }

        public first_notice_of_loss Create()
        {
           return _db.first_notice_of_loss.Create();
        }

        public List<first_notice_of_loss> GetAll()
        {
           return _db.first_notice_of_loss.ToList();
        }

        public first_notice_of_loss GetById(int id)
        {
           return _db.first_notice_of_loss.Where(x => x.LossID == id).ToArray().First();
        }

        public first_notice_of_loss[] GetByInsuredUserId(string id)
        {
            return _db.first_notice_of_loss.Where(x => x.Insured_User == id).ToArray();
        }
    }
}
