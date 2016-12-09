using InsuredTraveling.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class FranchiseService : IFranchiseService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetAll()
        {
            var languageId = SiteLanguages.CurrentLanguageId();

            var franchise = _db.retaining_risk_name.Where(x => x.language_id == languageId).Select(p => new SelectListItem
            {
                Text = p.name,
                Value = p.retaining_risk_id.ToString()
            });

            return franchise;
        }

        public List<retaining_risk> GetAllRetainingRisks()
        {
            return _db.retaining_risk.ToList();
        }

        public retaining_risk_name GetRetainingRiskENData(int retainingRiskID)
        {
            var language = _db.languages.Where(x => x.CultureName == "en").FirstOrDefault();
            return _db.retaining_risk_name.Where(x => x.retaining_risk_id == retainingRiskID && x.language_id == language.Id).FirstOrDefault();
        }

        public retaining_risk_name GetRetainingRiskMKData(int retainingRiskID)
        {
            var language = _db.languages.Where(x => x.CultureName == "mk").FirstOrDefault();
            return _db.retaining_risk_name.Where(x => x.retaining_risk_id == retainingRiskID && x.language_id == language.Id).FirstOrDefault();
        }
    }
}
