using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IFirstNoticeOfLossService
    {
         List<first_notice_of_loss> GetAll();

        first_notice_of_loss GetById(int id);

        first_notice_of_loss[] GetByInsuredUserId(string id);

        void Add(first_notice_of_loss FirstNoticeOfLoss);

        first_notice_of_loss Create();
    }
}
