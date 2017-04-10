using InsuredTraveling.DI;
using InsuredTraveling.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Configuration;
using InsuredTraveling.Filters;

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



        [HttpGet]

        [Route("lasttenmessagesweb")]
        public JObject LastTenMessagesWeb(int requestId, string username)
        {
            string ichatwith = "";
            JObject response = new JObject();
            JArray listMessages = new JArray();
            var messages = _ics.LastTenMessagesByRequest(requestId).OrderBy(x => x.ID);
            var request = _ics.ChatRequest(requestId);
            var requestedby = request.Requested_by;
            var acceptedby = request.Accepted_by;

            if (username.Equals(requestedby) || username.Equals(acceptedby))
            {
                if (username == requestedby)
                {
                    ichatwith = acceptedby;
                }
                else
                {
                    ichatwith = requestedby;
                }

                response.Add("ichatwith", ichatwith);

                foreach (message message in messages)
                {
                    JObject messageJSON = new JObject();

                    messageJSON.Add("Id", message.ID);
                    messageJSON.Add("RequestId", message.ConversationID);
                    messageJSON.Add("Text", message.Text);
                    messageJSON.Add("Date", message.Timestamp.Date.Day + "-" + message.Timestamp.Date.Month + "-" + message.Timestamp.Date.Year);
                    messageJSON.Add("Hour", message.Timestamp.Hour);
                    messageJSON.Add("Minute", message.Timestamp.Minute);
                    messageJSON.Add("From", message.from_username);

                    listMessages.Add(messageJSON);
                }
            }
            else
            {
                response.Add("Error", "Not your username");
            }

            response.Add("Messages", listMessages);
            
            return response;
        }

        [HttpGet]

        [Route("nexttenmessagesweb")]
        public JObject NextTenMessagesWeb(int requestId, int messageId, string username)
        {

            string ichatwith = "";
            JObject response = new JObject();
            JArray listMessages = new JArray();
            var messages = _ics.NextTenMessagesByRequest(requestId, messageId);
            if (messages.Count == 0)
            {
                response.Add("Messages", "End");
                return response;
            }
            var m = messages.OrderByDescending(x => x.ID);
            var requestedby = m.FirstOrDefault().chat_requests.Requested_by;
            var acceptedby = m.FirstOrDefault().chat_requests.Accepted_by;

            if (username.Equals(requestedby) || username.Equals(acceptedby))
            {
                if (username == requestedby)
                {
                    ichatwith = acceptedby;
                }
                else
                {
                    ichatwith = requestedby;
                }

                response.Add("ichatwith", ichatwith);

                foreach (message message in m)
                {
                    JObject messageJSON = new JObject();

                    messageJSON.Add("Id", message.ID);
                    messageJSON.Add("RequestId", message.ConversationID);
                    messageJSON.Add("Text", message.Text);
                    messageJSON.Add("Date", message.Timestamp.Date.Day + "-" + message.Timestamp.Date.Month + "-" + message.Timestamp.Date.Year);
                    messageJSON.Add("Hour", message.Timestamp.Hour);
                    messageJSON.Add("Minute", message.Timestamp.Minute);
                    messageJSON.Add("From", message.from_username);

                    listMessages.Add(messageJSON);
                }

                response.Add("Messages", listMessages);

            }
            else
            {
                response.Add("Error", "Not your username");
            }



            return response;
        }


    }
}