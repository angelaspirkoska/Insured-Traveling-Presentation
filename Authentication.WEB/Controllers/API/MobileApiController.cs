using InsuredTraveling.Models;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System;
using InsuredTraveling.DI;
using InsuredTraveling.Helpers;
using System.Text;
using InsuredTraveling.ViewModels;
using InsuredTraveling.App_Start;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using Authentication.WEB.Models;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;


namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/mobile")]
    public class MobileApiController : ApiController
    {
        private IPolicyService _ps;
        private IUserService _us;
        private IFirstNoticeOfLossService _fnls;
        private ILuggageInsuranceService _lis;
        private IOkSetupService _oss;
        private IHealthInsuranceService _his;
        private IBankAccountService _bas;
        private IInsuredsService _iss;
        private IFirstNoticeOfLossService _fis;
        private IPolicyTypeService _pts;
        private IAdditionalInfoService _ais;
        private IPolicyInsuredService _pis;
        private IAdditionalChargesService _acs;
        private ICountryService _coun;
        private IExchangeRateService _exch;
        private IFranchiseService _fran;
        private ITravelNumberService _tn;
        private IOkSetupService _os;
        private ISava_setupService _sss;
        private ISavaPoliciesService _sps;
        private IEventsService _es;
        private IEventUserService _eus;

        public MobileApiController()
        {

        }

        public MobileApiController(IUserService us,
                                   IPolicyInsuredService pis,
                                   IAdditionalChargesService acs,
                                   IPolicyService ps,
                                   IFirstNoticeOfLossService fnls,
                                   IHealthInsuranceService his,
                                   ILuggageInsuranceService lis,
                                   IOkSetupService oss,
                                   IBankAccountService bas,
                                   IInsuredsService iss,
                                   IFirstNoticeOfLossService fis,
                                   IPolicyTypeService pts,
                                   IAdditionalInfoService ais,
                                   ICountryService coun,
                                   IExchangeRateService exch,
                                   IFranchiseService fran,
                                   ITravelNumberService tn,
                                   IOkSetupService os,
                                   ISava_setupService sss, ISavaPoliciesService sps,
                                   IEventsService es,
                                   IEventUserService eus)
        {
            _ps = ps;
            _us = us;
            _fnls = fnls;
            _lis = lis;
            _oss = oss;
            _his = his;
            _bas = bas;
            _iss = iss;
            _fis = fis;
            _pts = pts;
            _ais = ais;
            _pis = pis;
            _acs = acs;
            _coun = coun;
            _exch = exch;
            _fran = fran;
            _tn = tn;
            _os = os;
            _sss = sss;
            _sps = sps;
            _es = es;
            _eus = eus;
        }

        [Route("IsUserVerified")]
        public JObject IsUserVerified(UserDTO model)
        {
            JObject data = new JObject();

            aspnetuser user = _us.GetUserDataByUsername(model.username);

            data.Add("email", user.EmailConfirmed == true ? true : false);
            data.Add("phone", user.PhoneNumberConfirmed == true ? true : false);

            return data;
            
        }

        [HttpPost]
        [Route("IsUserUpdated")]
        public JObject IsUserUpdated(UserDTO username)
        {
            var user = _us.GetUserDataByUsername(username.username);
            var userUpdated =  new JObject();
            if (user == null)
                return userUpdated;
            userUpdated.Add("updated", user.updated == true? "yes" : "no");
            return userUpdated;           
        }

        [HttpPost]
        [Route("GetEvents")]
        public JObject GetEvents(UserDTO username)
        {
            var data = new JObject();
            var events = _es.GetEventsForUser(username.username);
            if(events == null)
            {
                data.Add("Error", "There are none events for this user");
                return data;
            }
            JArray userEvents = new JArray();
            foreach (var e in events)
            {
                JObject userEvent = new JObject();
                userEvent.Add("Id", e.ID);
                userEvent.Add("CreatedBy", e.aspnetuser.UserName);
                userEvent.Add("Description", e.Description);
                userEvent.Add("Location", e.Location);
                userEvent.Add("Organizer", e.Organizer);
                userEvent.Add("Title", e.Title);
                userEvent.Add("EventType", e.Type == true? "VipEvent" : "Event");
                userEvent.Add("EndDate", e.EndDate.ToString());
                userEvent.Add("StartDate", e.StartDate.ToString());
                userEvent.Add("PublishDate", e.PublishDate.ToString());
                userEvent.Add("Voucher", e.Voucher.ToString());
                userEvent.Add("Chat", e.Chat.ToString());
                bool isAttending = _eus.UserIsAttending(_us.GetUserIdByUsername(username.username), e.ID);
                userEvent.Add("Attending", isAttending.ToString());

                userEvents.Add(userEvent);
            }

            data.Add("events", userEvents);
            return data;
        }

       
        [Route("RetrieveUserInfo")]
        public JObject RetrieveUserInformation(UserDTO username)
        {
            JObject data = new JObject();
            aspnetuser user = _us.GetUserDataByUsername(username.username);

            if (user == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            //information about the user
            var userJson = new JObject();
            userJson.Add("User_ID", user.Id);
            userJson.Add("FirstName", user.FirstName);
            userJson.Add("LastName", user.LastName);
            userJson.Add("Municipality", user.Municipality);
            userJson.Add("MobilePhoneNumber", user.MobilePhoneNumber);
            userJson.Add("PassportNumber", user.PassportNumber);
            userJson.Add("PostalCode", user.PostalCode);
            userJson.Add("PhoneNumber", user.PhoneNumber);
            userJson.Add("EMBG", user.EMBG);
            userJson.Add("DateOfBirth", user.DateOfBirth);
            userJson.Add("Gender", user.Gender);
            userJson.Add("City", user.City);
            userJson.Add("Address", user.Address);
            userJson.Add("Email", user.Email);
            userJson.Add("Points", user.Points);
            if (user.aspnetroles.FirstOrDefault() != null)
            {
                var roleName = user.aspnetroles.FirstOrDefault().Name;
                userJson.Add("role", roleName);
            }
            data.Add("user", userJson);

            JArray userPolicies = new JArray();
            var policies = _sps.GetSavaPoliciesForUser(user.EMBG);
            foreach (var policy in policies)
            {
                var userPolicy = new JObject();
                userPolicy.Add("SSN_insured", policy.SSN_insured);
                userPolicy.Add("SSN_policyHolder", policy.SSN_policyHolder);
                userPolicy.Add("policy_number", policy.policy_number);
                userPolicy.Add("premium", policy.premium);
                userPolicy.Add("discount_points", policy.discount_points);
                userPolicy.Add("email_seller", policy.email_seller);
                userPolicy.Add("expiry_date", policy.expiry_date);
                userPolicies.Add(userPolicy);
            }
            data.Add("policies", userPolicies);
            return data;
        }

        [HttpPost]
        [Route("ok_setup")]
        public IHttpActionResult OK_Setup(OK_SETUP ok_s)
        {
            if (ok_s == null) return BadRequest();
            if (String.IsNullOrEmpty(ok_s.InsuranceCompany)) return BadRequest();

            var data = _oss.GetLastByInsuranceCompany(ok_s.InsuranceCompany);
            if (ok_s.Version == null)
            {
                return Json(data);
            }
            else if (ok_s.Version == data.VersionNumber)
            {
                return Ok();
            }
            return Json(data);
        }


        [HttpPost]
        [Route("sava_setup")]
        public IHttpActionResult Sava_Setup(OK_SETUP savaSetup)
        {
            var savaSetups = _sss.GetAllSavaSetups();
            var lastSavaSetup = savaSetups.Any() ? savaSetups.Last() : null;
            if (lastSavaSetup == null)
                return Ok(new {message = "No data for the sava setup"});
            if (savaSetup == null)
            {
                return Json(lastSavaSetup);
            }
            else if (savaSetup.Version == lastSavaSetup.version)
            {
                return Ok();
            }
            return Json(lastSavaSetup);
        }

        [HttpPost]
        [Route("ReportDetailLoss")]
        public JObject ReportDetailLoss(DetailFirstNoticeOfLossViewModel addLoss)
        {
            if (addLoss == null)
            {
                throw new Exception("Internal error: Empty Loss");
            }
            var policy = _ps.GetPolicyIdByPolicyNumber(addLoss.Policy_Number);
            if (policy == null)
                throw new Exception("Policy not found");

            var idFNOL = SaveFirstNoticeOfLossHelper.SaveDetailFirstNoticeOdLoss(addLoss, policy, _fis, _ais, _bas);
            if (idFNOL != -1)
            {
                JObject data = new JObject();
                data.Add("Id", idFNOL);
                return data;
            }
            else throw new Exception("Internal error: Not saved");
        }

        [HttpPost]
        [Route("CreateQuote")]
        public IHttpActionResult CreateQuote(AddPolicyMobileViewModel addPolicy)
        {
            if (addPolicy == null)
            {
                throw new Exception("Internal error: Empty Policy");
            }
            int result = SavePolicyHelper.SavePolicyFromMobile(addPolicy, _ps, _us, _iss, _pis, _acs);
            if (result != -1)
                return Ok(new { QuoteID = result });
            else throw new Exception("Internal error: Not saved");
        }

        //If not used from mobile app, delete it
        [HttpPost]
        [AllowAnonymous]
        [Route("ExistsPolicy")]
        public IHttpActionResult ExistsPolicy(Policy policy)
        {
            if (policy.Policy_Number == null || policy.Policy_Number == "")
            {
                throw new Exception("Internal error: Empty Input");
            }
            try
            {
                var result = _ps.GetPolicyIdByPolicyNumber(policy.Policy_Number);
                if (result == null)
                {
                    var myCustomMessage = "Policy/Quote doesn't exists";
                    return ResponseMessage(
                        Request.CreateResponse(
                            HttpStatusCode.NotFound,
                            myCustomMessage
                        ));


                }
                else return ResponseMessage(
                       Request.CreateResponse(
                           HttpStatusCode.Found,
                           "Found " + policy.Policy_Number
                       ));

            }
            catch
            {
                throw new Exception("Internal error");
            }

        }

        [HttpPost]
        [Route("CreatePolicy")]
        public IHttpActionResult CreatePolicy(Policy policy)
        {
            if (policy.Policy_Number == null || policy.Policy_Number == "")
            {
                throw new Exception("Internal error: Empty Policy");
            }
            try
            {
                _ps.UpdatePaymentStatus(policy.Policy_Number);
                return Ok();
            }
            catch
            {
                throw new Exception("Internal error: Payment status not changed");
            }

        }

        [HttpPost]
        [Route("GetDefaultData")]
        public JObject GetDefaultData()
        {
            JObject data = new JObject();

            //additional_charge
            JArray additionalCharges = new JArray();
            var allCharges = _acs.GetAllAdditionalCharge();
            foreach (var charge in allCharges)
            {
                JObject additionalCharge = new JObject();
                additionalCharge.Add("ID", charge.ID);
                additionalCharge.Add("Percentage", charge.Percentage);
                additionalCharge.Add("Version", charge.Version);
                var chargeDataEN = _acs.GetAdditionalChargeENData(charge.ID);
                if (chargeDataEN != null)
                {
                    additionalCharge.Add("NameEN", chargeDataEN.name);
                }

                var chargeDataMK = _acs.GetAdditionalChargeMKData(charge.ID);
                if (chargeDataMK != null)
                {
                    additionalCharge.Add("NameMK", chargeDataMK.name);
                }
                additionalCharges.Add(additionalCharge);
            }
            data.Add("additional_charges", additionalCharges);

            //banks
            JArray banks = new JArray();
            var allBanks = _bas.GetAllBanks();
            foreach (var bank in allBanks)
            {
                JObject bankObject = new JObject();
                bankObject.Add("ID", bank.ID);
                bankObject.Add("Name", bank.Name);
                banks.Add(bankObject);
            }
            data.Add("banks", banks);

            //banks prefix
            JArray banksPrefix = new JArray();
            var allPrefixs = _bas.GetAllPrefix();
            foreach (var prefix in allPrefixs)
            {
                JObject prefixObject = new JObject();
                prefixObject.Add("Bank_ID", prefix.Bank_ID);
                prefixObject.Add("Prefix_Number", prefix.Prefix_Number);
                banksPrefix.Add(prefixObject);
            }
            data.Add("banksPrefix", banksPrefix);

            //travel number
            JArray travelNumber = new JArray();
            var allTravelNumber = _tn.GetAllTravelNumbers();
            foreach (var number in allTravelNumber)
            {
                JObject numberObject = new JObject();
                numberObject.Add("ID", number.ID);
                numberObject.Add("Name", number.Name);
                travelNumber.Add(numberObject);
            }
            data.Add("travelNumber", travelNumber);

            //countries
            JArray countries = new JArray();
            var allCountries = _coun.GetAllCountries();
            foreach (var country in allCountries)
            {
                JObject countryObject = new JObject();
                countryObject.Add("ID", country.ID);
                var countryDataEN = _coun.GetCountriesENData(country.ID);
                if (countryDataEN != null)
                {
                    countryObject.Add("NameEN", countryDataEN.name);
                }

                var countryDataMK = _coun.GetACountriesMKData(country.ID);
                if (countryDataMK != null)
                {
                    countryObject.Add("NameMK", countryDataMK.name);
                }
                countries.Add(countryObject);
            }
            data.Add("countries", countries);

            //exchange rate
            JArray exchangeRates = new JArray();
            var allRates = _exch.GetAllExchangeRates();
            foreach (var rate in allRates)
            {
                JObject rateObject = new JObject();
                rateObject.Add("ID", rate.ID);
                rateObject.Add("Currency", rate.Currency);
                rateObject.Add("Value", rate.Value);
                rateObject.Add("Version", rate.Version);
                exchangeRates.Add(rateObject);
            }
            data.Add("exchangeRates", exchangeRates);

            //policy Types
            JArray policyTypes = new JArray();
            var allPolicyTypes = _pts.GetAllPolicyType();
            foreach (var policyType in allPolicyTypes)
            {
                JObject policyObject = new JObject();
                policyObject.Add("ID", policyType.ID);
                policyObject.Add("Name", policyType.type);
                policyTypes.Add(policyObject);
            }
            data.Add("policyTypes", policyTypes);

            //Retaining Risks
            JArray retainingRisks = new JArray();
            var allRetainingRisks = _fran.GetAllRetainingRisks();
            foreach (var risk in allRetainingRisks)
            {
                JObject riskObject = new JObject();
                riskObject.Add("ID", risk.ID);
                var riskDataEN = _fran.GetRetainingRiskENData(risk.ID);
                if (riskDataEN != null)
                {
                    riskObject.Add("NameEN", riskDataEN.name);
                }

                var riskDataMK = _fran.GetRetainingRiskMKData(risk.ID);
                if (riskDataMK != null)
                {
                    riskObject.Add("NameMK", riskDataMK.name);
                }
                if(!(riskDataMK == null || riskDataEN == null))
                    retainingRisks.Add(riskObject);
            }
            data.Add("retainingRisk", retainingRisks);
            return data;
        }

        [HttpPost]
        [Route("SaveInvoices/{id}")]
        public async Task<IHttpActionResult> SaveInvoices(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(@"~/DocumentsFirstNoticeOfLoss/Invoices/" + fileName), buffer);

                    var document = new document();
                    document.Name = fileName;
                    var documentID = _fis.AddDocument(document);
                    _fis.AddInvoice(documentID);
                    _fis.AddDocumentToFirstNoticeOfLoss(documentID, id);
                }
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception("Internal error: Not saved");
            }
        }

        [HttpPost]
        [Route("SaveHealthDocuments/{id}")]
        public async Task<IHttpActionResult> SaveHealthDocuments(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(@"~/DocumentsFirstNoticeOfLoss/HealthInsurance/" + fileName), buffer);

                    var document = new document();
                    document.Name = fileName;
                    var documentID = _fis.AddDocument(document);
                    _fis.AddDocumentToFirstNoticeOfLoss(documentID, id);
                }
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception("Internal error: Not saved");
            }
        }

        [HttpPost]
        [Route("SaveLuggageDocuments/{id}")]
        public async Task<IHttpActionResult> SaveLuggageDocuments(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(@"~/DocumentsFirstNoticeOfLoss/LuggageInsurance/" + fileName), buffer);

                    var document = new document();
                    document.Name = fileName;
                    var documentID = _fis.AddDocument(document);
                    _fis.AddDocumentToFirstNoticeOfLoss(documentID, id);
                }
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception("Internal error: Not saved");
            }
        }

        [HttpPost]
        [Route("Payment")]
        public async Task<HttpResponseMessage> MobilePayment(CreditCardInfoModel paymentModel)
        {

            ok_setup LastEntry = _os.GetLast();
            if (LastEntry.TestPayment == 1)
            {

                if (paymentModel.OrderId.HasValue)
                {
                    try
                    {
                        string quoteNumber = _ps.GetPolicyNumberByPolicyId(paymentModel.OrderId.Value);
                        _ps.UpdatePaymentStatus(quoteNumber);
                        HttpError myCustomError = new HttpError("File successfuly.") { { "Is3DSecure", false }, { "Response", "{'TRANID':'','PAResSyntaxOK':'false','islemtipi':'Auth','refreshtime':'10','lang':'mk','merchantID':'180000069','amount':'500','sID':'1','ACQBIN':'435742','Ecom_Payment_Card_ExpDate_Year':'20','MaskedPan':'429724***4937','clientIp':'88.85.116.22','iReqDetail':'','okUrl':'https://localhost:44375/api/HalkbankPayment/Handle','md':'429724:B1BFD1386EE5C99F997854210EFE15930334DF46EC90BC7994AB81564537D7CE:4274:##180000069','ProcReturnCode':'99','taksit':'','vendorCode':'','paresTxStatus':'-','Ecom_Payment_Card_ExpDate_Month':'02','storetype':'3D_PAY_HOSTING','iReqCode':'','veresEnrolledStatus':'N','Response':'Approved','mdErrorMsg':'N-status/Not enrolled from Directory Server: http://katmai:8080/mdpayacs/vereq','ErrMsg':'Нарачката е веќе платена','PAResVerified':'false','cavv':'','digest':'digest','failUrl':'https://localhost:44375/api/HalkbankPayment/Handle','cavvAlgorithm':'','xid':'C5BphugnaeXHj26RXrVOyR91QFA=','encoding':'UTF-8','currency':'807','oid':'23011','mdStatus':'2','dsId':'1','eci':'','version':'2.0','clientid':'180000069','txstatus':'N','HASH':'UAMehE7tsfURlS4d8udtWa3m+C4=','rnd':'SIUIAvmeELilibPLVdFW','HASHPARAMS':'clientid:oid:AuthCode:ProcReturnCode:Response:mdStatus:cavv:eci:md:rnd:','HASHPARAMSVAL':'1800000692301199Declined2429724:B1BFD1386EE5C99F997854210EFE15930334DF46EC90BC7994AB81564537D7CE:4274:##180000069SIUIAvmeELilibPLVdFW'}" } } ;
                        return Request.CreateErrorResponse(HttpStatusCode.OK, myCustomError);
                       
                    }
                    catch
                    {
                        throw new Exception("Internal error: Payment status not changed, Connection to Database timeout");
                    }

                }
                else
                {
                    throw new Exception("Internal error: Empty Policy");
                }
            }
            else if (LastEntry.TestPayment == 0)
            {
                try
                {
                    return HalkBankPayment(paymentModel);
                }
                catch
                {
                    throw new Exception("Internal error: Can`t access HalkBank Payment api");
                }
            }
            

            else
            {
                throw new Exception("No valid method for payment.");
            }

        }

        public HttpResponseMessage HalkBankPayment(CreditCardInfoModel paymenyModel)
        {

         
            Uri uri = new Uri(ConfigurationManager.AppSettings["webpage_apiurl"] + "/api/halkbankpayment/pay");
            HttpClient client = new HttpClient();
            client.BaseAddress = uri;
            var mediaType = new MediaTypeHeaderValue("application/json");
            var jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<CreditCardInfoModel>(paymenyModel, jsonFormatter);
            HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
            return responseMessage;
        }
    }
}

