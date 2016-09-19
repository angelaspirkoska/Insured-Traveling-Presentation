using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Configuration;

namespace Authentication.WEB.Models
{
    class HalkbankPaymentModel
    {
        public string Pay(string amount, string oid, string xid, string creditCard, string expMonth, string expYear, string cv2)
        {
            var responseData = PostRequestOne(amount, oid, xid, creditCard, expMonth, expYear, cv2);

            var responseFromSecondRequest = ProcessPostRequestTwo(responseData);

            return responseFromSecondRequest;
        }

        private string ProcessPostRequestTwo(string responseData)
        {
            var htmlDecode = WebUtility.HtmlDecode(responseData);
            string response = "";

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlDecode);

            var returnForm = document.DocumentNode.Descendants().First(m => m.Name == "form");
            if (returnForm.Attributes["action"] != null)
            {
                var rfActionUrl = returnForm.Attributes["action"].Value;

                var returnFormData = returnForm.SelectNodes("//input[@type='hidden']");

                NameValueCollection frmData = new NameValueCollection();

                foreach (var node in returnFormData)
                {
                    frmData.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
                }
                response = PostRequestTwo(rfActionUrl, frmData);
            }
            return response;
        }

        private string PostRequestOne(string amount, string oid, string xid, string creditCard, string expMonth, string expYear, string cv2)
        {
            const string apiServerUrl = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";

            // Create a request for the URL. 
            HttpWebRequest request = WebRequest.Create(apiServerUrl) as HttpWebRequest;
            CookieContainer cookies = new CookieContainer();

            // If required by the server, set the credentials.s tring amount, string oid, string xid, string creditCard, string expMonth, string expYear, string cv
            string postData = GetPostDataAsString(GetPostDataForRequestOneReal(amount, oid, xid, creditCard, expMonth, expYear, cv2));

            //byte[] send = Encoding.Default.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;
            request.ContentLength = postData.Length;
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Referer = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();


            StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
            string responseData = responseReader.ReadToEnd();


            responseReader.Close();
            request.GetResponse().Close();
            return responseData;
        }


        private string PostRequestTwo(string url, NameValueCollection frmData)
        {

            // Create a request for the URL. 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookies = new CookieContainer();

            // If required by the server, set the credentials.
            string postData = GetPostDataAsString(frmData);

            //byte[] send = Encoding.Default.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookies;
            request.ContentLength = postData.Length;
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);


            request.ContentLength = byteArray.Length;

            using (Stream requestWriter = request.GetRequestStream())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(postData);
                requestWriter.Write(bytes, 0, bytes.Length);
            }

            StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
            string responseData = responseReader.ReadToEnd();




            responseReader.Close();
            request.GetResponse().Close();

            return responseData;
        }

        /*  private NameValueCollection GetPostDataForRequestOne()
          {
              const string storetype = "3D_PAY_HOSTING";
              const string transactionType = "auth";
              const string installmentCount = "";
              const string refreshTime = "0";
              const string currency = "807"; //mkd den

              const string clientId = "180000069";
              string storekey = "SKEY3545";
              const string amount = "10.00";
              string oid = Guid.NewGuid().ToString(); //Order Id. Must be unique. If left blank, system will generate a unique one.

              string okUrl = "http://localhost:49639/api/halkbankpayment/handle";                      //URL which client be redirected if authentication is successful
              string failUrl = "http://localhost:49639/api/halkbankpayment/handle";                    //URL which client be redirected if authentication is not successful

              string lang = "mk";

              string rnd = DateTime.Now.ToString();           //A random number, such as date/time

              string hashstr = clientId + oid + amount + okUrl + failUrl + transactionType + installmentCount + rnd + storekey;
              System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
              byte[] hashbytes = Encoding.GetEncoding("UTF-8").GetBytes(hashstr);
              byte[] inputbytes = sha.ComputeHash(hashbytes);

              String hash = Convert.ToBase64String(inputbytes); //Hash value used for validation

              Random r = new Random();

              NameValueCollection nameValueCollection = new NameValueCollection
              {
                  {"clientid", clientId},
                  {"amount", amount},
                  {"oid", oid},
                  {"okUrl", okUrl},
                  {"failUrl", failUrl},
                  {"islemtipi", transactionType},
                  {"taksit", installmentCount},
                  {"rnd", rnd},
                  {"hash", hash},
                  {"storetype", storetype},
                  {"lang", lang},
                  {"currency", currency},
                  {"refreshtime", refreshTime},
                  {"encoding", "UTF-8"},
                  {"xid", EncodeTo64(r.Next(100000).ToString())},//ova da se praka vo metodot
                  {"pan", "6775820411838201"},//ova se praka od nadvor
                  {"Ecom_Payment_Card_ExpDate_Month", "11"},//ova se praka od nadvor
                  {"Ecom_Payment_Card_ExpDate_Year", "17"},//ova se praka od nadvor
                  {"cv2", "698"}
              };


              return nameValueCollection;
          } */

        private NameValueCollection GetPostDataForRequestOneReal(string amount, string oid, string xid, string creditCard, string expMonth, string expYear, string cv2)
        {
            const string storetype = "3D_PAY_HOSTING";
            const string transactionType = "auth";
            const string installmentCount = "";
            const string refreshTime = "0";
            const string currency = "807"; //mkd den

            const string clientId = "180000069";
            string storekey = "SKEY3545";
            string okUrl = ConfigurationManager.AppSettings["webpage_url"] + "/api/halkbankpayment/handle";                      //URL which client be redirected if authentication is successful
            string failUrl = ConfigurationManager.AppSettings["webpage_url"] + "/api/halkbankpayment/handle";
            //  string okUrl = ConfigurationManager.AppSettings["webpage_url"] + "/api/halkbankpayment/handle";                      //URL which client be redirected if authentication is successful
            // string failUrl = ConfigurationManager.AppSettings["webpage_url"] + "/api/halkbankpayment/handle";                    //URL which client be redirected if authentication is not successful

            string lang = "mk";

            string rnd = DateTime.Now.ToString();           //A random number, such as date/time

            string hashstr = clientId + oid + amount + okUrl + failUrl + transactionType + installmentCount + rnd + storekey;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = Encoding.GetEncoding("UTF-8").GetBytes(hashstr);
            byte[] inputbytes = sha.ComputeHash(hashbytes);

            String hash = Convert.ToBase64String(inputbytes); //Hash value used for validation
            Random r = new Random();

            NameValueCollection nameValueCollection = new NameValueCollection
            {
                {"clientid", clientId},
                {"amount", amount},
                {"oid", oid},
                {"okUrl", okUrl},
                {"failUrl", failUrl},
                {"islemtipi", transactionType},
                {"taksit", installmentCount},
                {"rnd", rnd},
                {"hash", hash},
                {"storetype", storetype},
                {"lang", lang},
                {"currency", currency},
                {"refreshtime", refreshTime},
                {"encoding", "UTF-8"},
                {"xid", EncodeTo64(r.Next(100000).ToString())},//ova da se praka vo metodot
                {"pan", creditCard},//ova se praka od nadvor
                {"Ecom_Payment_Card_ExpDate_Month", expMonth},//ova se praka od nadvor
                {"Ecom_Payment_Card_ExpDate_Year", expYear},//ova se praka od nadvor
                {"cv2", cv2} //ova se praka od nadvor
            };


            return nameValueCollection;
        }

        private string GetPostDataAsString(NameValueCollection nv)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nv.Count; i++)
            {
                sb.Append(i == nv.Count - 1
                    ? $"{nv.GetKey(i)}={nv.Get(i)}"
                    : $"{nv.GetKey(i)}={nv.Get(i)}&");
            }

            return sb.ToString();
        }

        private string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                = Encoding.ASCII.GetBytes(toEncode);
            string returnValue
                = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public Dictionary<string, object> NvcToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }

            return result;
        }
    }
}
