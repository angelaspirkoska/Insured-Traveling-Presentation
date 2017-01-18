using InsuredTraveling.DI;
using InsuredTraveling.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {
        private IChatService _ics;
        public ChatController(IChatService ics)
        {
            _ics = ics;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("requestinfo")]
        public JObject requestInfo(Request request)
        {
            var requestInfo = _ics.ChatRequest(request.requestId);
            JObject response = new JObject();
            response.Add("requestId", requestInfo.ID);
            response.Add("isAccepted", requestInfo.Accepted);
            response.Add("acceptedBy", requestInfo.Accepted_by);
            response.Add("isClosed", requestInfo.fnol_created);
            return response;
        }

        //[System.Web.Http.HttpGet]
        //[AllowAnonymous]
        //[Route("requestinfo")]
        //public JObject isAccepted(Request request)
        //{
        //    var requestInfo = _ics.ChatRequest(request.requestId);
        //    JObject response = new JObject();
        //    response.Add("requestId", requestInfo.ID);
        //    response.Add("isAccepted", requestInfo.Accepted);
        //    response.Add("acceptedBy", requestInfo.Accepted_by);
        //    response.Add("isClosed", requestInfo.fnol_created);
        //    return response;
        //}


    }
}