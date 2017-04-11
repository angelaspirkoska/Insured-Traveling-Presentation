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
        public static List<language> Languages { private get; set; }

        public static List<language> GetAllanguages()
        {
            InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
            return _db.languages.Where(x => x.Active == true).ToList();
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

        public static string GetCurrentCultureSign()
        {
            var langCookie = HttpContext.Current.Request.Cookies["culture"];
            string lang = null;
            if (langCookie != null)
            {
                return langCookie.Value;
            }
            return lang;
        }

        public static int CurrentLanguageId()
        {
            var lang = GetCurrentCultureSign();
            if(!String.IsNullOrEmpty(lang))
            {
                var language = GetAllanguages().Where(x => x.CultureName.Equals(lang)).FirstOrDefault();
                return language != null ? language.Id : GetDefaultLanguageId();
            }
            return GetDefaultLanguageId();           
        }

        public static int GetDefaultLanguageId()
        {
            return GetAllanguages().FirstOrDefault().Id;
        }
    }
}