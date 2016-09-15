using Authentication.WEB.Models;
using System;
using System.Linq;
using InsuredTraveling;

namespace Authentication.WEB.Services
{
    class RatingEngineService
    {
        public double? discountCountry(string country, string policy_type, string franchise)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_zemja_na_patuvanje.Where(x => x.country.Name.Equals(country) &&
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
            double? discount = (entities.p_zemja_na_patuvanje.Where(x => x.country.Name.Equals(country) &&
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

        public double? DiscountGroup(string policy_type, long members)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? discount = (entities.discount_group.Where(x => x.policy_type.type.Equals(policy_type)).Where(x => x.group.Memebers < members)
                .OrderByDescending(x => x.group.Memebers).First()).Discount;
            return discount;
        }

        public double? procentDoplata(string doplatok1, string doplatok2)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? doplata = (entities.p_doplatoci.Where(x => x.Doplatok.Equals(doplatok1)).First()).P_Doplatok *
                (entities.p_doplatoci.Where(x => x.Doplatok.Equals(doplatok2)).First()).P_Doplatok;
            return doplata;
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

        public double? totalPremium(Policy policy)
        {
            double? dCountry = discountCountry(policy.zemjaNaPatuvanje, policy.vidPolisa, policy.Franchise);
            double? dFranchise = DiscountFranchise(policy.zemjaNaPatuvanje, policy.vidPolisa, policy.Franchise);
            double? dDays = DiscountDays(policy.vidPolisa, policy.vaziDenovi);

            InsuredTravelingEntity entities = new InsuredTravelingEntity();

            double? exchange_rate = entities.exchange_rate.First().Value;
            double? minPremium = exchange_rate * dCountry * (1 - dFranchise) * policy.vaziDenovi * (1 - dDays);

            if (policy.brojOsigureniciVid.Equals("polisaPoedinecno"))
            {
                double? pVozrast = DiscountAge(countAge(policy.startDate, policy.EMBG));
                minPremium *= (1 - pVozrast);
            }
            if (policy.brojOsigureniciVid.Equals("polisaFamilijarno"))
            {
                double? dFamily = DiscountFamily(policy.vidPolisa);
                var insuredID = entities.policy_insurees.Where(x => x.PolicyID == policy.policyNumber).Single().InsuredID;
                var insured = entities.insureds.Where(x => x.ID == insuredID).ToArray();
                double? dAge;

                foreach (insured i in insured)
                {
                    dAge = DiscountAge(countAge(policy.startDate, i.SSN));
                    double? osnovnaPremija1 = exchange_rate * dCountry * (1 - dFranchise) * policy.vaziDenovi * (1 - dDays) * (1 - dAge);
                    minPremium += osnovnaPremija1;
                }

                minPremium *= (1 - dFamily);
            }
            if (policy.brojOsigureniciVid.Equals("polisaGrupno"))
            {
                double? dGroup = DiscountGroup(policy.vidPolisa, policy.brojLicaGrupa);
                minPremium *= policy.brojLicaGrupa * (1 - dGroup);
            }

            double? pDoplata = procentDoplata(policy.doplatok1, policy.doplatok2);
            minPremium *= pDoplata;

            int zaokruzena = (int)minPremium;
            zaokruzena = ((int)Math.Round(zaokruzena / 10.0)) * 10;

            if (zaokruzena < 200)
                zaokruzena = 200;

            return zaokruzena;
        }
    }
}
