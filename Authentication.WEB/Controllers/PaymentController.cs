using Authentication.WEB.Models;
using Authentication.WEB.Services;
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
using InsuredTraveling.Filters;
using InsuredTraveling.Controllers;
using System.Net.Mail;

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
            ViewBag.IsPaid = false;
            this._us = us;
            this._ps = ps;
            _iss = iss;
            _pis = pis;
            _acs = acs;
        }

        // GET: Payment
        [HttpGet]
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
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Policy p)
        {
            //for model validate sake
            p.Travel_NumberID = 1;
            p.Policy_TypeID = 1;
            p.Travel_Insurance_TypeID = 1;
            p.Retaining_RiskID = 1;

            ViewBag.IsPaid = false;
            PaymentModel model = new PaymentModel();

            p.isMobile = false;
            var PolicyId = SavePolicyHelper.SavePolicy(p, _ps, _us, _iss, _pis, _acs);

            var policy = _ps.GetPolicyById(PolicyId);
            model.mainInsured = _pis.GetAllInsuredByPolicyId(policy.ID).First();
            model.PolicyNumber = policy.Policy_Number;
            var additionalCharges = _acs.GetAdditionalChargesByPolicyId(PolicyId);

            model.additionalCharge1 = "Без доплаток";
            model.additionalCharge2 = "Без доплаток";
            if (additionalCharges.Count >= 1 && additionalCharges[0] != null)
            {
                model.additionalCharge1 = _acs.GetAdditionalChargeName(additionalCharges[0].ID);
            }
            if (additionalCharges.Count >= 2 && additionalCharges[1] != null)
            {
                model.additionalCharge2 = _acs.GetAdditionalChargeName(additionalCharges[1].ID);
            }

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
            model.retaining_risk = "No Deductible";
            model.retaining_risk_mk = "Без франшиза";
            model.Pat = policy;
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
            var policyNumber = Request.Form.Get("PolicyNumber");
            if (model.mdStatus == "1" || model.mdStatus == "2" || model.mdStatus == "3" || model.mdStatus == "4")
            {

                string fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PolicyPDF/" + model.TransId + model.amount + ".pdf");

                PrintPolicyModel pat = new PrintPolicyModel();
                pat.Pat = _ps.GetPolicyIdByPolicyNumber(policyNumber);
                _ps.UpdatePaymentStatus(policyNumber);
                var actionResult = new ViewAsPdf("Print", pat);
                var byteArray = actionResult.BuildPdf(ControllerContext);

                var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();

                // ADD MAIL ADRESS
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/EmailHeaderSuccess.png"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();

                string body1 = string.Format(@"   
                     <div style='margin-left:20px'>
                     <img style='width:700px' src=""cid:{0}"" />
                     <p> <b>Welcome to Insured Traveling </b> - the standalone platform for online sales of insurance policies.</p>                  
                     <br /> <br /> 
                     <br />" + "Успешно извршена трансакција \n Уплатена сума: " + model.amount + " ден. \n Трансакциски код: " + model.TransId + "\n Автентикациски код: " + model.AuthCode +
                     "<br /> <br />Thank you for using our Insurence Service</div>"
                , inlineLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(body1, null, "text/html");
                view.LinkedResources.Add(inlineLogo);

                var PolicyHolderEmail = _ps.GetPolicyHolderEmailByPolicyId(pat.Pat.ID);
                MailService mailService = new MailService(PolicyHolderEmail);
                mailService.setSubject("Издадена полиса број: " + model.oid);


                mailService.setBodyText(body1, true);
                mailService.AlternativeViews(view);
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