using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace InsuredTraveling.App_Start
{
    public class SiteLanguages
    {
        public static List<language> GetAllanguages()
        {
            InsuredTravelingEntity _db = new InsuredTravelingEntity();
            return  _db.languages.Where(x => x.Active == true).ToList();
        }
        public static bool IsLanguageAvailable(string lang)
        {
            return GetAllanguages().Where(a => a.CultureName.Equals(lang)).FirstOrDefault() != null ? true : false;
        }

        public static string GetDefaultLanguage()
        {
            return GetAllanguages().FirstOrDefault().CultureName;
        }

        public void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang))
                    lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie langCookie = new HttpCookie("culture", lang);
                langCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(langCookie);

            }
            catch (Exception ex) { }
        }
    }
}