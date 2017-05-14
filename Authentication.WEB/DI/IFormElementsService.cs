using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
     public interface IFormElementsService
    {
        List<form_elements> GetAll(int excelId);
    }
}
