using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI 
{
    public class RolesService : IRolesService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public List<aspnetrole> GetAllRoles()
        {
            return _db.aspnetroles.ToList();
        }
    }
}
