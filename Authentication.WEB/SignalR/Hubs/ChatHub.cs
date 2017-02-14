using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using InsuredTraveling.Filters;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using InsuredTraveling.DTOs;
using InsuredTraveling.DTOs.SignalR;
using System.Collections.Generic;

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
            List<LastMessagesDTO> lastMessages = new List<LastMessagesDTO>();
            if (r.IsUser("admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, username);

                Clients.Group(username).MessageRequest(GetActiveRequests());
                lastMessages = GetLastMessages(username, true);
                
            }
            else if (r.IsUser("end user"))
            {
                Groups.Add(Context.ConnectionId, username);
                lastMessages = GetLastMessages(username, false);
            }

            Clients.Group(username).ActiveMessages(lastMessages);

            return base.OnConnected();
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
                    Clients.Group("Admins").MessageRequest(GetActiveRequests());
                }
                
               
            }
            else
            {
                requestId = listRequestsByUser.Last().ID;
            }
            BaseRequestIdDTO RequestIdDTO = new BaseRequestIdDTO();
            RequestIdDTO.RequestId = requestId;
            Clients.Group(username).RequestId(RequestIdDTO);
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
            Clients.Group("Admins").MessageRequest(GetActiveRequests());

        }
        public void SendMessage(MessageDTO message)
        {
            var from = Context.User.Identity.Name;
            RoleAuthorize r = new RoleAuthorize();
            bool isAdmin = false;
            if (r.IsUser("admin"))
                isAdmin = true;

            var messageDTO = new MessageDTO
            {
                From = from,
                Message = message.Message,
                RequestId = message.RequestId,
                Admin = isAdmin
            };
            Clients.Group(message.To).ReceiveMessage(messageDTO);
            SaveMessage(messageDTO);
        }
        public void DiscardMessage(BaseRequestIdDTO requestIdDTO)
        {
            ChatStatusUpdateDTO adminResponse = new ChatStatusUpdateDTO();
            var requestId = requestIdDTO.RequestId;
            var request = _db.chat_requests.Where(x => x.ID == requestId).SingleOrDefault();
            if (request.fnol_created.HasValue && request.fnol_created.Value == false)
            {
                request.discarded = true;
                _db.SaveChanges();

                ChatStatusUpdateDTO enduserResponse = new ChatStatusUpdateDTO();
                enduserResponse.RequestId = request.ID;
                enduserResponse.Message = "Your request has been discarded. First notice of loss is not created.";
                Clients.Group(request.Requested_by).DiscardedMessage(enduserResponse);
                adminResponse.RequestId = request.ID;
                adminResponse.Success = true;
                adminResponse.Message = "You discarded the request. First notice of loss is not created.";
            }
            else
            {
                adminResponse.RequestId = request.ID;
                adminResponse.Success = false;
                adminResponse.Message = "You can't discard this chat. First notice of loss was created.";         
            }            
            Clients.Group(request.Accepted_by).Discarded(adminResponse);
        }
        public void CreateFnol(BaseRequestIdDTO requestIdDTO)
        {
            var request = _db.chat_requests.Where(x => x.ID == requestIdDTO.RequestId).SingleOrDefault();
            ChatStatusUpdateDTO adminResponse = new ChatStatusUpdateDTO();

            if (!request.discarded)
            {
                request.fnol_created = true;
                _db.SaveChanges();

                //TODO create fnol in db

                ChatStatusUpdateDTO enduserResponse = new ChatStatusUpdateDTO();
                enduserResponse.RequestId = request.ID;
                enduserResponse.Message = "First notice of loss was created successfully.";      
                Clients.Group(request.Requested_by).FnolCreatedMessage(enduserResponse);
                adminResponse.RequestId = request.ID;
                adminResponse.Success = true;
                adminResponse.Message = "First notice of loss was created successfully.";

            }
            else
            {
                adminResponse.RequestId = request.ID;
                adminResponse.Success = false;
                adminResponse.Message = "This chat was already discarded.";
            }

            Clients.Group(request.Accepted_by).FnolCreated(adminResponse);

        }

        private void SaveMessage(MessageDTO messageDTO)
        {
            int requestId = messageDTO.RequestId;
            string fromUsername = messageDTO.From;
            string textMessage = messageDTO.Message;           
            message message = new message();
            message.ConversationID = requestId;
            message.Text = textMessage;
            message.Timestamp = DateTime.Now;
            message.from_username = fromUsername;
            var m = _db.messages.Add(message);
            _db.SaveChangesAsync();
        }

        private MessageRequestsDTO GetActiveRequests()
        {
            var responseList = _db.chat_requests.Where(x => x.Accepted == false).Select(x => new RequestDTO
            {
                RequestedBy = x.Requested_by,
                Timestamp = x.Datetime_request.ToString()
            }).ToList();

            MessageRequestsDTO messageRequests = new MessageRequestsDTO
            {
                RequestNumber = responseList.Count,
                Requests = responseList
            };

            return messageRequests;
        }

        private List<LastMessagesDTO> GetLastMessages(string username, bool isAdmin)
        {
            List<chat_requests> chatsActive = new List<chat_requests>();
            if(isAdmin)
            {
                chatsActive = _db.chat_requests.Where(x => x.Accepted == true && x.Accepted_by.Equals(username) && x.discarded == false
                                           && x.fnol_created == false).Take(5).ToList();
            } else
            {
                chatsActive = _db.chat_requests.Where(x => x.Accepted == true && x.Requested_by.Equals(username) && x.discarded == false
                                            && x.fnol_created == false).Take(5).ToList();
            }
           

            List<LastMessagesDTO> lastMessagesDTO = new List<LastMessagesDTO>();

            foreach (chat_requests chat in chatsActive)
            {
                LastMessagesDTO messageLast = null;
                if (isAdmin)
                {
                    messageLast = chat.messages.Where(x => x.ConversationID == chat.ID).OrderByDescending(x => x.Timestamp).Select(x => new LastMessagesDTO
                    {
                        From = x.from_username,
                        Message = x.Text,
                        Admin = isAdmin,
                        Timestamp = x.Timestamp,
                        ChatWith = chat.Requested_by,
                        MessageId = x.ID,
                        RequestId = chat.ID
                    }).FirstOrDefault();
                }
                else
                {
                    messageLast = chat.messages.Where(x => x.ConversationID == chat.ID).OrderByDescending(x => x.Timestamp).Select(x => new LastMessagesDTO
                    {
                        From = x.from_username,
                        Message = x.Text,
                        Admin = isAdmin,
                        Timestamp = x.Timestamp,
                        ChatWith = chat.Accepted_by,
                        MessageId = x.ID,
                        RequestId = chat.ID
                    }).FirstOrDefault();
                }

                if (messageLast != null)
                {
                    lastMessagesDTO.Add(messageLast);
                }

            }

            return lastMessagesDTO;
        }

    }
}