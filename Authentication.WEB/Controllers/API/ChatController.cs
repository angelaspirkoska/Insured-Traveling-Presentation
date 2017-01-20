using InsuredTraveling.DI;
using InsuredTraveling.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Configuration;

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
    
        [Route("requestinfo")]
        public JObject requestInfo(Request request)
        {
            var requestInfo = _ics.ChatRequest(request.requestId);
            JObject response = new JObject();
            response.Add("requestId", requestInfo.ID);
            response.Add("isAccepted", requestInfo.Accepted);
            response.Add("acceptedBy", requestInfo.Accepted_by);
            response.Add("isFnolCreated", requestInfo.fnol_created);
            response.Add("isDiscarded", requestInfo.discarded);
            return response;
        }


        [HttpPost]

        [Route("lasttenmessages")]
        public JObject LastTenMessages(Request request)
        {
            var messages = _ics.LastTenMessagesByRequest(request.requestId);
            JObject response = new JObject();
            JArray listMessages = new JArray();
            foreach(message message in messages)
            {
                JObject messageJSON = new JObject();
                messageJSON.Add("Id", message.ID);
                messageJSON.Add("RequestId", message.ConversationID);
                messageJSON.Add("Text", message.Text);
                messageJSON.Add("Timestamp", message.Timestamp);
                messageJSON.Add("From", message.from_username);
                listMessages.Add(messageJSON);
            }
            response.Add("Messages",listMessages);
            return response;
        }

        [HttpPost]

        [Route("nexttenmessages")]
        public JObject NextTenMessages(Request request)
        {
            var messages = _ics.NextTenMessagesByRequest(request.requestId, request.messageId);
            JObject response = new JObject();
            JArray listMessages = new JArray();
            foreach (message message in messages)
            {
                JObject messageJSON = new JObject();
                messageJSON.Add("Id", message.ID);
                messageJSON.Add("RequestId", message.ConversationID);
                messageJSON.Add("Text", message.Text);
                messageJSON.Add("Timestamp", message.Timestamp);
                messageJSON.Add("From", message.from_username);
                listMessages.Add(messageJSON);
            }
            response.Add("Messages", listMessages);
            return response;
        }

    }
}