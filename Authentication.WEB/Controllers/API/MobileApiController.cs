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
                                   IOkSetupService os)
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
            data.Add("user", userJson);

            //user's policies
            JArray userPolicies = new JArray();
            travel_policy[] policies = _ps.GetPolicyByUsernameId(user.Id);
            foreach (travel_policy policy in policies)
            {
                var userPolicy = new JObject();
                userPolicy.Add("ID", policy.ID);
                userPolicy.Add("Policy_Number", policy.Policy_Number);
                userPolicy.Add("Exchange_RateID", policy.Exchange_RateID);
                userPolicy.Add("CountryID", policy.CountryID);
                userPolicy.Add("Policy_TypeID", policy.Policy_TypeID);
                userPolicy.Add("Retaining_RiskID", policy.Retaining_RiskID);
                userPolicy.Add("Start_Date", policy.Start_Date);
                userPolicy.Add("End_Date", policy.End_Date);
                userPolicy.Add("Valid_Days", policy.Valid_Days);
                userPolicy.Add("Travel_NumberID", policy.Travel_NumberID);
                userPolicy.Add("Travel_Insurance_TypeID", policy.Travel_Insurance_TypeID);
                userPolicy.Add("Created_By", policy.Created_By);
                userPolicy.Add("Date_Created", policy.Date_Created);
                userPolicy.Add("Date_Modified", policy.Date_Modified);
                userPolicy.Add("Modified_By", policy.Modified_By);
                userPolicy.Add("Payment_Status", policy.Modified_By);
                userPolicy.Add("Date_Cancellation", policy.Modified_By);

                //information about policy holder
                if (policy.insured != null)
                {
                    userPolicy.Add("Policy_HolderID", policy.Policy_HolderID);
                    userPolicy.Add("Name", policy.insured.Name);
                    userPolicy.Add("Lastname", policy.insured.Lastname);
                    userPolicy.Add("SSN", policy.insured.SSN);
                    userPolicy.Add("DateBirth", policy.insured.DateBirth);
                    userPolicy.Add("Age", policy.insured.Age);
                    userPolicy.Add("Email", policy.insured.Email);
                    userPolicy.Add("Phone_Number", policy.insured.Phone_Number);
                    userPolicy.Add("City", policy.insured.City);
                    userPolicy.Add("Postal_Code", policy.insured.Postal_Code);
                    userPolicy.Add("Address", policy.insured.Address);
                    userPolicy.Add("Passport_Number_IdNumber", policy.insured.Passport_Number_IdNumber);
                    userPolicy.Add("Type_InsuredID", policy.insured.Type_InsuredID);

                    //bank account for the policy holder
                    var ssn = _us.GetUserSsnByUsername(username.username);
                    var holderDetails = _iss.GetInsuredDataBySsn(ssn);
                    if (holderDetails != null)
                    {
                        JArray bankAccounts = new JArray();
                        var banks = _bas.BankAccountsByInsuredId(holderDetails.ID);
                        foreach (bank_account_info bankAccount in banks)
                        {
                            var bankAccountObject = new JObject();
                            bankAccountObject.Add("BankAccount", bankAccount.Account_Number);
                            bankAccountObject.Add("BankName", bankAccount.bank.Name);
                            bankAccountObject.Add("BankAccountId", bankAccount.ID);
                            bankAccounts.Add(bankAccountObject);
                        }
                        userPolicy.Add("policyHolderBankAccounts", bankAccounts);
                    }
                }
                //information about insureds
                if (policy.policy_insured != null)
                {
                    JArray policyInsureds = new JArray();
                    foreach (var insured in policy.policy_insured)
                    {
                        var policyInsured = new JObject();
                        policyInsured.Add("Insured_ID", policy.Policy_HolderID);
                        policyInsured.Add("Name", policy.insured.Name);
                        policyInsured.Add("Lastname", policy.insured.Lastname);
                        policyInsured.Add("SSN", policy.insured.SSN);
                        policyInsured.Add("DateBirth", policy.insured.DateBirth);
                        policyInsured.Add("Age", policy.insured.Age);
                        policyInsured.Add("Email", policy.insured.Email);
                        policyInsured.Add("Phone_Number", policy.insured.Phone_Number);
                        policyInsured.Add("City", policy.insured.City);
                        policyInsured.Add("Postal_Code", policy.insured.Postal_Code);
                        policyInsured.Add("Address", policy.insured.Address);
                        policyInsured.Add("Passport_Number_IdNumber", policy.insured.Passport_Number_IdNumber);
                        policyInsured.Add("Type_InsuredID", policy.insured.Type_InsuredID);

                        JArray bankAccounts = new JArray();
                        var banks = _bas.BankAccountsByInsuredId(insured.InsuredID);
                        foreach (bank_account_info bankAccount in banks)
                        {
                            var bankAccountObject = new JObject();
                            bankAccountObject.Add("BankAccount", bankAccount.Account_Number);
                            bankAccountObject.Add("BankName", bankAccount.bank.Name);
                            bankAccountObject.Add("BankAccountId", bankAccount.ID);
                            bankAccounts.Add(bankAccountObject);
                        }
                        policyInsured.Add("insuredBankAccounts", bankAccounts);
                        policyInsureds.Add(policyInsured);
                    }

                    userPolicy.Add("insureds", policyInsureds);
                }
                userPolicies.Add(userPolicy);
            }
            data.Add("policy", userPolicies);

            // user's quotes
            JArray userQuotes = new JArray();
            travel_policy[] quotes = _ps.GetPolicyNotPayedByUsernameId(user.Id);
            foreach (travel_policy policy in quotes)
            {
                var userQuote = new JObject();
                userQuote.Add("Policy_Number", policy.Policy_Number);
                userQuote.Add("Exchange_RateID", policy.Exchange_RateID);
                userQuote.Add("CountryID", policy.CountryID);
                userQuote.Add("Policy_TypeID", policy.Policy_TypeID);
                userQuote.Add("Retaining_RiskID", policy.Retaining_RiskID);
                userQuote.Add("Start_Date", policy.Start_Date);
                userQuote.Add("End_Date", policy.End_Date);
                userQuote.Add("Valid_Days", policy.Valid_Days);
                userQuote.Add("Travel_NumberID", policy.Travel_NumberID);
                userQuote.Add("Group_Members", policy.Group_Members);
                userQuote.Add("Group_Total_Premium", policy.Group_Total_Premium);
                userQuote.Add("Created_By", policy.Created_By);
                userQuote.Add("Date_Created", policy.Date_Created);
                userQuote.Add("Date_Modified", policy.Date_Modified);
                userQuote.Add("Modified_By", policy.Modified_By);
                userQuote.Add("Payment_Status", policy.Modified_By);
                userQuote.Add("Date_Cancellation", policy.Modified_By);

                //information about policy holder
                if (policy.insured != null)
                {
                    userQuote.Add("Policy_HolderID", policy.Policy_HolderID);
                    userQuote.Add("Name", policy.insured.Name);
                    userQuote.Add("Lastname", policy.insured.Lastname);
                    userQuote.Add("SSN", policy.insured.SSN);
                    userQuote.Add("DateBirth", policy.insured.DateBirth);
                    userQuote.Add("Age", policy.insured.Age);
                    userQuote.Add("Email", policy.insured.Email);
                    userQuote.Add("Phone_Number", policy.insured.Phone_Number);
                    userQuote.Add("City", policy.insured.City);
                    userQuote.Add("Postal_Code", policy.insured.Postal_Code);
                    userQuote.Add("Address", policy.insured.Address);
                    userQuote.Add("Passport_Number_IdNumber", policy.insured.Passport_Number_IdNumber);
                    userQuote.Add("Type_InsuredID", policy.insured.Type_InsuredID);

                    //bank account for the policy holder
                    var ssn = _us.GetUserSsnByUsername(username.username);
                    var holderDetails = _iss.GetInsuredDataBySsn(ssn);
                    if (holderDetails != null)
                    {
                        JArray bankAccounts = new JArray();
                        var banks = _bas.BankAccountsByInsuredId(holderDetails.ID);
                        foreach (bank_account_info bankAccount in banks)
                        {
                            var bankAccountObject = new JObject();
                            bankAccountObject.Add("BankAccount", bankAccount.Account_Number);
                            bankAccountObject.Add("BankName", bankAccount.bank.Name);
                            bankAccountObject.Add("BankAccountId", bankAccount.ID);
                            bankAccounts.Add(bankAccountObject);
                        }
                        userQuote.Add("policyHolderBankAccounts", bankAccounts);
                    }
                }
                //information about insureds
                if (policy.policy_insured != null)
                {
                    JArray quoteInsureds = new JArray();
                    foreach (var insured in policy.policy_insured)
                    {
                        var quoteInsured = new JObject();
                        quoteInsured.Add("ID", policy.Policy_HolderID);
                        quoteInsured.Add("Name", policy.insured.Name);
                        quoteInsured.Add("Lastname", policy.insured.Lastname);
                        quoteInsured.Add("SSN", policy.insured.SSN);
                        quoteInsured.Add("DateBirth", policy.insured.DateBirth);
                        quoteInsured.Add("Age", policy.insured.Age);
                        quoteInsured.Add("Email", policy.insured.Email);
                        quoteInsured.Add("Phone_Number", policy.insured.Phone_Number);
                        quoteInsured.Add("City", policy.insured.City);
                        quoteInsured.Add("Postal_Code", policy.insured.Postal_Code);
                        quoteInsured.Add("Address", policy.insured.Address);
                        quoteInsured.Add("Passport_Number_IdNumber", policy.insured.Passport_Number_IdNumber);
                        quoteInsured.Add("Type_InsuredID", policy.insured.Type_InsuredID);

                        JArray bankAccounts = new JArray();
                        var banks = _bas.BankAccountsByInsuredId(insured.InsuredID);
                        foreach (bank_account_info bankAccount in banks)
                        {
                            var bankAccountObject = new JObject();
                            bankAccountObject.Add("BankAccount", bankAccount.Account_Number);
                            bankAccountObject.Add("BankName", bankAccount.bank.Name);
                            bankAccountObject.Add("BankAccountId", bankAccount.ID);
                            bankAccounts.Add(bankAccountObject);
                        }
                        quoteInsured.Add("insuredBankAccounts", bankAccounts);
                        quoteInsureds.Add(quoteInsured);
                    }

                    userQuote.Add("insureds", quoteInsureds);
                }
                userQuotes.Add(userQuote);
            }

            data.Add("quote", userQuotes);

            //user's reports of loss
            JArray userFNOL = new JArray();
            first_notice_of_loss[] fnols = _fnls.GetByInsuredUserId(user.Id);

            foreach (first_notice_of_loss fnol in fnols)
            {
                var fnolObject = new JObject();
                if (fnol.Short_Detailed == true)
                {
                    fnolObject.Add("ChatId", fnol.ChatId);
                    fnolObject.Add("ID", fnol.ID);
                    fnolObject.Add("Policy_Number", fnol.travel_policy.Policy_Number);
                    fnolObject.Add("Short_Detailed", fnol.Short_Detailed);
                }
                else
                {
                    fnolObject.Add("ID", fnol.ID);
                    fnolObject.Add("PolicyId", fnol.PolicyId);
                    fnolObject.Add("Policy_Number", fnol.travel_policy.Policy_Number);

                    if (fnol.insured != null)
                    {
                        var claimantBankAccount = _bas.BankAccountInfoById(fnol.Claimant_bank_accountID);

                        fnolObject.Add("Claimant_ID", fnol.insured.ID);
                        fnolObject.Add("Name", fnol.insured.Name);
                        fnolObject.Add("Lastname", fnol.insured.Lastname);
                        fnolObject.Add("SSN", fnol.insured.SSN);
                        fnolObject.Add("DateBirth", fnol.insured.DateBirth);
                        fnolObject.Add("Age", fnol.insured.Age);
                        fnolObject.Add("Email", fnol.insured.Email);
                        fnolObject.Add("Phone_Number", fnol.insured.Phone_Number);
                        fnolObject.Add("City", fnol.insured.City);
                        fnolObject.Add("Postal_Code", fnol.insured.Postal_Code);
                        fnolObject.Add("Address", fnol.insured.Address);
                        fnolObject.Add("Passport_Number_IdNumber", fnol.insured.Passport_Number_IdNumber);
                        fnolObject.Add("Type_InsuredID", fnol.insured.Type_InsuredID);
                        fnolObject.Add("Claimant_Account_HolderID", claimantBankAccount.Account_HolderID);
                        fnolObject.Add("Claimant_Account_Number", claimantBankAccount.Account_Number);
                        fnolObject.Add("Claimant_BankID", claimantBankAccount.BankID);
                    }

                    fnolObject.Add("Relation_claimant_policy_holder", fnol.Relation_claimant_policy_holder);
                    fnolObject.Add("Destination", fnol.Destination);
                    fnolObject.Add("Depart_Date_Time", fnol.Depart_Date_Time);
                    fnolObject.Add("Arrival_Date_Time", fnol.Arrival_Date_Time);
                    fnolObject.Add("Transport_means", fnol.Transport_means);
                    fnolObject.Add("Total_cost", fnol.Total_cost);
                    fnolObject.Add("Web_Mobile", fnol.Web_Mobile);
                    fnolObject.Add("CreatedBy", fnol.CreatedBy);

                    if (fnol.Policy_holder_bank_account_info != null)
                    {
                        fnolObject.Add("PolicyHolder_Account_HolderID", fnol.Policy_holder_bank_account_info.Account_HolderID);
                        fnolObject.Add("PolicyHolder_Account_Number", fnol.Policy_holder_bank_account_info.Account_Number);
                        fnolObject.Add("BankID", fnol.Policy_holder_bank_account_info.BankID);
                    }
                    var healthInsurance = _fis.GetHealthAdditionalInfoByLossId(fnol.ID);
                    if (healthInsurance == null)
                    {
                        fnolObject.Add("HealthInsurance_Y_N", "N");
                    }
                    else
                    {
                        fnolObject.Add("HealthInsurance_Y_N", "Y");
                        if (fnol.additional_info != null)
                        {
                            fnolObject.Add("Datetime_accident", fnol.additional_info.Datetime_accident);
                            fnolObject.Add("Accident_place", fnol.additional_info.Accident_place);
                            if (fnol.additional_info.health_insurance_info != null)
                            {
                                fnolObject.Add("Datetime_doctor_visit", fnol.additional_info.health_insurance_info.Datetime_doctor_visit);
                                fnolObject.Add("Doctor_info", fnol.additional_info.health_insurance_info.Doctor_info);
                                fnolObject.Add("Medical_case_description	", fnol.additional_info.health_insurance_info.Medical_case_description);
                                fnolObject.Add("Previous_medical_history", fnol.additional_info.health_insurance_info.Previous_medical_history);
                                fnolObject.Add("Responsible_institution", fnol.additional_info.health_insurance_info.Responsible_institution);
                            }
                        }
                    }

                    var luggageInsurance = _fis.GetLuggageAdditionalInfoByLossId(fnol.ID);
                    if (luggageInsurance == null)
                    {
                        fnolObject.Add("LuggageInsurance_Y_N", "N");
                    }
                    else
                    {
                        fnolObject.Add("LuggageInsurance_Y_N", "Y");
                        if (fnol.additional_info != null)
                        {
                            fnolObject.Add("Datetime_accident", fnol.additional_info.Datetime_accident);
                            fnolObject.Add("Accident_place", fnol.additional_info.Accident_place);
                            if (fnol.additional_info.luggage_insurance_info != null)
                            {
                                fnolObject.Add("Place_description", fnol.additional_info.luggage_insurance_info.Place_description);
                                fnolObject.Add("Detail_description", fnol.additional_info.luggage_insurance_info.Detail_description);
                                fnolObject.Add("Report_place", fnol.additional_info.luggage_insurance_info.Report_place);
                                fnolObject.Add("Floaters", fnol.additional_info.luggage_insurance_info.Floaters);
                                fnolObject.Add("Floaters_value", fnol.additional_info.luggage_insurance_info.Floaters_value);
                                fnolObject.Add("Luggage_checking_Time", fnol.additional_info.luggage_insurance_info.Luggage_checking_Time);
                            }
                        }
                    }
                    JArray invoices = new JArray();
                    var allInvoices = _fis.GetInvoiceDocumentName(fnol.ID);
                    foreach (var invoice in allInvoices)
                    {
                        var invoiceObject = new JObject();
                        invoiceObject.Add("invoicePath", ConfigurationManager.AppSettings["webpage_url"] + "/DocumentsFirstNoticeOfLoss/Invoices/" + invoice);
                        invoices.Add(invoiceObject);
                    }
                    fnolObject.Add("invoices", invoices);

                    JArray documents = new JArray();
                    var allDoc = _fis.GetHealthLuggageDocumentName(fnol.ID);
                    foreach (var doc in allDoc)
                    {
                        var document = new JObject();
                        document.Add("documentPath", healthInsurance != null ? ConfigurationManager.AppSettings["webpage_url"] + "/DocumentsFirstNoticeOfLoss/HealthInsurance/" + doc : ConfigurationManager.AppSettings["webpage_url"] + "/DocumentsFirstNoticeOfLoss/LuggageInsurance/" + doc);
                        documents.Add(document);
                    }

                    fnolObject.Add("documents", documents);

                    userFNOL.Add(fnolObject);
                }
            }
            data.Add("loss", userFNOL);
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
            bool result = SavePolicyHelper.SavePolicyFromMobile(addPolicy, _ps, _us, _iss, _pis, _acs);
            if (result)
                return Ok();
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

                if (paymentModel.OrderId != null || paymentModel.OrderId != "")
                {
                    try
                    {
                        _ps.UpdatePaymentStatus(paymentModel.OrderId);
                        HttpError myCustomError = new HttpError("File successfuly.") { { "Is3DSecure", false }, { "Response", "{'TRANID':'','PAResSyntaxOK':'false','islemtipi':'Auth','refreshtime':'10','lang':'mk','merchantID':'180000069','amount':'500','sID':'1','ACQBIN':'435742','Ecom_Payment_Card_ExpDate_Year':'20','MaskedPan':'429724***4937','clientIp':'88.85.116.22','iReqDetail':'','okUrl':'https://localhost:44375/api/HalkbankPayment/Handle','md':'429724:B1BFD1386EE5C99F997854210EFE15930334DF46EC90BC7994AB81564537D7CE:4274:##180000069','ProcReturnCode':'99','taksit':'','vendorCode':'','paresTxStatus':'-','Ecom_Payment_Card_ExpDate_Month':'02','storetype':'3D_PAY_HOSTING','iReqCode':'','veresEnrolledStatus':'N','Response':'Declined','mdErrorMsg':'N-status/Not enrolled from Directory Server: http://katmai:8080/mdpayacs/vereq','ErrMsg':'Нарачката е веќе платена','PAResVerified':'false','cavv':'','digest':'digest','failUrl':'https://localhost:44375/api/HalkbankPayment/Handle','cavvAlgorithm':'','xid':'C5BphugnaeXHj26RXrVOyR91QFA=','encoding':'UTF-8','currency':'807','oid':'23011','mdStatus':'2','dsId':'1','eci':'','version':'2.0','clientid':'180000069','txstatus':'N','HASH':'UAMehE7tsfURlS4d8udtWa3m+C4=','rnd':'SIUIAvmeELilibPLVdFW','HASHPARAMS':'clientid:oid:AuthCode:ProcReturnCode:Response:mdStatus:cavv:eci:md:rnd:','HASHPARAMSVAL':'1800000692301199Declined2429724:B1BFD1386EE5C99F997854210EFE15930334DF46EC90BC7994AB81564537D7CE:4274:##180000069SIUIAvmeELilibPLVdFW'}" } } ;
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

