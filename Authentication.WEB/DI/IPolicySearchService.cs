using InsuredTraveling.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface IPolicySearchService
    {
        List<SearchPolicyViewModel> GetCountriesName(List<SearchPolicyViewModel> searchModel, int languageId);
    }
}