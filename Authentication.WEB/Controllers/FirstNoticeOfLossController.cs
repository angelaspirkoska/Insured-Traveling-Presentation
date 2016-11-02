using InsuredTraveling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.DI;
using System.Data.Entity;
using System.Collections.Generic;
using InsuredTraveling.Helpers;
using InsuredTraveling.ViewModels;
using InsuredTraveling.Filters;
using Newtonsoft.Json;
using AutoMapper;
using System.IO;

namespace InsuredTraveling.Controllers
{
    
    [SessionExpire]
  //  [Authorize]
    public class FirstNoticeOfLossController : Controller
    {
        private IUserService _us;
        private IPolicyService _ps;
        private IPolicyInsuredService _pis;
        private IInsuredsService _iss;
        private IFirstNoticeOfLossService _fis;
        private IPolicyTypeService _pts;
        private IAdditionalInfoService _ais;
        private IBankAccountService _bas;
        
        public FirstNoticeOfLossController(IUserService us, 
                                           IPolicyService ps, 
                                           IPolicyInsuredService pis,
                                           IInsuredsService iss, 
                                           IFirstNoticeOfLossService fis,
                                           IBankAccountService bas, 
                                           IPolicyTypeService pts, 
                                           IAdditionalInfoService ais)
        {
            _us = us;
            _ps = ps;
            _pis = pis;
            _iss = iss;
            _bas = bas;
            _pts = pts;
            _ais = ais;
            _fis = fis;
        }

        [SessionExpire]
        public ActionResult Index(int? policyId)
        {           
            var policies = ShowUserData();
            ViewBag.Date = DateTime.Now.Year + "-"+DateTime.Now.Month + "-" +DateTime.Now.Day;
            ViewBag.Policies = new SelectList(policies, "Value", "Text", policyId.ToString());
            ViewBag.PolicyId = policyId != null ? policyId.ToString() : "0";
            return View();
        }

        [SessionExpire]
        [HttpPost]
        public async Task<ActionResult> Index(FirstNoticeOfLossReportViewModel firstNoticeOfLossViewModel, IEnumerable<HttpPostedFileBase> invoices, IEnumerable<HttpPostedFileBase> documentsHealth, IEnumerable<HttpPostedFileBase> documentsLuggage)
        {
            var policies = ShowUserData();
            ViewBag.Policies = new SelectList(policies, "Value", "Text", firstNoticeOfLossViewModel.PolicyId.ToString());
            //firstNoticeOfLossViewModel.isMobile = false;
            ModelState.Remove("PolicyHolderForeignBankAccountId");
            ModelState.Remove("ClaimantForeignBankAccountId");


            if (firstNoticeOfLossViewModel.IsHealthInsurance)
            {
                ModelState.Remove("AccidentDateTimeLuggage");
                ModelState.Remove("Floaters");
                ModelState.Remove("FloatersValue");
                ModelState.Remove("AccidentPlaceLuggage");
                ModelState.Remove("PlaceDescription");
                ModelState.Remove("DetailDescription");
                ModelState.Remove("ReportPlace");
                ModelState.Remove("AccidentTimeLuggage");
                ModelState.Remove("LugaggeCheckingTime");
                ModelState.Remove("ArriveTime");
                ModelState.Remove("Invoices");
                ViewBag.insurance = "Health Insurance"; 
            }
            else
            {
                ModelState.Remove("AccidentDateTimeHealth");
                ModelState.Remove("AccidentTimeHealth");
                ModelState.Remove("AccidentPlaceHealth");
                ModelState.Remove("DoctorVisitDateTime");
                ModelState.Remove("DoctorInfo");
                ModelState.Remove("ArriveTime");
                ModelState.Remove("Invoices");
                ViewBag.insurance = "Luggage Insurance";
            }

            if (ModelState.IsValid)
            {
               
               // var result = SaveDataInDb(firstNoticeOfLossViewModel, invoices, documentsHealth, documentsLuggage);

                var result = SaveFirstNoticeOfLossHelper.SaveFirstNoticeOfLoss( _iss, _us, _fis,
            _bas,  _pts, _ais, firstNoticeOfLossViewModel, invoices, documentsHealth, documentsLuggage);
                //var uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/api/mobile/ReportLoss");
                //var client = new HttpClient { BaseAddress = uri };
                //var jsonFormatter = new JsonMediaTypeFormatter();
                //HttpContent content = new ObjectContent<FirstNoticeOfLossReportViewModel>(firstNoticeOfLossViewModel,
                //    jsonFormatter);
                //HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                //string responseBody = await responseMessage.Content.ReadAsStringAsync();

                //firstNoticeOfLossViewModel.ShortDetailed = false;

                if (result>0)
                {
                    ViewBag.Message = "Successfully reported!";
                    return RedirectToAction("View", new { id = result });
                   // return View("View", result);
                }
                else
                {
                    ViewBag.Message = "Something went wrong!";
                    return View();
                }
            }
            return View(firstNoticeOfLossViewModel);
        }

        [SessionExpire]
        public ActionResult View(int? id)
        {
            var model = new FirstNoticeOfLossReportViewModel();
            if(id != null)
            {
                var data = _fis.GetById(Convert.ToInt32(id));
                model = Mapper.Map<first_notice_of_loss, FirstNoticeOfLossReportViewModel>(data);
                model.Invoices = new List<FileDescriptionViewModel>();
                model.InsuranceInfoDoc = new List<FileDescriptionViewModel>();

                var allInvoices = _fis.GetInvoiceDocumentName(model.Id);
                foreach (var invoice in allInvoices)
                {
                    var file = new FileDescriptionViewModel();
                    file.FileName = invoice;
                    file.FilePath = "~/DocumentsFirstNoticeOfLoss/Invoices/" + file.FileName;
                    model.Invoices.Add(file);
                }

                var isHealthInsurance = _fis.IsHealthInsuranceByAdditionalInfoId(data.Additional_infoID);
                var allDoc = _fis.GetHealthLuggageDocumentName(model.Id);
                foreach (var doc in allDoc)
                {
                    var file = new FileDescriptionViewModel();
                    file.FileName = doc;
                    file.FilePath = isHealthInsurance ? "~/DocumentsFirstNoticeOfLoss/HealthInsurance/" + file.FileName : "~/DocumentsFirstNoticeOfLoss/LuggageInsurance/" + file.FileName;
                    model.InsuranceInfoDoc.Add(file);
                }

            }
            return View(model);
        }

        [SessionExpire]
        public FileResult DocumentDownload(string path)
        {
            try
            {
                string fileName = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                if (System.IO.File.Exists(Server.MapPath(path)))
                {
                    string contentType = "application/" + Path.GetExtension(path).Replace(".", String.Empty);

                    return File(path, contentType, fileName);
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }
        [SessionExpire]
        private bool SaveDataInDb(FirstNoticeOfLossReportViewModel firstNoticeOfLossViewModel, IEnumerable<HttpPostedFileBase> invoices, IEnumerable<HttpPostedFileBase> documentsHealth, IEnumerable<HttpPostedFileBase> documentsLuggage)
        {
            var result = true;
            var additionalInfo = _ais.Create();
            if (firstNoticeOfLossViewModel.IsHealthInsurance)
            {
                additionalInfo.Accident_place = firstNoticeOfLossViewModel.AccidentPlaceHealth;
                if (firstNoticeOfLossViewModel.AccidentDateTimeHealth != null)
                    additionalInfo.Datetime_accident = firstNoticeOfLossViewModel.AccidentDateTimeHealth.Value;

                var healthInsuranceInfo = new health_insurance_info
                {
                    Additional_infoId = _ais.Add(additionalInfo),
                    additional_info = additionalInfo,
                    Datetime_doctor_visit = firstNoticeOfLossViewModel.DoctorVisitDateTime,
                    Doctor_info = firstNoticeOfLossViewModel.DoctorInfo,
                    Medical_case_description = firstNoticeOfLossViewModel.MedicalCaseDescription,
                    Previous_medical_history = firstNoticeOfLossViewModel.PreviousMedicalHistory,
                    Responsible_institution = firstNoticeOfLossViewModel.ResponsibleInstitution
                };

                try
                {

                    result = _ais.AddHealthInsuranceInfo(healthInsuranceInfo) > 0;

                }
                finally
                {

                }

            }

            else
            {
                additionalInfo.Accident_place = firstNoticeOfLossViewModel.AccidentPlaceLuggage;

                if (firstNoticeOfLossViewModel.AccidentDateTimeLuggage != null)
                    additionalInfo.Datetime_accident = firstNoticeOfLossViewModel.AccidentDateTimeLuggage.Value;
                var luggageInsuranceInfo = new luggage_insurance_info
                {
                    Additional_infoId = _ais.Add(additionalInfo),
                    additional_info = additionalInfo,
                    Place_description = firstNoticeOfLossViewModel.PlaceDescription,
                    Detail_description = firstNoticeOfLossViewModel.DetailDescription,
                    Report_place = firstNoticeOfLossViewModel.ReportPlace,
                    Floaters = firstNoticeOfLossViewModel.Floaters,
                    Floaters_value = float.Parse(firstNoticeOfLossViewModel.FloatersValue),
                    Luggage_checking_Time = firstNoticeOfLossViewModel.LugaggeCheckingTime ?? new TimeSpan(0, 0, 0)
                };

                try
                {
                    _ais.AddLuggageInsuranceInfo(luggageInsuranceInfo);
                }
                finally
                {

                }
            }



            var firstNoticeOfLossEntity = _fis.Create();
            firstNoticeOfLossEntity.PolicyId = firstNoticeOfLossViewModel.PolicyId;
            firstNoticeOfLossEntity.ClaimantId = firstNoticeOfLossViewModel.ClaimantId;
            firstNoticeOfLossEntity.Relation_claimant_policy_holder = firstNoticeOfLossViewModel.RelationClaimantPolicyHolder;
            firstNoticeOfLossEntity.Destination = firstNoticeOfLossViewModel.Destination;
            firstNoticeOfLossEntity.Depart_Date_Time = firstNoticeOfLossViewModel.DepartDateTime;
            firstNoticeOfLossEntity.Arrival_Date_Time = firstNoticeOfLossViewModel.ArrivalDateTime;
            firstNoticeOfLossEntity.Transport_means = firstNoticeOfLossViewModel.TransportMeans;
            firstNoticeOfLossEntity.Total_cost = firstNoticeOfLossViewModel.TotalCost;
            firstNoticeOfLossEntity.CreatedDateTime = DateTime.Now;
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            firstNoticeOfLossEntity.CreatedBy = _us.GetUserIdByUsername(username);
            firstNoticeOfLossEntity.Message = "";



            firstNoticeOfLossEntity.Additional_infoID = additionalInfo.ID;

            firstNoticeOfLossEntity.PolicyId = firstNoticeOfLossViewModel.PolicyId;
            firstNoticeOfLossEntity.ClaimantId = firstNoticeOfLossEntity.ClaimantId;
            firstNoticeOfLossEntity.Relation_claimant_policy_holder = firstNoticeOfLossEntity.Relation_claimant_policy_holder;

            if (firstNoticeOfLossViewModel.ClaimantExistentBankAccount)
            {
                firstNoticeOfLossEntity.Claimant_bank_accountID = firstNoticeOfLossViewModel.ClaimantForeignBankAccountId;
            }
            else
            {
                var bankAccountId = SaveBankAccountInfoHelper.SaveBankAccountInfo(_bas, firstNoticeOfLossViewModel.ClaimantId,
                     firstNoticeOfLossViewModel.ClaimantBankName,
                     firstNoticeOfLossViewModel.ClaimantBankAccountNumber);

                firstNoticeOfLossEntity.Claimant_bank_accountID = bankAccountId;
            }



            if (firstNoticeOfLossViewModel.PolicyHolderExistentBankAccount)
            {
                firstNoticeOfLossEntity.Policy_holder_bank_accountID = firstNoticeOfLossViewModel.PolicyHolderForeignBankAccountId;
            }
            else
            {
                var bankAccountId = SaveBankAccountInfoHelper.SaveBankAccountInfo(_bas, firstNoticeOfLossViewModel.PolicyHolderId,
                     firstNoticeOfLossViewModel.PolicyHolderBankName,
                     firstNoticeOfLossViewModel.PolicyHolderBankAccountNumber);

                firstNoticeOfLossEntity.Policy_holder_bank_accountID = bankAccountId;
            }

            int FirstNoticeOfLossID = 0;
            try
            {
                 FirstNoticeOfLossID= _fis.Add(firstNoticeOfLossEntity);

            }
            finally { }

            foreach (var file in invoices)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var path = @"~/DocumentsFirstNoticeOfLoss/Invoices/" + file.FileName;
                    file.SaveAs(Server.MapPath(path));

                    var document = new document();
                    document.Name = file.FileName;
                    var documentID = _fis.AddDocument(document);
                    _fis.AddInvoice(documentID);
                    _fis.AddDocumentToFirstNoticeOfLoss(documentID, FirstNoticeOfLossID);
                }
            }

            foreach (var file in documentsHealth)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var path = @"~/DocumentsFirstNoticeOfLoss/HealthInsurance/" + file.FileName;
                    file.SaveAs(Server.MapPath(path));
                    var document = new document();
                    document.Name = file.FileName;
                    var documentID = _fis.AddDocument(document);
                    _fis.AddDocumentToFirstNoticeOfLoss(documentID, FirstNoticeOfLossID);
                }
            }

            foreach (var file in documentsLuggage)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var path = @"~/DocumentsFirstNoticeOfLoss/LuggageInsurance/" + file.FileName;
                    file.SaveAs(Server.MapPath(path));
                    var document = new document();
                    document.Name = file.FileName;
                    var documentID = _fis.AddDocument(document);
                    _fis.AddDocumentToFirstNoticeOfLoss(documentID, FirstNoticeOfLossID);
                }
            }

            return FirstNoticeOfLossID>0;            
        }
        public List<SelectListItem> ShowUserData()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var policies = _us.GetPolicyNumberListByUsernameToList(System.Web.HttpContext.Current.User.Identity.Name);
            return policies;
        }

        [HttpGet]
        public JObject GetInsuredData(int SelectedInsuredId)
        {
            var NewJsonInsured = new JObject();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var SelectedInsured = _iss.GetInsuredData(SelectedInsuredId);
                bool ISSameUserAndSelectedInsured = _us.IsSameLoggedUserAndInsured(System.Web.HttpContext.Current.User.Identity.Name, SelectedInsuredId);
                var InsuredBankAccounts = _bas.BankAccountsByInsuredId(SelectedInsuredId);

                NewJsonInsured.Add("Id", SelectedInsured.ID);
                NewJsonInsured.Add("FirstName", SelectedInsured.Name);
                NewJsonInsured.Add("LastName", SelectedInsured.Lastname);
                NewJsonInsured.Add("Adress", SelectedInsured.Address);
                NewJsonInsured.Add("PhoneNumber", SelectedInsured.Phone_Number);
                NewJsonInsured.Add("SSN", SelectedInsured.SSN);
                NewJsonInsured.Add("IsSameUserAndSelectedInsured", ISSameUserAndSelectedInsured);

                var BankAccountsInsuredJsonArray = new JArray();

                foreach (var BankAccount in InsuredBankAccounts)
                {
                    var NewJsonBankAccount = new JObject();
                    NewJsonBankAccount.Add("Id", BankAccount.ID);
                    NewJsonBankAccount.Add("AccountNumber", BankAccount.Account_Number);
                    NewJsonBankAccount.Add("BankName", BankAccount.bank.Name);

                    BankAccountsInsuredJsonArray.Add(NewJsonBankAccount);
                }
                NewJsonInsured.Add("BankAccounts", BankAccountsInsuredJsonArray);

            }
            else
            {
                NewJsonInsured.Add("response", "Not authenticated user");
            }


            return NewJsonInsured;

        }

        [HttpGet]
        public async Task<JObject> GetInsureds(int PolicyID)
        {
            var Result = new JObject();
            //var Result = "";


            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var Policy = _ps.GetPolicyClientsInfo(PolicyID);
                var Banks = _bas.GetAllPrefix();

                var Insureds = Policy.policy_insured;
                var PolicyHolder = Policy.insured;
                var PolicyHolderBankAccounts = Policy.insured.bank_account_info;

                var StartDate = Policy.Start_Date;
                var EndDate = Policy.End_Date;


                Result.Add("StartDate",  StartDate.Year+ String.Format("-{0:00}-{0:00}", StartDate.Month , StartDate.Day));
                Result.Add("EndDate", EndDate.Year + String.Format("-{0:00}-{0:00}", EndDate.Month, EndDate.Day));
                var PolicyHolderData = new JObject();
                PolicyHolderData.Add("Id", PolicyHolder.ID);
                PolicyHolderData.Add("FirstName", PolicyHolder.Name);
                PolicyHolderData.Add("LastName", PolicyHolder.Lastname);
                PolicyHolderData.Add("SSN", PolicyHolder.SSN);
                PolicyHolderData.Add("PhoneNumber", PolicyHolder.Phone_Number);
                PolicyHolderData.Add("City", PolicyHolder.City);
                PolicyHolderData.Add("Adress", PolicyHolder.Address);

                var BankAccountsPolicyHolderJsonArray = new JArray();

                foreach (var BankAccount in PolicyHolderBankAccounts)
                {
                    var NewJsonBankAccount = new JObject();
                    NewJsonBankAccount.Add("Id", BankAccount.ID);
                    NewJsonBankAccount.Add("AccountNumber", BankAccount.Account_Number);
                    NewJsonBankAccount.Add("BankName", BankAccount.bank.Name);

                    BankAccountsPolicyHolderJsonArray.Add(NewJsonBankAccount);
                }
                PolicyHolderData.Add("BankAccounts", BankAccountsPolicyHolderJsonArray);

                Result.Add("policyholder", PolicyHolderData);


                var InsuredsJsonArray = new JArray();

                foreach (var v in Insureds)
                {
                    var NewJsonInsured = new JObject();
                    NewJsonInsured.Add("Id", v.insured.ID);
                    NewJsonInsured.Add("FirstName", v.insured.Name);
                    NewJsonInsured.Add("LastName", v.insured.Lastname);

                    InsuredsJsonArray.Add(NewJsonInsured);
                }


                Result.Add("data", InsuredsJsonArray);



                var BankListData = new JArray();
                foreach (var Bank in Banks)
                {
                    var BanksData = new JObject();
                    BanksData.Add("Prefix", Bank.Prefix_Number);
                    BanksData.Add("BankName", Bank.bank.Name);
                    BankListData.Add(BanksData);
                }
                Result.Add("banks", BankListData);

                


                return Result;


            }
            else
            {
                Result.Add("response", "Not authenticated user");
            }

            return Result;
            ;


        }

    }
}