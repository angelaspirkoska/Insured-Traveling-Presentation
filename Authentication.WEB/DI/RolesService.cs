using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;
using System.Web.Mvc;

namespace InsuredTraveling.DI 
{
    public class RolesService : IRolesService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public List<aspnetrole> GetAllRoles()
        {
            return _db.aspnetroles.ToList();
        }

        public IQueryable<SelectListItem> GetAll()
        {
            var roles = _db.aspnetroles.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Name,
                Selected = (p.Name == "End user") ? true : false
            });


            

            return roles;
        }
    }
}
