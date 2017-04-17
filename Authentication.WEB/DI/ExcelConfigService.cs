using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ExcelConfigService : IExcelConfigService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddExcelConfig(excelconfig excelConfig)
        {
            
            _db.excelconfigs.Add(excelConfig);
            _db.SaveChanges();
            return excelConfig.ID;
        }

    }
}