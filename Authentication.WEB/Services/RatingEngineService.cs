using Authentication.WEB.Models;
using System;
using System.Linq;
using InsuredTraveling;
using InsuredTraveling.Models;

namespace Authentication.WEB.Services
{
    class RatingEngineService
    {
        public double? discountCountry(string country, string policy_type, string franchise)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.discount_country.Where(x => x.country.Name.Equals(country) &&
                x.policy_type.type.Equals(policy_type) && x.Franchise.Equals(franchise)).First()).Percentage;
            return popust;
        }

        public double? DiscountAge(int age)
        {
            string name = " ";
            if (age < 18)
            {
                name = "0<=17";
            } else if (age < 64)
            {
                name = "17<64";
            } else {
                name = ">64";
            }
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? discount = (entities.discount_age.Where(x => x.Name == name).OrderByDescending(x => x.ID).First()).Discount;
            return discount;
        }

        public double? DiscountFranchise(string country, string policy_type, string franchise)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? discount = (entities.discount_country.Where(x => x.country.Name.Equals(country) &&
                x.policy_type.type.Equals(policy_type) && x.Franchise.Equals(franchise)).First()).Discount_franchise;
            return discount;
        }

        public double? DiscountDays(string policy_type, long days)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            var p = (entities.discount_days.Where(x => x.policy_type.ID.Equals(policy_type)).Where(x => x.travel_duration.Days.Equals(days)) //Treba da se vide shto se prima vo denovi 
                .OrderByDescending(x => x.Travel_durationID).First());
            double? discount = p.Discount;
            return discount;
        }

        public double? DiscountFamily(string policy_type)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? discount = (entities.discount_family.Where(x => x.policy_type.type.Equals(policy_type)).First()).Discount;
            return discount;
        }

        public double? DiscountGroup(string policy_type, int? members)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? discount = (entities.discount_group.Where(x => x.policy_type.type.Equals(policy_type)).Where(x => x.group.Memebers < members)
                .OrderByDescending(x => x.group.Memebers).First()).Discount;
            return discount;
        }

        public double? procentDoplata(double? ac1, double? ac2)
        {
            return ac1*ac2;
        }

        public int countAge(DateTime policyStart, string embg = null)
        {
            if (policyStart == null)
                return -1;

            int day;
            int month;
            int year;
            int.TryParse(embg.Substring(0, 2), out day);
            int.TryParse(embg.Substring(2, 2), out month);
            int.TryParse(embg.Substring(4, 3), out year);

            if (year < 900)
                year += 2000;
            else
                year += 1000;

            DateTime birthDate = new DateTime(year, month, day);

            int age = policyStart.Year - birthDate.Year;
            if (policyStart.Month < birthDate.Month || (policyStart.Month == birthDate.Month && policyStart.Day < birthDate.Day))
                age--;

            return age;
        }

        public double? totalPremium(travel_policy policy)
        {
            double? dCountry = discountCountry(policy.country.Name, policy.policy_type.type, policy.retaining_risk.Franchise);
            double? dFranchise = DiscountFranchise(policy.country.Name, policy.policy_type.type, policy.retaining_risk.Franchise);
            double? dDays = DiscountDays(policy.policy_type.type, policy.Valid_Days);
            string ssnInsured = " ";
            policy_insured[] p_i = policy.policy_insured.ToArray();
            foreach(policy_insured p in p_i)
            {
                if(p.insured.Type_InsuredID.Equals(InsuredType.Type.insured))
                {
                    ssnInsured = p.insured.SSN;
                    break;
                }
            }

            InsuredTravelingEntity entities = new InsuredTravelingEntity();

            double? exchange_rate = entities.exchange_rate.First().Value;
            double? minPremium = exchange_rate * dCountry * (1 - dFranchise)  * (1 - dDays);

            if (policy.Travel_Insurance_TypeID == 1)
            {
                double? pVozrast = DiscountAge(countAge(policy.Start_Date, ssnInsured));
                minPremium *= (1 - pVozrast);
            }
            if (policy.Travel_Insurance_TypeID==2)
            {
                double? dFamily = DiscountFamily(policy.policy_type.type);
                var insuredID = entities.policy_insured.Where(x => x.PolicyID == policy.ID).Single().InsuredID;
                var insured = entities.insureds.Where(x => x.ID == insuredID).ToArray();
                double? dAge;

                foreach (insured i in insured)
                {
                    dAge = DiscountAge(countAge(policy.Start_Date, i.SSN));
                    double? osnovnaPremija1 = exchange_rate * dCountry * (1 - dFranchise) * policy.Valid_Days * (1 - dDays) * (1 - dAge);
                    minPremium += osnovnaPremija1;
                }

                minPremium *= (1 - dFamily);
            }
            if (policy.travel_insurance_type.Name.Equals("Group"))
            {
                double? dGroup = DiscountGroup(policy.policy_type.type, policy.Group_Members);
                minPremium *= policy.Group_Members * (1 - dGroup);
            }

            policy_additional_charge[] aditional_charges = policy.policy_additional_charge.ToArray();

            double? additional_charge1 = 1;
            double? additional_charge2 = 1;
            if (aditional_charges != null)
            {
                additional_charge1 = aditional_charges[0].additional_charge.Percentage;
                additional_charge2 = aditional_charges[1].additional_charge.Percentage;
            }
            double? pDoplata = procentDoplata(additional_charge1, additional_charge2 );
            minPremium *= pDoplata;

            int zaokruzena = (int)minPremium;
            zaokruzena = ((int)Math.Round(zaokruzena / 10.0)) * 10;

            if (zaokruzena < 200)
                zaokruzena = 200;

            return zaokruzena;
        }
    }
}
