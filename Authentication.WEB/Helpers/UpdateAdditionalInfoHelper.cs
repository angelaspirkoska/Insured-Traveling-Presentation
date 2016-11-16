using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.DI;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.Helpers
{
    public static class UpdateAdditionalInfoHelper
    {
        public static void UpdateAdditionalInfo(FirstNoticeOfLossEditViewModel model, IFirstNoticeOfLossService _fnol, IAdditionalInfoService _ais, ILuggageInsuranceService _lis, IHealthInsuranceService _his)
        {
            var fnol = _fnol.GetById(model.Id);
            var additionalInfo = fnol.additional_info;
            if (fnol.additional_info.health_insurance_info != null)
            {
               
                var healthInfo = fnol.additional_info.health_insurance_info;
                additionalInfo.Accident_place = model.AccidentPlaceHealth;
                additionalInfo.Datetime_accident = model.AccidentDateTimeHealth ?? new DateTime(0, 0, 0);
                additionalInfo.Datetime_accident.Add(model.AccidentTimeHealth ?? new TimeSpan(0,0,0));

                healthInfo.Datetime_doctor_visit = model.DoctorVisitDateTime;
                healthInfo.Doctor_info = model.DoctorInfo;
                healthInfo.Medical_case_description = model.MedicalCaseDescription;
                healthInfo.Previous_medical_history = model.PreviousMedicalHistory;
                healthInfo.Responsible_institution = model.ResponsibleInstitution;

                _ais.UpdateAdditionalAndHealthInfo(additionalInfo, healthInfo);
            }
            else
            {
                var luggageInfo = fnol.additional_info.luggage_insurance_info;
                additionalInfo.Accident_place = model.AccidentPlaceLuggage;
                
                additionalInfo.Datetime_accident = model.AccidentDateTimeLuggage ?? new DateTime(0, 0, 0);
                //additionalInfo.Datetime_accident.Add(model.AccidentTimeLuggage ?? new TimeSpan(0, 0, 0));
                var a =additionalInfo.Datetime_accident.Date + model.AccidentTimeLuggage;
                additionalInfo.Datetime_accident = a ?? new DateTime(0, 0, 0);


                luggageInfo.Place_description = model.PlaceDescription;
                luggageInfo.Detail_description = model.DetailDescription;               
                luggageInfo.Report_place = model.ReportPlace;
                luggageInfo.Floaters = model.Floaters;
                luggageInfo.Floaters_value = Int64.Parse(model.FloatersValue);
                luggageInfo.Luggage_checking_Time = model.LugaggeCheckingTime ?? new TimeSpan(0,0,0);

                _ais.UpdateAdditionalAndLuggageInfo(additionalInfo, luggageInfo);
            }

        }
    }
}