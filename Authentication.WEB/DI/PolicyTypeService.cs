using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class PolicyTypeService : IPolicyTypeService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetAll()
        {
           
        
            var policy = _db.policy_type.Select(p => new SelectListItem
            {
                Text = p.type,
                Value = p.ID.ToString()
            });

            return policy; 
    }
}
}
