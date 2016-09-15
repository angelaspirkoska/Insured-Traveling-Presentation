using Authentication.WEB.Models;
using Authentication.WEB.Services;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.WEB.Controllers
{
    public class MobilePolicyApiController : ApiController
    {

        public HttpResponseMessage PostComplex(MobilePolicyModel policy)
        {
            if (policy != null)
            {
                policy.Token = HttpUtility.HtmlEncode(policy.Token);
                string EMBG = policy.EMBG;
                ValidationService validate = new ValidationService();
                if (validate.validateEMBG(policy.EMBG))
                {

                }
                // convert any html markup in the status text.
                //   update.asdf = HttpUtility.HtmlEncode(update.asdf);
                // update.Ivan = HttpUtility.HtmlEncode(update.Ivan);
                string Premium = null;
                Premium = HttpUtility.HtmlEncode(policy.PremiumAmount);
                //   update. = HttpUtility.HtmlEncode(update.);
                // Assign a new ID.
                var id = Guid.NewGuid();
                //updates[id] = update;

                // Create a 201 response.
                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(policy.PremiumAmount)
                };

                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { action = "status", id = id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
