using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IAdditionalInfoService
    {
        additional_info Create();

        int Add(additional_info additionalInfo);
        additional_info GetAdditionalInfoById(int id);

        health_insurance_info CreateHealthInsuranceInfo();
        int AddHealthInsuranceInfo(health_insurance_info healthInsuranceInfo);
        health_insurance_info GetHealthInsuranceInfoById(int id);

        luggage_insurance_info CreateLuggageInsuranceInfoInsuranceInfo();
        int AddLuggageInsuranceInfo(luggage_insurance_info luggageInsuranceInfo);
        luggage_insurance_info GetLuggageInsuranceInfoById(int id);

        void UpdateAdditionalAndHealthInfo(additional_info additionalInfo, health_insurance_info HealthInfo);
        void UpdateAdditionalAndLuggageInfo(additional_info additionalInfo, luggage_insurance_info LugaggeInfo);

    }
}
