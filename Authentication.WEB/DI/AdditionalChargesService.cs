using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
   public class AdditionalChargesService : IAdditionalChargesService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetAll()
        {
            var AdditionalCharge = _db.additional_charge.Select(p => new SelectListItem
            {
                Text = p.Doplatok,
                Value = p.ID.ToString()
            });
            return AdditionalCharge;
        }
    }
}
