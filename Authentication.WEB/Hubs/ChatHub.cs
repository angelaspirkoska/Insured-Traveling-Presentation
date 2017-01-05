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
            var listRequestsByUser = _db.chat_requests.Where(x => x.Requested_by == username && x.Accepted == false).ToList();
            if (listRequestsByUser.Count == 0)
            {
        
            var request = new chat_requests {
                Requested_by = username
            };

            try
            {
                _db.chat_requests.Add(request);
                _db.SaveChanges();
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
        }


        public void SendRequestMobile(string username)
        {
            //var username = Context.User.Identity.Name;
            var listRequestsByUser = _db.chat_requests.Where(x => x.Requested_by == username && x.Accepted == false).ToList();
            if (listRequestsByUser.Count == 0)
            {

                var request = new chat_requests
                {
                    Requested_by = username
                };

                try
                {
                    _db.chat_requests.Add(request);
                    _db.SaveChanges();
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
        }

        public void SendMessageMobile(String from, String to, String message)
        {
           // var from = Context.User.Identity.Name;
            var data = new JObject();
            data.Add("from", from);
            data.Add("message", message);
            Clients.Group(to).ReceiveMessage(data);

            //zacuvuvanje u bazu fali!!!
        }
        public void SendMessage(String to, String message)
        {
            var from = Context.User.Identity.Name;
            var data = new JObject();
            data.Add("from", from);
            data.Add("message", message);
            Clients.Group(to).ReceiveMessage(data);

            //zacuvuvanje u bazu fali!!!
        }
        public void AcceptRequest(string enduser)
        {
            var username = Context.User.Identity.Name;

            var requests = _db.chat_requests.Where(request => request.Requested_by == enduser && request.Accepted == false).ToList();
            requests.ForEach(delegate(chat_requests request)
            {
                request.Accepted_by = username;
                request.Accepted = true;
            });

            var conversation = _db.conversations.FirstOrDefault(conv => conv.admin == username && conv.user == enduser);
            if(conversation == null)
            {
                var conversationNew = new conversation();
                conversationNew.admin = username;
                conversationNew.user = enduser;
                _db.conversations.Add(conversationNew);
            }

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
            response.Add("conversationId", conversation.ID);
            response.Add("admin", username);

            Clients.Group(enduser).SendAcknowledge(response);
            Clients.Group(username).ReceiveId(conversation.ID);
            //Clients.Group(username).MessageRequest(response);

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




    }
}