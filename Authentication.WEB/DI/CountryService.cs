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
            var Countries = _db.countries.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString()
            });
            return Countries;
        }
    }
}
