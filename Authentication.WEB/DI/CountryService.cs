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
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

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

        public List<country> GetAllCountries()
        {
            return _db.countries.ToList();
        }

        public countries_name GetCountriesENData(int countryId)
        {
            var language = _db.languages.Where(x => x.CultureName == "en").FirstOrDefault();
            return _db.countries_name.Where(x => x.countries_id == countryId && x.language_id == language.Id).FirstOrDefault();
        }

        public countries_name GetACountriesMKData(int countryId)
        {
            var language = _db.languages.Where(x => x.CultureName == "mk").FirstOrDefault();
            return _db.countries_name.Where(x => x.countries_id == countryId && x.language_id == language.Id).FirstOrDefault();
        }
    }
}
