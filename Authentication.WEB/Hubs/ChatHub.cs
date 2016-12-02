using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        public void SendRequest()
        {
            var username = Context.User.Identity.Name;
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

            var response = new JObject();
            response.Add("numberRequests", 1);
            var array = new JArray();
            array.Add(new
            {
                from = username,
                timestamp = request.Datetime_request
            });
            response.Add("data", array);
            Clients.Group("Admins").MessageRequest(response);

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
                conversation.admin = username;
                conversation.user = enduser;
                _db.conversations.Add(conversation);
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

            var response = new
            {
                conversationId = conversation.ID,
                admin = username
            };

            Clients.Group(enduser).SendAcknowledge(response);
            Clients.Group(username).ReceiveId(conversation.ID);
        }

    }
}