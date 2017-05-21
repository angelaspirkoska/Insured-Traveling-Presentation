using Authentication.WEB.Models;
using System;
using System.Linq;
using InsuredTraveling;
using InsuredTraveling.Models;

namespace Authentication.WEB.Services
{
    class RatingEngineService
    {
        private InsuredTravelingEntity entities = new InsuredTravelingEntity();
        public double? discountCountry(int? countryID, int policy_typeID, int franchiseID)
        {
            double? discount = (entities.discount_country.Where(x => x.CountryID == countryID &&
                                                        x.Policy_typeID == policy_typeID && x.Franchise == franchiseID.ToString()).First()).Percentage;
            return discount;
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
            double? discount = (entities.discount_age.Where(x => x.Name == name).First()).Discount;
            return discount;
        }

        public double? DiscountFranchise(int? countryID, int policy_typeID, int franchiseID)
        {
            double? discount = (entities.discount_country.Where(x => x.CountryID == countryID &&
                x.Policy_typeID == policy_typeID && x.Franchise.Equals(franchiseID.ToString())).First()).Discount_franchise;
            return discount;
        }

        public double? DiscountDays(int policy_typeID, long days)
        {
            int travelDurationID;

            if (days < 14)
            {
                travelDurationID = entities.travel_duration.Where(x => x.Days == "Do 14 dena").Single().ID;
            } else if (days > 14 && days < 30)
            {
                travelDurationID = entities.travel_duration.Where(x => x.Days == "Nad 15 dena").Single().ID;
            } else
            {
                travelDurationID = entities.travel_duration.Where(x => x.Days == "Nad 30 dena").Single().ID;
            }

            var p = (entities.discount_days.Where(x => x.Policy_typeID == policy_typeID && x.Travel_durationID == travelDurationID) //Treba da se vide shto se prima vo denovi 
                .OrderByDescending(x => x.Travel_durationID).First());
            double? discount = p.Discount;
            return discount;
        }

        public double? DiscountFamily(int policy_typeID)
        {
            double? discount = (entities.discount_family.Where(x => x.Policy_typeID == policy_typeID).First()).Discount;
            return discount;
        }

        public double? DiscountGroup(int policy_typeID, int? members)
        {
            double? discount = (entities.discount_group.Where(x => x.Policy_typeID == policy_typeID).Where(x => x.group.Memebers < members)
                .OrderByDescending(x => x.group.Memebers).First()).Discount;
            return discount;
        }

        public double? Discount(string DiscountCode )
        {
            if ((entities.discount_codes.Where(x => ( x.Discount_Name == DiscountCode) && ( x.End_Date >= DateTime.Now ))).Count() >= 1)
                {
                double? discount = (entities.discount_codes.Where(x => x.Discount_Name == DiscountCode).First()).Discount_Coef;

                return discount;
            }
            return null;
        }

        public double? procentDoplata(double? ac1, double? ac2)
        {
            return ac1*ac2;
        }

        public int countAgeUsingSSN(DateTime policyStart, string embg = null)
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
        public int countAgeUsingBirthDate(DateTime policyStart, DateTime birthDate)
        {
            if (birthDate == null)
                return -1;

            int age = policyStart.Year - birthDate.Year;
            if (policyStart.Month < birthDate.Month || (policyStart.Month == birthDate.Month && policyStart.Day < birthDate.Day))
                age--;

            return age;
        }

        public double? totalPremium(Policy policy)
        {           
            double? dCountry = discountCountry(policy.CountryID, policy.Policy_TypeID, policy.Retaining_RiskID);
            double? dFranchise = DiscountFranchise(policy.CountryID, policy.Policy_TypeID, policy.Retaining_RiskID);
            double? dDays = DiscountDays(policy.Policy_TypeID, policy.Valid_Days);
            string ssnInsured = " ";
            insured[] p_i = policy.insureds.ToArray();
            foreach(insured p in p_i)
            {
                if(p.Type_InsuredID.Equals(InsuredType.Type.insured))
                {
                    ssnInsured = p.SSN;
                    break;
                }
            }

            double? exchange_rate = entities.exchange_rate.First().Value;
            double? minPremium = exchange_rate * dCountry * (1 - dFranchise)  * (1 - dDays) * policy.Valid_Days;

            if (policy.Travel_Insurance_TypeID == 1)
            {
                double? pVozrast = DiscountAge(countAgeUsingBirthDate(policy.Start_Date, policy.BirthDate));
                minPremium *= (1 - pVozrast);
            }
            if (policy.Travel_Insurance_TypeID==2)
            {
                double? dFamily = DiscountFamily(policy.Policy_TypeID);
                double? dAge;

                foreach (insured i in p_i)
                {
                    dAge = DiscountAge(countAgeUsingBirthDate(policy.Start_Date, i.DateBirth));
                    double? osnovnaPremija1 = exchange_rate * dCountry * (1 - dFranchise) * policy.Valid_Days * (1 - dDays) * (1 - dAge);
                    minPremium += osnovnaPremija1;
                }

                minPremium *= (1 - dFamily);
            }
            if (policy.Travel_Insurance_TypeID == 3)
            {
                double? dGroup = DiscountGroup(policy.Policy_TypeID, policy.Group_Members);
                minPremium *= policy.Group_Members * (1 - dGroup);
            }
            
            if (policy.DiscountCode != null && policy.DiscountCode != "" )
            {
                double? discount = Discount(policy.DiscountCode);
                if(discount != null || discount != 0)
                {
                    minPremium = minPremium * (1 - (discount / 100));
                }
               
            }

            if(policy.additional_charges != null)
            {
                additional_charge[] aditional_charges = policy.additional_charges.ToArray();

                double? additional_charge1 = 1;
                double? additional_charge2 = 1;
                if (aditional_charges.Count() != 0)
                {
                    int id1 = aditional_charges[0].ID;
                    int id2 = aditional_charges.Count() == 2 ? aditional_charges[1].ID : 1;
                    additional_charge1 = entities.additional_charge.Where(x => x.ID == id1).Single().Percentage;
                    additional_charge2 = entities.additional_charge.Where(x => x.ID == id2).Single().Percentage;
                }
                double? pDoplata = procentDoplata(additional_charge1, additional_charge2);
                minPremium *= pDoplata;
            }

            int roundedPremium = (int)minPremium;
            roundedPremium = ((int)Math.Round(roundedPremium / 10.0)) * 10;

            if (roundedPremium < 200)
                roundedPremium = 200;

            return roundedPremium;
        }

        public double? totalPremiumSava(Policy policy, int policyPackageType, int policyTypeSava, 
            int durationType, int openDurationDays, int persSport, string isProfessionalDriver, string isAbroadStudent, string extraNezgoda, 
            string extraDomasnaAsistencija, string extraAvtoAsistencija, int numOfInsureds)
        {
            //osnovna premija

            var basePremiumByDay = 0.0;
            var premium = 0.0;
            if(durationType == 0)
            {
                if (policyPackageType == 1)
                    basePremiumByDay = 0.5;
                if (policyPackageType == 2)
                    basePremiumByDay = 0.7;
                if (policyPackageType == 3)
                    basePremiumByDay = 1;
                premium = basePremiumByDay * policy.Valid_Days;
            } else
            {
                if(openDurationDays == 6)
                {
                    if (policyPackageType == 1)
                    {
                        if (policy.Valid_Days == 30)
                            premium = 18;
                        if (policy.Valid_Days == 60)
                            premium = 36;
                        if (policy.Valid_Days == 90)
                            premium = 54;
                    }
                    if (policyPackageType == 2)
                    {
                        if (policy.Valid_Days == 30)
                            premium = 25;
                        if (policy.Valid_Days == 60)
                            premium = 50;
                        if (policy.Valid_Days == 90)
                            premium = 76;
                    }
                    if (policyPackageType == 3)
                    {
                        if (policy.Valid_Days == 30)
                            premium = 36;
                        if (policy.Valid_Days == 60)
                            premium = 72;
                        if (policy.Valid_Days == 90)
                            premium = 108;
                    }
                }
                if(openDurationDays == 12)
                {
                    if (policyPackageType == 1)
                    {
                        if (policy.Valid_Days == 60)
                            premium = 36;
                        if (policy.Valid_Days == 90)
                            premium = 54;
                        if (policy.Valid_Days == 120)
                            premium = 72;
                    }
                    if (policyPackageType == 2)
                    {
                        if (policy.Valid_Days == 60)
                            premium = 50;
                        if (policy.Valid_Days == 90)
                            premium = 76;
                        if (policy.Valid_Days == 120)
                            premium = 101;
                    }
                    if (policyPackageType == 3)
                    {
                        if (policy.Valid_Days == 60)
                            premium = 72;
                        if (policy.Valid_Days == 90)
                            premium = 108;
                        if (policy.Valid_Days == 120)
                            premium = 144;
                    }
                }
            }

            //po broj na lica
            premium *= numOfInsureds;

            //doplatoci
            var age = DateTime.Now.Year - policy.BirthDate.Year;
            if (policy.BirthDate > DateTime.Now.AddYears(-age))
                age--;
            if (age >= 65 && age <= 70)
                premium *= 2;

            if (isAbroadStudent == "on")
                premium *= 1.5;
            if (isProfessionalDriver == "on")
                premium *= 2;

            if (persSport == 1)
                premium *= 1.6;

            //popusti
            if (policyTypeSava == 2 || policyTypeSava == 3)
                premium = premium - 0.15 * premium;
            if (policyTypeSava == 4 || policyTypeSava == 5)
                premium = premium - 0.2 * premium;
            
            //popust fransiza
            if(policy.Retaining_RiskID == 2)
            {
                premium = premium - 0.20 * premium;
            }
            else if(policy.Retaining_RiskID == 3)
            {
                premium = premium - 0.30 * premium;
            }

            //extra nezgoda, avto...
            var premiumNezgoda = 0.0;
            //ovde treba po broj na lica
            if (extraNezgoda == "on")
                premiumNezgoda = 2 * policy.Valid_Days;

            var premiumAvto = 0.0;
            var premiumDomasna = 0.0;
            //ovde treba po broj na lica
            if (extraAvtoAsistencija == "on")
            {
                if (policy.Valid_Days < 8)
                {
                    premiumAvto = 3.3 * 61.5;
                    premiumDomasna = 203;
                }
                else if (policy.Valid_Days < 15)
                {
                    premiumAvto = 4.1 * 61.5;
                    premiumDomasna = 252;
                }
                else if (policy.Valid_Days < 30)
                {
                    premiumAvto = 4.9 * 61.5;
                    premiumDomasna = 301;
                }
                else
                {
                    premiumAvto = 11 * 61.5;
                    premiumDomasna = 661;
                }
            }

            if (premium < 2)
                premium = 2;

            return Math.Ceiling(premium * 61.5) + premiumNezgoda + premiumAvto + premiumDomasna;
        }
    }
}
