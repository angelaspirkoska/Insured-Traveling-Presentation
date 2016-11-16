using System;
using System.Linq;

namespace InsuredTraveling.DI
{
    public class AdditionalInfoService : IAdditionalInfoService
    {
        private InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int Add( additional_info additionalInfo)
        {

            _db.additional_info.Add(additionalInfo);
            _db.SaveChanges();
            return additionalInfo.ID;
        }

        public int AddHealthInsuranceInfo(health_insurance_info healthInsuranceInfo)
        {
            _db.health_insurance_info.Add(healthInsuranceInfo);
            return _db.SaveChanges();
        }

        public int AddLuggageInsuranceInfo(luggage_insurance_info luggageInsuranceInfo)
        {
            _db.luggage_insurance_info.Add(luggageInsuranceInfo);
            return _db.SaveChanges();
        }

        public additional_info Create()
        {
            return _db.additional_info.Create();
        }

        public health_insurance_info CreateHealthInsuranceInfo()
        {
            return _db.health_insurance_info.Create();
        }

        public luggage_insurance_info CreateLuggageInsuranceInfoInsuranceInfo()
        {
            return _db.luggage_insurance_info.Create();
        }

        public additional_info GetAdditionalInfoById(int id)
        {
            return _db.additional_info.Single(x => x.ID == id);          
        }

        public health_insurance_info GetHealthInsuranceInfoById(int id)
        {
            return _db.health_insurance_info.Single(x => x.Additional_infoId == id);
        }

        public luggage_insurance_info GetLuggageInsuranceInfoById(int id)
        {
            return _db.luggage_insurance_info.Single(x => x.Additional_infoId == id);
        }

        public void UpdateAdditionalAndHealthInfo(additional_info additionalInfo, health_insurance_info HealthInfo)
        {
            var oldAddInfo = _db.additional_info.Where(x => x.ID == additionalInfo.ID).SingleOrDefault();
            oldAddInfo = additionalInfo;


            var oldHealthInfo = _db.health_insurance_info.Where(x => x.Additional_infoId == HealthInfo.Additional_infoId).SingleOrDefault();
            oldHealthInfo = HealthInfo;
            _db.SaveChanges();  
        }

        public void UpdateAdditionalAndLuggageInfo( additional_info additionalInfo, luggage_insurance_info LugaggeInfo)
        {
            var oldAddInfo = _db.additional_info.Where(x => x.ID == additionalInfo.ID).SingleOrDefault();
            oldAddInfo = additionalInfo;

            var oldLuggInfo = _db.luggage_insurance_info.Where(x => x.Additional_infoId == LugaggeInfo.Additional_infoId).SingleOrDefault();
            oldLuggInfo = LugaggeInfo;
            _db.SaveChanges();
        }
    }
}
