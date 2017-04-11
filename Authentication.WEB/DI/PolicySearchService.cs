using InsuredTraveling.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class PolicySearchService : IPolicySearchService
    {
        private readonly InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

        public List<SearchPolicyViewModel>  GetCountriesName(List<SearchPolicyViewModel> searchModel, int languageId)
        {
            foreach(var policy in searchModel)
            {
                var countryName = _db.countries_name.Where(x => x.countries_id == policy.CountryId && x.language_id == languageId).FirstOrDefault();
                policy.Country = countryName != null ? countryName.name : null;
            }
            return searchModel;
        }
    }
}