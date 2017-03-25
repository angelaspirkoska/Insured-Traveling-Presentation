using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InsuredTraveling.DI;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;

namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/OkSetup")]
    public class Ok_SetupController : ApiController
    {
        private IOkSetupService _os;
        private IDiscountService _ds;

        public Ok_SetupController(IOkSetupService os,IDiscountService ds)
        {
            _os = os;
            _ds = ds;
        }
        [HttpGet]
        [Route("CheckSSN")]
        public IHttpActionResult OK_SETUP_SSN()
        {
            ok_setup Last_Entry = _os.GetLast();
            var data = new JObject();
            if (Last_Entry.SSNValidationActive == 0)
            {
                data.Add("message", false);
                return BadRequest();
            }
            else
            {
                data.Add("message", true);
                return Json(data);
            }

            

        }

        [System.Web.Http.Route("FindDiscountName")]
        public async Task<IHttpActionResult> FindUsername(DiscountModel username)
        {
            if (!String.IsNullOrEmpty(username.Discount_Name))
            {


                if (!_ds.DiscountCodeExist(username.Discount_Name))
                {
                    return Ok();

                }else
                {
                    return BadRequest();
                }
                
           

            }
            return BadRequest();
        }


    }
}
