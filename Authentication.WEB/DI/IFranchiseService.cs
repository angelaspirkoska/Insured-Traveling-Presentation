using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
   public interface IFranchiseService
    {

        IQueryable<SelectListItem> GetAll();
        List<retaining_risk> GetAllRetainingRisks();
        retaining_risk_name GetRetainingRiskENData(int retainingRiskID);
        retaining_risk_name GetRetainingRiskMKData(int retainingRiskID);

    }
}
