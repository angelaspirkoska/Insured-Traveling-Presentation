using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InsuredTraveling.Models.AdminPanel;

namespace InsuredTraveling.DI
{
   public interface IOkSetupService
    {

        List<ok_setup> GetAllOkSetups();

        void AddOkSetup(ok_setup ok); //????? dali ili samo se menja status

        void DeleteOkSetup(int id);

        ok_setup GetLastByInsuranceCompany(string InsuranceCompany);
        
    }
}
