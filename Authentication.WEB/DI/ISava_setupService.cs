using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface ISava_setupService
    {
        void AddSavaOkSetup(Models.Sava_AdminPanelModel ok);
        List<sava_setup> GetAllSavaSetups();
        sava_setup GetLast();
        void DeleteOkSetup(int id);
    }
}
