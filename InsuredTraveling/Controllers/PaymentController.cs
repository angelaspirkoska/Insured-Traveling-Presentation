using Authentication.WEB.Models;
using Authentication.WEB.Services;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("Payment")]
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            PaymentModel model = new PaymentModel();

            model.clientId = "180000069";                   //Merchant Id defined by bank to user
            model.amount = "9.95";                         //Transaction amount
            model.oid = "";                                //Order Id. Must be unique. If left blank, system will generate a unique one.
            model.okUrl = "http://localhost:49639/Payment/PaymentSuccess";                      //URL which client be redirected if authentication is successful
            model.failUrl = "http://localhost:49639/Payment/PaymentFail";                    //URL which client be redirected if authentication is not successful
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


            int id = 2;
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            model.Pat = entities.patnicko.Where(x => x.Polisa_Broj == id).FirstOrDefault();
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

                MailService mailService = new MailService("asaid@optimalreinsurance.com");
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