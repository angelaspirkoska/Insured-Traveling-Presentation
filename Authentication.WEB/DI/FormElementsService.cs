using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class FormElementsService : IFormElementsService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public List<form_elements> GetAll(int excelId)
        {
            return _db.form_elements.Where(x => x.ExcelID == excelId).ToList();
        }
    }
}