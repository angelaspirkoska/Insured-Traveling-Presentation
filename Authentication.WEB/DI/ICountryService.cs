using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public interface ICountryService
    {
        IQueryable<SelectListItem> GetAll();
        List<country> GetAllCountries();
        countries_name GetCountriesENData(int countryId);
        countries_name GetACountriesMKData(int countryId);
    }
}
