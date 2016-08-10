using Authentication.WEB.Models;
using InsuredTraveling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Authentication.WEB.Controllers
{
    public class HalkbankPaymentApiController : ApiController
    {
        [Route("api/halkbankpayment/handle")]
        [HttpPost]
        public IHttpActionResult Handle()
        {
            HalkbankPaymentModel halkbankPayment = new HalkbankPaymentModel();
            Task<NameValueCollection> formDataAsync = Request.Content.ReadAsFormDataAsync();
            var nvc = formDataAsync.Result;
            var dict = halkbankPayment.NvcToDictionary(nvc, false);

            string json = JsonConvert.SerializeObject(dict);

            return Json(json);
        }

        [Route("api/halkbankpayment/pay")]
        [HttpPost]
        public IHttpActionResult Pay(MobilePolicyModel MobilePolicy)
        {
            HalkbankPaymentModel halkbankPayment = new HalkbankPaymentModel();
            InsuredTravelingEntity TravelEntity = new InsuredTravelingEntity();

            transaction TransactionModel = new transaction();

            MobilePolicy.PremiumAmount = HttpUtility.HtmlEncode(MobilePolicy.PremiumAmount);
            string amount = MobilePolicy.PremiumAmount;



            string oid = (TravelEntity.transactions.OrderByDescending(p => p.OrderID).Select(r => r.OrderID).First() + 1).ToString();
            string xid = ""; // Generated in function
            string creditCard = MobilePolicy.CreditCard;
            string expMonth = MobilePolicy.expMonth;
            string expYear = MobilePolicy.expYear;
            string cv2 = MobilePolicy.cv2;

            var payResult = halkbankPayment.Pay(amount, oid, xid, creditCard, expMonth, expYear, cv2); // Add arguments here
            var cleanJson = payResult.Replace(@"\""", "'").Replace("\"", "");

            var dict = JObject.Parse(cleanJson);
            //pravis sto sakas

            TransactionModel.OrderID = oid;
            TransactionModel.mdStatus = dict["mdStatus"].ToString();
            TransactionModel.merchantID = dict["merchantID"].ToString();
            if (dict["amount"].ToString() == amount || (dict["ReturnOid"].ToString() == oid))
            {
                TransactionModel.amount = dict["amount"].ToString();
            }
            else
            {
                return Json("error: Transaction ID OR AMOUNT INCOSISTENT"); //CHECK THIS
            }
            TransactionModel.ACQBIN = dict["ACQBIN"].ToString();
            TransactionModel.Response = dict["Response"].ToString();
            TransactionModel.xid = dict["xid"].ToString();
            TransactionModel.islemtipi = dict["islemtipi"].ToString();
            TransactionModel.ProcReturnCode = dict["ProcReturnCode"].ToString();
            TransactionModel.mdErrorMsg = dict["mdErrorMsg"].ToString();
            TransactionModel.ErrMsg = dict["ErrMsg"].ToString();
            TransactionModel.HASH = dict["HASH"].ToString();
            TransactionModel.TRANID = dict["TRANID"].ToString();
            TravelEntity.transactions.Add(TransactionModel);
            TravelEntity.SaveChanges();

            if ((TransactionModel.mdStatus == "1" || TransactionModel.mdStatus == "2" || TransactionModel.mdStatus == "3" || TransactionModel.mdStatus == "4") && (TransactionModel.Response == "Approved"))
            {
                var dict2 = new JObject();
                dict2.Add("IsValid", "true");
                dict2.Add("mdStatus", TransactionModel.mdStatus);
                dict2.Add("amount", TransactionModel.amount);
                dict2.Add("OrderID", TransactionModel.OrderID);
                return Json(dict2);

            }
            else
            {
                var dict3 = new JObject();
                dict3.Add("IsValid", "false");
                dict3.Add("mdStatus", TransactionModel.mdStatus);
                dict3.Add("amount", TransactionModel.amount);
                dict3.Add("OrderID", TransactionModel.OrderID);
                dict3.Add("ErrMsg", dict["ErrMsg"].ToString());

                return Json(dict3);
            }
            //return Json(cleanJson);
        }
    }
}
