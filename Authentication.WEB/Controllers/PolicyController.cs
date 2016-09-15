using Authentication.WEB.Models;
using Authentication.WEB.Services;
using Newtonsoft.Json.Linq;
using System;
using Rotativa;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web.Mvc;
using InsuredTraveling;

namespace Authentication.WEB.Controllers
{
    public class PolicyController : Controller
    {
        // GET: Policy
        [HttpGet]
        public ActionResult Index()
        {
            PolicyInfoList info = new PolicyInfoList();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            info.countries = entities.countries;
            info.franchises = entities.retaining_risk_value;
            info.policies = entities.policy_type;
            info.doplatokList = entities.p_doplatoci;

            return View(info);
        }

        public async System.Threading.Tasks.Task<ActionResult> CreatePolicy(Policy policy)
        {
            ValidationService validationService = new ValidationService();
            RatingEngineService ratingEngineService = new RatingEngineService();

            InsuredTravelingEntity entityDB = new InsuredTravelingEntity();
            policy polisaEntity = new policy();

            Uri uri = new Uri("http://localhost:19655/api/premium/calculate");
            HttpClient client = new HttpClient();
            client.BaseAddress = uri;
            var mediaType = new MediaTypeHeaderValue("application/json");
            var jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<Policy>(policy, jsonFormatter);



            HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(responseBody);
            int premium = data.PremiumAmount;

            bool valid = validationService.masterValidate(policy);
            double? vkupnaPremija = ratingEngineService.totalPremium(policy);
            policy.VkupnaPremija = vkupnaPremija;
            System.Web.HttpContext.Current.Session.Add("SessionId", policy.policyNumber);

            if (valid)
            {

                long ID = (entityDB.policies.OrderByDescending(p => p.Polisa_Broj).Select(r => r.Polisa_Broj).FirstOrDefault() + 1);
                string ID_Company = entityDB.policies.OrderByDescending(p => p.Polisa_Broj).Select(r => r.BRoj_Polisa_Kompanija).FirstOrDefault();
                int tempID;
                if (String.IsNullOrEmpty(ID_Company))
                {
                    tempID = 0;
                }
                else {
                    string ID_trim = ID_Company.Substring(0, 4);
                    string ID_trim2 = ID_Company.Substring(5);
                    tempID = int.Parse(ID_trim2) + 1;
                }
                // if (user == sava ) 

                polisaEntity.Polisa_Broj = ID;
                polisaEntity.BRoj_Polisa_Kompanija = "Sava_" + tempID;

                polisaEntity.Vid_Polisa = policy.vidPolisa;

                polisaEntity.Fransiza = policy.Franchise;
                polisaEntity.Popust_Fransiza = policy.procentFranchise; // Mozebi gresno

                polisaEntity.Zemja_Na_Patuvanje = policy.zemjaNaPatuvanje;

                polisaEntity.Ime_I_Prezime = policy.imePrezime;
                polisaEntity.Adresa = policy.Adresa;
                polisaEntity.EMBG = policy.EMBG;
                polisaEntity.Broj_Pasos = policy.brojPasos;

                polisaEntity.Zapocnuva_Na = policy.startDate;
                polisaEntity.Zavrsuva_Na = policy.endDate;
                polisaEntity.Vazi_Denovi = policy.vaziDenovi;
                polisaEntity.Popust_Denovi = policy.popustDenovi; //StringDouble ?




                if (policy.brojPatuvanja == "ednoPatuvanje")
                    polisaEntity.Pat_Edno = true;
                // Mozna greska pri validacija 

                //   polisaEntity.Pat_Poveke = policy.povekePatuvanja; // Mozna greska pri validacija =/

                if (policy.brojOsigureniciVid == "polisaPoedinecno")
                {
                    polisaEntity.Vid_Poedinecno = true;
                }
                else if (policy.brojOsigureniciVid == "polisaFamilijarno")
                {
                    polisaEntity.Vid_Grupno = true; ;
                }
                else if (policy.brojOsigureniciVid == "polisaGrupno")
                {
                    polisaEntity.Vid_Familijarno = true;
                }

                polisaEntity.Grupa = policy.brojLicaGrupa; // Broj lica  ??
                polisaEntity.Vkupna_Premija = policy.VkupnaPremija;

                polisaEntity.Doplatok_1 = policy.doplatok1;
                polisaEntity.Doplatok_2 = policy.doplatok2;

                //polisaEntity.Osigurenik1_EMBG = policy.osigurenik1ImePrezime;
                //polisaEntity.Osigurenik2_EMBG = policy.osigurenik2ImePrezime;
                //polisaEntity.Osigurenik3_EMBG = policy.osigurenik3ImePrezime;
                //polisaEntity.Osigurenik4_EMBG = policy.osigurenik4ImePrezime;
                //polisaEntity.Osigurenik5_EMBG = policy.osigurenik5ImePrezime;
                //polisaEntity.Osigurenik6_EMBG = policy.osigurenik6ImePrezime;

                //polisaEntity.Osigurenik1_Ime_I_Prezime = policy.osigurenik1ImePrezime;
                //polisaEntity.Osigurenik2_Ime_I_Prezime = policy.osigurenik2ImePrezime;
                //polisaEntity.Osigurenik3_Ime_I_Prezime = policy.osigurenik3ImePrezime;
                //polisaEntity.Osigurenik4_Ime_I_Prezime = policy.osigurenik4ImePrezime;
                //polisaEntity.Osigurenik5_Ime_I_Prezime = policy.osigurenik5ImePrezime;
                //polisaEntity.Osigurenik6_Ime_I_Prezime = policy.osigurenik6ImePrezime;

                polisaEntity.Kurs = policy.kurs;



                entityDB.policies.Add(polisaEntity);
                var result = entityDB.SaveChanges();
            }

            return Json(new { success = true, responseText = "Success." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPolicy()
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            // long id = Convert.ToInt64 (System.Web.HttpContext.Current.Session["SessionId"]);
            int id = 2;

            PaymentModel pat = new PaymentModel();
            pat.Pat = entities.policies.Where(x => x.Polisa_Broj == id).FirstOrDefault();
            return new ViewAsPdf("Print", pat);
        }
    }
}