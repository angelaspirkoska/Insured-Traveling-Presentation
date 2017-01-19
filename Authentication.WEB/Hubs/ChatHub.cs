using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using InsuredTraveling.Filters;
using Newtonsoft.Json.Linq;

namespace InsuredTraveling.Hubs
{
    public class ChatHub : Hub
    {
        readonly InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public override Task OnConnected()
        {
            RoleAuthorize r = new RoleAuthorize();
           var username = System.Web.HttpContext.Current.User.Identity.Name;
            if (r.IsUser("admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);

                
                var responseList = _db.chat_requests.Where(x => x.Accepted == false).Select(x => new {
                    from = x.Requested_by,
                    timestamp = x.Datetime_request.ToString()
                }).ToArray();

                JObject response = new JObject();

                response.Add("numberRequests", responseList.Length);

                response.Add("data", JArray.FromObject(responseList));

                Clients.Group(username).MessageRequest(response);
            }
            else if(r.IsUser("end user"))
            {
                //Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);
            }
            return base.OnConnected();
        }

        public void OnConnectedMobile(string username)
        {
            Groups.Add(Context.ConnectionId, username);
        }

        public void SendRequest()
        {
            var username = Context.User.Identity.Name;
            int requestId = 0;
            var listRequestsByUser = _db.chat_requests.Where(x => x.Requested_by == username && x.Accepted == false).ToList();

            if (listRequestsByUser.Count == 0)
            {

                var request = new chat_requests {
                    Requested_by = username,
                    fnol_created = false
            };
                
            try
            {
               var a = _db.chat_requests.Add(request);
               var b= _db.SaveChanges();
                  requestId = a.ID;
            }
            catch(Exception ex)
            {
                
            }
            finally
            {

            }
            var numberRequests = _db.chat_requests.Where(x => x.Accepted == false).ToList();

            var responseAdmins = new JObject();
            responseAdmins.Add("numberRequests", numberRequests.Count);
            var array = new JArray();
            foreach(chat_requests chatRequest in numberRequests)
                {
                    array.Add(new JObject(new JProperty("from", chatRequest.Requested_by), new JProperty("timestamp", chatRequest.Datetime_request.ToString())));
                }
            
            responseAdmins.Add("data", array);
            Clients.Group("Admins").MessageRequest(responseAdmins);
            }
            else
            {
                requestId = listRequestsByUser.Last().ID;
            }
            JObject requestIdJson = new JObject();
            requestIdJson.Add("requestId", requestId);
            Clients.Group(username).RequestId(requestId);
        }


        public void SendRequestMobile(string username)
        {
            int requestId = 0;
            var listRequestsByUser = _db.chat_requests.Where(x => x.Requested_by == username && x.Accepted == false).ToList();

            if (listRequestsByUser.Count == 0)
            {

                var request = new chat_requests
                {
                    Requested_by = username
                };

                try
                {
                    var a = _db.chat_requests.Add(request);
                    _db.SaveChanges();
                    requestId = a.ID;
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
                var numberRequests = _db.chat_requests.Where(x => x.Accepted == false).ToList();

                var responseAdmins = new JObject();
                responseAdmins.Add("numberRequests", numberRequests.Count);
                var array = new JArray();
                foreach (chat_requests chatRequest in numberRequests)
                {
                    array.Add(new JObject(new JProperty("from", chatRequest.Requested_by), new JProperty("timestamp", chatRequest.Datetime_request.ToString())));
                }

                responseAdmins.Add("data", array);
                Clients.Group("Admins").MessageRequest(responseAdmins);
            }
            else
            {
                requestId = listRequestsByUser.Last().ID;
            }
            JObject requestIdJson = new JObject();
            requestIdJson.Add("requestId", requestId);
            Clients.Group(username).RequestId(new JObject("requestId",requestId));

        }
        

        public void SendMessageMobile(string from, string to, string message, string requestId)
        {
           // var from = Context.User.Identity.Name;
            var data = new JObject();
            data.Add("from", from);
            data.Add("message", message);
            data.Add("requestId", requestId);
                
            Clients.Group(to).ReceiveMessage(data);

            SaveMessage(int.Parse(requestId), from, message);

        }
        public void SendMessage(String to, String message, string requestId)
        {
            var from = Context.User.Identity.Name;
            var data = new JObject();
            data.Add("from", from);
            data.Add("message", message);
            data.Add("requestId", requestId);
            Clients.Group(to).ReceiveMessage(data);    

            SaveMessage(int.Parse(requestId), from, message);

            //zacuvuvanje u bazu fali!!!
        }

      
        public void AcceptRequest(string enduser)
        {
            var username = Context.User.Identity.Name;

            var request = _db.chat_requests.Where(x => x.Requested_by == enduser && x.Accepted == false).SingleOrDefault();


            request.Accepted_by = username;
            request.Accepted = true;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            var response = new JObject();
            response.Add("requestId", request.ID);
            response.Add("admin", username);

            var responseAdmin = new JObject();
            responseAdmin.Add("requestId", request.ID);
            responseAdmin.Add("enduser", enduser);

            Clients.Group(enduser).SendAcknowledge(response);
            Clients.Group(username).ReceiveId(responseAdmin);

            var numberRequests = _db.chat_requests.Where(x => x.Accepted == false).ToList();

            var responseAdmins = new JObject();
            responseAdmins.Add("numberRequests", numberRequests.Count);
            var array = new JArray();
            foreach (chat_requests chatRequest in numberRequests)
            {
                array.Add(new JObject(new JProperty("from", chatRequest.Requested_by), new JProperty("timestamp", chatRequest.Datetime_request.ToString())));
            }

            responseAdmins.Add("data", array);
            Clients.Group("Admins").MessageRequest(responseAdmins);

        }


        private void SaveMessage(int requestId, string fromUsername, string textMessage)
        {
            message message = new message();
            message.ConversationID = requestId;
            message.Text = textMessage;
            message.Timestamp = DateTime.Now;
            message.from_username = fromUsername;
            _db.messages.Add(message);
            _db.SaveChangesAsync();
        }

    }
}