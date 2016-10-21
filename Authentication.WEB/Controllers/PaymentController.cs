using Authentication.WEB.Models;
using Authentication.WEB.Services;
using InsuredTraveling;
using InsuredTraveling.Models;
using Rotativa;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using InsuredTraveling.Helpers;
using InsuredTraveling.DI;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("Payment")]
    public class PaymentController : Controller
    {
        private IUserService _us;
        private IPolicyService _ps;
        private IInsuredsService _iss;
        private IPolicyInsuredService _pis;
        private IAdditionalChargesService _acs;

        public PaymentController(IUserService us, IPolicyService ps, IInsuredsService iss, IPolicyInsuredService pis, IAdditionalChargesService acs)
        {
            this._us = us;
            this._ps = ps;
            _iss = iss;
            _pis = pis;
            _acs = acs;
        }

        // GET: Payment
        public ActionResult Index()
        {
            PaymentModel model = new PaymentModel();

            model.clientId = "180000069";                   //Merchant Id defined by bank to user
            model.amount = "9.95";                         //Transaction amount
            model.oid = "";                                //Order Id. Must be unique. If left blank, system will generate a unique one.
            model.okUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentSuccess";                      //URL which client be redirected if authentication is successful
            model.failUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentFail";                    //URL which client be redirected if authentication is not successful
            model.rnd = DateTime.Now.ToString();           //A random number, such as date/time

            model.currency = "807";                        //Currency code, 949 for TL, ISO_4217 standard
            model.instalmentCount = "";                    //Instalment count, if there is no instalment should left blank
            model.transactionType = "Auth";                //Transaction type
            model.storekey = "SKEY3545";                     //Store key value, defined by bank.
            model.hashstr = model.clientId + model.oid + model.amount + model.okUrl + model.failUrl + model.transactionType + model.instalmentCount + model.rnd + model.storekey;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            model.hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(model.hashstr);
            model.inputbytes = sha.ComputeHash(model.hashbytes);


            model.hash = Convert.ToBase64String(model.inputbytes); //Hash value used for validation

            //p.Created_By = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
            //p.Date_Created = DateTime.Now;
            //InsuredTravelingEntity _db = new InsuredTravelingEntity();
            //var p1 = _db.travel_policy.Create();
            //p1 = Mapper.Map<Policy, travel_policy>(p);
            //model.Pat = p1;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Policy p)
        {
            PaymentModel model = new PaymentModel();

            var PolicyId = SavePolicyHelper.SavePolicy(p, _ps, _us, _iss, _pis, _acs);

           var policy = _ps.GetPolicyById(PolicyId);

            model.clientId = "180000069";                   //Merchant Id defined by bank to user
            model.amount = p.Total_Premium.ToString();
             //   "9.95";                         //Transaction amount
            model.oid = "";                                //Order Id. Must be unique. If left blank, system will generate a unique one.
            model.okUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentSuccess";                      //URL which client be redirected if authentication is successful
            model.failUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentFail";                    //URL which client be redirected if authentication is not successful
            model.rnd = DateTime.Now.ToString();           //A random number, such as date/time

            model.currency = "807";                        //Currency code, 949 for TL, ISO_4217 standard
            model.instalmentCount = "";                    //Instalment count, if there is no instalment should left blank
            model.transactionType = "Auth";                //Transaction type
            model.storekey = "SKEY3545";                     //Store key value, defined by bank.
            model.hashstr = model.clientId + model.oid + model.amount + model.okUrl + model.failUrl + model.transactionType + model.instalmentCount + model.rnd + model.storekey;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            model.hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(model.hashstr);
            model.inputbytes = sha.ComputeHash(model.hashbytes);


            model.hash = Convert.ToBase64String(model.inputbytes); //Hash value used for validation


            model.Pat = policy;

            //viewbag da dodam!!!!!!!!!


            return View(model);
        }










        [Route("PaymentSuccess")]
        public ActionResult PaymentSuccess()
        {
            PaymentSuccessModel model = new PaymentSuccessModel();
            model.amount = Request.Form.Get("amount");
            model.oid = Request.Form.Get("ReturnOid");
            model.TransId = Request.Form.Get("TransId");
            model.AuthCode = Request.Form.Get("AuthCode");
            model.mdStatus = Request.Form.Get("mdStatus");
            if (model.mdStatus == "1" || model.mdStatus == "2" || model.mdStatus == "3" || model.mdStatus == "4")
            {
                string fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PolicyPDF/" + model.TransId + model.amount + ".pdf");
                var actionResult = new ActionAsPdf("../Policy/Print");
                var byteArray = actionResult.BuildPdf(ControllerContext);
                var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();



                // ADD MAIL ADRESS

                MailService mailService = new MailService("aleksandra@optimalreinsurance.com");
                mailService.setSubject("Издадена полиса број: " + model.oid);
                string bodyText = "Успешно извршена трансакција \n Уплатена сума: " + model.amount + " ден. \n Трансакциски код: " + model.TransId + "\n Автентикациски код: " + model.AuthCode;

                mailService.setBodyText(bodyText);
                mailService.attach(new System.Net.Mail.Attachment(fullPath));
                mailService.sendMail();
            }
            return View(model);
        }

        [Route("PaymentFail")]
        public ActionResult PaymentFail()
        {
            PaymentFailModel model = new PaymentFailModel();
            model.ErrMsg = Request.Form.Get("ErrMsg");
            model.mdErrorMsg = Request.Form.Get("mdErrorMsg");
            return View(model);
        }
    }
}