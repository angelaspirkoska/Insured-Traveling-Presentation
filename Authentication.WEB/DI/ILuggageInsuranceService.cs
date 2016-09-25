using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface ILuggageInsuranceService
    {
        luggage_insurance GetById(int id);

        void Add(luggage_insurance LuggageInsurance);
    }
}
