using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InsuredTraveling.DI;
using Newtonsoft.Json.Linq;

namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/OkSetup")]
    public class Ok_SetupController : ApiController
    {
        private IOkSetupService _os;
        

        public Ok_SetupController(IOkSetupService os)
        {
            _os = os;
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


    }
}
