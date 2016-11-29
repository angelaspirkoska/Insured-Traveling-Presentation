using InsuredTraveling.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class CountryService : ICountryService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetAll()
        {
            var languageId = SiteLanguages.CurrentLanguageId(); 

            var countries = _db.countries_name.Where(x => x.language_id == languageId).Select(p => new SelectListItem
            {
                Text = p.name,
                Value = p.countries_id.ToString()
            });
            return countries;
        }
    }
}
