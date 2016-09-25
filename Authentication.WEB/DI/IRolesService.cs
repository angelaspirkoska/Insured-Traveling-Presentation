using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InsuredTraveling.Models.AdminPanel;

namespace InsuredTraveling.DI
{
   public interface IRolesService
    {
        List<aspnetrole> GetAllRoles();
       
    }
}
