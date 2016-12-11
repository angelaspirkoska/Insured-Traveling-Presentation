using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface ITravelNumberService
    {
        List<travel_number> GetAllTravelNumbers();
    }
}
