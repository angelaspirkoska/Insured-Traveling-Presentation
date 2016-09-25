using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class FranchiseService : IFranchiseService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetAll()
        {
            var franchise = _db.retaining_risk.Select(p => new SelectListItem
            {
                Text = p.Franchise,
                Value = p.ID.ToString()
            });

            return franchise;
        }
    }
}
