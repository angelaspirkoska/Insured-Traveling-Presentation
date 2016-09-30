﻿using InsuredTraveling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.DI;
using System.Data.Entity;


namespace InsuredTraveling.Controllers
{
    public class ReportLossController : Controller
    {
        private IUserService _us;
        private IPolicyService _ps;
        private IPolicyInsuredService _pis;
        private IInsuredsService _iss;
        private IBankAccountService _bas;
        public ReportLossController(IUserService us, IPolicyService ps, IPolicyInsuredService pis, IInsuredsService iss,
            IBankAccountService bas)
        {
            _us = us;
            _ps = ps;
            _pis = pis;
            _iss = iss;
            _bas = bas;
        }

        public  ActionResult Index()
        {
             ShowUserData();
             return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FNOL f)
        {
            ShowUserData();
    
                if (f.insurance == "Health Insurance")
                {
                    f.HealthInsurance = true;
                    f.LuggageInsurance = false;
                    ModelState.Remove("baggageLossDate");
                    ModelState.Remove("placeBaggageLoss");
                    ModelState.Remove("detailedDescription");
                    ModelState.Remove("placeReported");
                    ModelState.Remove("airportArrivalTime");
                    ModelState.Remove("baggageDropTime");


            }
                else
                {
                    f.LuggageInsurance = true;
                    f.HealthInsurance = false;
                    ModelState.Remove("lossDate");
                    ModelState.Remove("lossTime");
                    ModelState.Remove("placeLoss");
            }
            if (ModelState.IsValid)
            {
                f.ShortDetailed = false;
                
                var uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/api/mobile/ReportLoss");

                var client = new HttpClient { BaseAddress = uri };
                var jsonFormatter = new JsonMediaTypeFormatter();
                HttpContent content = new ObjectContent<FNOL>(f, jsonFormatter);
                HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully reported!";
                    return View();
                }else
                {
                    ViewBag.Message = "Something went wrong!";
                    return View();
                }
            }
            ViewBag.insurance = f.insurance;
            

            //ViewBag.Policies;


            return View(f);
        }

        public void ShowUserData()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
           // var user = _us.GetUserDataByUsername(username);
            var policies = _us.GetPolicyNumberListByUsername(System.Web.HttpContext.Current.User.Identity.Name).ToListAsync();
           
           // await Task.WhenAll(policies);
            ViewBag.Policies = policies.Result;
          //  ViewBag.Name = user.FirstName + " " + user.LastName;
          //  ViewBag.Address = user.Address;
          //  ViewBag.Phone = user.MobilePhoneNumber;
          //  ViewBag.EMBG = user.EMBG;
        }


        [HttpGet]
        public JObject GetInsuredData(int SelectedInsuredId)
        {
            var SelectedInsured = _iss.GetInsuredData(SelectedInsuredId);
            bool ISSameUserAndSelectedInsured = _us.IsSameLoggedUserAndInsured(System.Web.HttpContext.Current.User.Identity.Name, SelectedInsuredId);
            var NewJsonInsured = new JObject();
            NewJsonInsured.Add("Id", SelectedInsured.ID);
            NewJsonInsured.Add("FirstName", SelectedInsured.Name);
            NewJsonInsured.Add("LastName", SelectedInsured.Lastname);
            NewJsonInsured.Add("Adress", SelectedInsured.Address);
            NewJsonInsured.Add("PhoneNumber", SelectedInsured.Phone_Number);           
            NewJsonInsured.Add("SSN", SelectedInsured.SSN);
            NewJsonInsured.Add("IsSameUserAndSelectedInsured", ISSameUserAndSelectedInsured);
            return NewJsonInsured;

        }

        [HttpGet]
        public JObject GetInsureds(int PolicyID)
        {
            var Result = new JObject();

            var Insureds = _pis.GetAllInsuredByPolicyId(PolicyID);
            var PolicyHolder = _ps.GetPolicyHolderByPolicyID(PolicyID);
            var PolicyHolderBankAccounts = _bas.BankAccountsByInsuredId(PolicyHolder.ID);
            var Banks = _bas.GetAllPrefix();
            
          
           
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
                NewJsonInsured.Add("Id", v.ID);               
                NewJsonInsured.Add("FirstName", v.Name);
                NewJsonInsured.Add("LastName", v.Lastname);    
                           
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
                 
            

        

    }
}