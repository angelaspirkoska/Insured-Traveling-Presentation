using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/SavaMobile")]
    public class SavaMobileController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CreatePolicy")]
        public IHttpActionResult CreatePolicy()
        {

            return Ok();
        }
    }
}