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
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
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

     
        public string GetRoleById(string userID)
        {
            var tempUser = _db.aspnetroles.Where(x => x.Id.Equals(userID)).FirstOrDefault();
            var tempa = _db.aspnetusers.Where(x => x.Id.Equals(userID)).FirstOrDefault();
            
            return tempUser.Id;
        }

    }
}
