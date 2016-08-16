using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace InsuredTraveling.Filters
{
    public class RecaptchaFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request["g-recaptcha-response"] == null) return;
            var privatekey = WebConfigurationManager.AppSettings["RecaptchaPrivateKey"];
            var response = filterContext.RequestContext.HttpContext.Request["g-recaptcha-response"];
            filterContext.ActionParameters["CaptchaValid"] = Validate(response, privatekey);
        }

        public static bool Validate(string mainresponse, string privatekey)
        {

            try
            {
                var req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + privatekey + "&response=" + mainresponse);

                var response = req.GetResponse();

                using (var readStream = new StreamReader(response.GetResponseStream()))
                {
                    var jsonResponse = readStream.ReadToEnd();

                    var jobj = JsonConvert.DeserializeObject<JsonResponseObject>(jsonResponse);

                    return jobj.Success;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public class JsonResponseObject
        {
            public bool Success { get; set; }
            [JsonProperty("error-codes")]
            public List<string> Errorcodes { get; set; }
        }
    }
}