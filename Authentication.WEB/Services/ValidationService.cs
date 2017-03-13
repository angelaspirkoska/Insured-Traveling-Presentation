using Authentication.WEB.Models;
using InsuredTraveling;
using System;
using System.Linq;

namespace Authentication.WEB.Services
{
    class ValidationService
    {
        public bool masterValidate(travel_policy policy)
        {
            if (!validateEMBG(policy.aspnetuser.EMBG))
                return false;
            if (!validateDates(policy.Start_Date, policy.End_Date))
                return false;
            if (!validateInsuredDays(policy.Start_Date, policy.End_Date, policy.Valid_Days, policy.travel_insurance_type.Name))
                return false;
            if (!validateAge(policy.Start_Date, policy.aspnetuser.EMBG, policy.policy_type.type))
                return false;
            return true;
        }

        public bool validateSSN(string embg = null)
        {
            if (embg == null)
                return false;

            //checks if embg has 13 characters
            if (embg.Count() != 13)
                return false;

            //checks if the embg is all numbers
            long isNumeric = 0;
            long.TryParse(embg, out isNumeric);

            if (isNumeric == 0)
                return false;

            //gets the day, month and year of birth
            int day;
            int month;
            int year;
            int.TryParse(embg.Substring(0, 2), out day);
            int.TryParse(embg.Substring(2, 2), out month);
            int.TryParse(embg.Substring(4, 3), out year);

            //checks the day, month and year



            if (month < 1 || month > 12)
                return false;
            if (year < 900 && (year + 2000) > DateTime.Now.Year)
                return false;
            // This is for finding exact days in month of the year.
            if (day < 1 || day > System.DateTime.DaysInMonth(year, month))
                return false;

            return true;
        }
        public bool validateEMBG_Advanced(string embg = null)
        {
            //return true;

            if (embg == null)
                return false;

            //checks if embg has 13 characters
            if (embg.Count() != 13)
                return false;

            //checks if the embg is all numbers
            long isNumeric = 0;
            long.TryParse(embg, out isNumeric);

            if (isNumeric == 0)
                return false;

            int a, b, c, d,e, f, g, h, i, j, k, l;
            int ch; int Result=0;

            int.TryParse(embg.Substring(0, 1), out a);
            int.TryParse(embg.Substring(1, 1), out  b);
            int.TryParse(embg.Substring(2, 1), out c);
            int.TryParse(embg.Substring(3, 1), out d);
            int.TryParse(embg.Substring(4, 1), out e);
            int.TryParse(embg.Substring(5, 1), out f);
            int.TryParse(embg.Substring(6, 1), out g);
            int.TryParse(embg.Substring(7, 1), out h);
            int.TryParse(embg.Substring(8, 1), out i);
            int.TryParse(embg.Substring(9, 1), out j);
            int.TryParse(embg.Substring(10, 1), out k);
            int.TryParse(embg.Substring(11, 1), out l);
            int.TryParse(embg.Substring(12, 1), out ch);
            Result = 11 - ((a + g) * 7 + (b + h) * 6 + (c + i) * 5 + (d + j) * 4 + (e + k) * 3 + (f + l) * 2) % 11;
            if (Result == 11)
            {
                Result = 0;
            }
            if (Result == ch)
            {
                return true;
            }
              else 
            {
                return false;
            }
           
        }

        public bool validateDates(DateTime starDate, DateTime endDate)
        {
            if (starDate < DateTime.UtcNow || endDate < DateTime.UtcNow)
                return false;
            if (starDate > endDate)
                return false;
            return true;
        }

        public bool validateInsuredDays(DateTime startDate, DateTime endDate, int insuredDays, string brojPatuvanja)
        {
            int dayDifference = (endDate - startDate).Days;
            if (dayDifference > 366)
                return false;
            if (brojPatuvanja.Equals("ednoPatuvanje") && insuredDays < 15)
                return false;
            return true;
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

        public bool validateAge(DateTime startDate, string EMBG, string policyType)
        {
            if (countAge(startDate, EMBG) >= 72)
                return false;
            if (countAge(startDate, EMBG) >= 70 && !policyType.Equals("Visa"))
                return false;
            return true;
        }
    }
}
