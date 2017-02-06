using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using InsuredTraveling.Filters;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using InsuredTraveling.DTOs;

namespace InsuredTraveling.Hubs
{
    public class ChatHub : Hub
    {
        readonly InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
        public override Task OnConnected()
        {
            RoleAuthorize r = new RoleAuthorize();
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            if (r.IsUser("admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);

                var responseList = _db.chat_requests.Where(x => x.Accepted == false).Select(x => new
                {
                    from = x.Requested_by,
                    timestamp = x.Datetime_request.ToString()
                }).ToArray();

                JObject response = new JObject();

                response.Add("numberRequests", responseList.Length);

                response.Add("data", JArray.FromObject(responseList));

                Clients.Group(username).MessageRequest(response);

                var chatsActive = _db.chat_requests.Where(x => x.Accepted == true && x.Accepted_by.Equals(username) && x.discarded == false
                                                        && x.fnol_created == false).ToList();
                JObject messages = new JObject();

                JArray messageList = new JArray();
                foreach (chat_requests chat in chatsActive)
                {
                    var messageLast = _db.messages.Where(x => x.ConversationID == chat.ID).OrderByDescending(x => x.Timestamp).FirstOrDefault();
                    if (messageLast != null)
                    {
                        JObject temp = new JObject();
                        temp.Add("requestId", chat.ID);
                        temp.Add("from", messageLast.from_username);
                        temp.Add("timestamp", messageLast.Timestamp);
                        temp.Add("messageId", messageLast.ID);
                        temp.Add("message", messageLast.Text);
                        temp.Add("admin", "true");
                        temp.Add("ichatwith", chat.Requested_by);
                        messageList.Add(temp);
                    }

                }
                messages.Add("messages", messageList);
                Clients.Group(username).ActiveMessages(messages);
            }
            else if (r.IsUser("end user"))
            {
                //Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);

                var chatsActive = _db.chat_requests.Where(x => x.Accepted == true && x.Requested_by.Equals(username) && x.discarded == false
                                                     && x.fnol_created == false).ToList();
                JObject messages = new JObject();
                JArray messageList = new JArray();
                foreach (chat_requests chat in chatsActive)
                {
                    var messageLast = _db.messages.Where(x => x.ConversationID == chat.ID).OrderByDescending(x => x.Timestamp).FirstOrDefault();



                    if (messageLast != null)
                    {
                        JObject temp = new JObject();
                        temp.Add("requestId", chat.ID);
                        temp.Add("from", messageLast.from_username);
                        temp.Add("timestamp", messageLast.Timestamp);
                        temp.Add("messageId", messageLast.ID);
                        temp.Add("message", messageLast.Text);
                        temp.Add("ichatwith", chat.Accepted_by);
                        temp.Add("admin", "true");
                        messageList.Add(temp);
                    }

                }
                messages.Add("messages", messageList);
                Clients.Group(username).ActiveMessages(messages);

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

                var request = new chat_requests
                {
                    Requested_by = username,
                    fnol_created = false,
                    discarded = false
                };

                try
                {
                    var a = _db.chat_requests.Add(request);
                    var b = _db.SaveChanges();
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
                    Requested_by = username,
                    fnol_created = false,
                    discarded = false
                };

                try
                {
                    var a = _db.chat_requests.Add(request);
                    _db.SaveChanges();
                    requestId = a.ID;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
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
            var requestIdDTO = new RequestIdDTO
            {
                RequestId = requestId
            };
            Clients.Group(username).RequestId(requestIdDTO);
        }

        public void AcceptRequest(string enduser)
        {
            var username = Context.User.Identity.Name;
            var request = _db.chat_requests.Where(x => x.Requested_by == enduser && x.Accepted == false).SingleOrDefault();
            if (request == null)
                return;

            request.Accepted_by = username;
            request.Accepted = true;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var endUserResponseDTO = new AcknowledgeEndUserDTO
            {
                RequestId = request.ID,
                Admin = username
            };
            var adminResponseDTO = new AcknowledgeAdminDTO
            {
                RequestId = request.ID,
                EndUser = enduser
            };

            Clients.Group(enduser).SendAcknowledge(endUserResponseDTO);
            Clients.Group(username).ReceiveId(adminResponseDTO);

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

        public void SendMessage(String to, String message, string requestId)
        {
            var from = Context.User.Identity.Name;
            RoleAuthorize r = new RoleAuthorize();
            bool isAdmin = false;
            if (r.IsUser("admin"))
                isAdmin = true;
            //var data = new JObject();
            //data.Add("from", from);
            //data.Add("message", message);
            //data.Add("requestId", requestId);
            //data.Add("admin", admin);
            var messageMobileDTO = new MessageMobileDTO
            {
                From = from,
                Message = message,
                RequestId = int.Parse(requestId),
                Admin = isAdmin
            };
            Clients.Group(to).ReceiveMessage(messageMobileDTO);

            SaveMessage(int.Parse(requestId), from, message);

            //zacuvuvanje u bazu fali!!!
        }

        public void SendMessageMobile(string from, string to, string message, int requestId)
        {
            // var from = Context.User.Identity.Name;
            var messageMobileDTO = new MessageMobileDTO
            {
                From = from,
                Message = message,
                RequestId = requestId
            };
            Clients.Group(to).ReceiveMessage(messageMobileDTO);

            SaveMessage(requestId, from, message);

        }

        public void DiscardMessage(int requestId)
        {
            JObject adminResponse = new JObject();
            var request = _db.chat_requests.Where(x => x.ID == requestId).SingleOrDefault();
            if (!request.fnol_created)
            {
                request.discarded = true;
                _db.SaveChanges();

                JObject enduserResponse = new JObject();
                enduserResponse.Add("requestId", request.ID);
                enduserResponse.Add("message", "Your request has been discarded. First notice of loss is not created.");

                Clients.Group(request.Requested_by).DiscardedMessage(enduserResponse);


                adminResponse.Add("requestId", request.ID);
                adminResponse.Add("discarded", "true");
                adminResponse.Add("message", "You discarded the request. First notice of loss is not created.");
            }

            adminResponse.Add("requestId", request.ID);
            adminResponse.Add("discarded", "false");
            adminResponse.Add("message", "You can't discard this chat. First notice of loss was created.");
            Clients.Group(request.Accepted_by).Discarded(adminResponse);
        }

        public void CreateFnol(int requestId)
        {
            var request = _db.chat_requests.Where(x => x.ID == requestId).SingleOrDefault();
            JObject adminResponse = new JObject();

            if (!request.discarded)
            {
                request.fnol_created = true;
                _db.SaveChanges();

                //da se kreira fnol vo baza

                JObject enduserResponse = new JObject();
                enduserResponse.Add("requestId", request.ID);
                enduserResponse.Add("message", "First notice of loss was created successfully.");

                Clients.Group(request.Requested_by).FnolCreatedMessage(enduserResponse);

                adminResponse.Add("requestId", request.ID);
                adminResponse.Add("fnolCreated", "true");
                adminResponse.Add("message", "First notice of loss was created successfully.");

            }
            else
            {
                adminResponse.Add("requestId", request.ID);
                adminResponse.Add("fnolCreated", "false");
                adminResponse.Add("message", "This chat was already discarded.");
            }

            Clients.Group(request.Accepted_by).FnolCreated(adminResponse);

        }

        public void SaveMessage(int requestId, string fromUsername, string textMessage)
        {
            message message = new message();
            message.ConversationID = requestId;
            message.Text = textMessage;
            message.Timestamp = DateTime.Now;
            message.from_username = fromUsername;
            var m = _db.messages.Add(message);
            _db.SaveChangesAsync();

            var chatReq = _db.chat_requests.Where(x => x.ID == requestId).SingleOrDefault();
            JObject adminUpdate = new JObject();
            adminUpdate.Add("requestId", requestId);
            adminUpdate.Add("timestamp", message.Timestamp);
            adminUpdate.Add("from", chatReq.Requested_by);
            adminUpdate.Add("message", textMessage);
            adminUpdate.Add("messageId", m.ID);
            adminUpdate.Add("admin", "true");

            Clients.Group(chatReq.Accepted_by).UpdateChat(adminUpdate);

            JObject enduserUpdate = new JObject();
            enduserUpdate.Add("requestId", requestId);
            enduserUpdate.Add("timestamp", message.Timestamp);
            enduserUpdate.Add("from", chatReq.Accepted_by);
            enduserUpdate.Add("message", textMessage);
            enduserUpdate.Add("messageId", m.ID);
            enduserUpdate.Add("admin", "false");

            Clients.Group(chatReq.Requested_by).UpdateChat(enduserUpdate);
        }
    }
}