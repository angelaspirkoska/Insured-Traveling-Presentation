using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using InsuredTraveling.Filters;
using System.Diagnostics;
using InsuredTraveling.DTOs;
using System.Collections.Generic;
using System.Web;

namespace InsuredTraveling.Hubs
{
    [Authorize]
    [SessionExpire]
    public class ChatHub : Hub
    {
        readonly InsuredTravelingEntity _db = new InsuredTravelingEntity();

        private string _currentUser = string.Empty;
        private bool _isAdmin = false;

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            RoleAuthorize roleAuthorize = new RoleAuthorize();
            if (System.Web.HttpContext.Current !=null)
            {
                _currentUser = System.Web.HttpContext.Current.User.Identity.Name;
                _isAdmin = roleAuthorize.IsUser("admin");
            }else
            {
                _isAdmin = false;
            }

            return base.OnReconnected();
        }
        public override Task OnConnected()
        {
            RoleAuthorize roleAuthorize = new RoleAuthorize();
            _currentUser = System.Web.HttpContext.Current.User.Identity.Name;
            List<LastMessagesDTO> lastMessages = new List<LastMessagesDTO>();
            _isAdmin = roleAuthorize.IsUser("admin");

            if (_isAdmin)
            {
                Groups.Add(Context.ConnectionId, "Admins");
                Groups.Add(Context.ConnectionId, _currentUser);

                Clients.Group(_currentUser).MessageRequest(GetActiveRequests());
                lastMessages = GetLastMessages(_currentUser, true);
            }
            else
            {
                Groups.Add(Context.ConnectionId, _currentUser);
                lastMessages = GetLastMessages(_currentUser, false);
            }

            Clients.Group(_currentUser).ActiveMessages(lastMessages);

            return base.OnConnected();
        }
        public void SendRequest()
        {
            var username = Context.User.Identity.Name;
            int requestId = 0;
            chat_requests listRequestsByUser = null;

            try
            {
                listRequestsByUser = _db.chat_requests.Where(x => x.Requested_by == username && x.Accepted == false).OrderByDescending(x => x.Datetime_request)?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            if (listRequestsByUser != null)
            {
                requestId = listRequestsByUser.ID;
            }
            else
            {
                var chatRequest = new chat_requests
                {
                    Requested_by = username,
                    fnol_created = false,
                    discarded = false
                };

                try
                {
                    _db.chat_requests.Add(chatRequest);
                    SaveDBChanges();
                    requestId = chatRequest.ID;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error adding/saving request to db: {ex.ToString()}");
                }
                finally
                {
                    Clients.Group("Admins").MessageRequest(GetActiveRequests());
                }
            }

            BaseRequestIdDTO requestIdDTO = new BaseRequestIdDTO();
            requestIdDTO.RequestId = requestId;
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

            SaveDBChanges();
            var endUserResponseDTO = new AcknowledgeEndUserDTO
            {
                RequestId = request.ID,
                Admin = username
            };
            Clients.Group(enduser).SendAcknowledge(endUserResponseDTO);

            var adminResponseDTO = new AcknowledgeAdminDTO
            {
                RequestId = request.ID,
                EndUser = enduser
            };
            Clients.Group(username).ReceiveId(adminResponseDTO);

            Clients.Group("Admins").MessageRequest(GetActiveRequests());
        }

        public void SendMessage(MessageDTO message)
        {
            if (message == null)
                return;
            if (string.IsNullOrWhiteSpace(message.From))
            {
                _currentUser = HttpContext.Current?.User?.Identity?.Name;
                if (string.IsNullOrWhiteSpace(_currentUser))
                    return;
                else
                    message.From = _currentUser;
            }
            Clients.Group(message.To).ReceiveMessage(message);

            SaveMessage(message);
        }

        public void DiscardMessage(BaseRequestIdDTO requestIdDTO)
        {
            StatusUpdateDTO adminResponse = new StatusUpdateDTO();
            var requestId = requestIdDTO.RequestId;
            var request = _db.chat_requests.Where(x => x.ID == requestId).SingleOrDefault();
            if (request.fnol_created.HasValue && request.fnol_created.Value == false)
            {
                request.discarded = true;
                SaveDBChanges();

                StatusUpdateDTO enduserResponse = new StatusUpdateDTO();
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
            StatusUpdateDTO adminResponse = new StatusUpdateDTO();

            if (!request.discarded)
            {
                request.fnol_created = true;
                SaveDBChanges();

                //TODO create fnol in db

                StatusUpdateDTO enduserResponse = new StatusUpdateDTO();
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

            var message = new message();
            message.ConversationID = requestId;
            message.Text = textMessage;
            message.Timestamp = DateTime.Now;
            message.from_username = fromUsername;

            _db.messages.Add(message);
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

            var chatsActiveByRole = isAdmin
                ? _db.chat_requests.Where(x => x.Accepted_by.Equals(username))
                : _db.chat_requests.Where(x => x.Requested_by.Equals(username));

            chatsActive = chatsActiveByRole.Where(x => x.Accepted == true && x.discarded == false
                                           && x.fnol_created == false).OrderByDescending(x => x.ID).Take(5).ToList();

            List<LastMessagesDTO> lastMessagesDTO = new List<LastMessagesDTO>();

            foreach (chat_requests chat in chatsActive)
            {
                var messageLast = chat.messages.Where(x => x.ConversationID == chat.ID).OrderByDescending(x => x.Timestamp).Select(x => new LastMessagesDTO
                {
                    From = x.from_username,
                    Message = x.Text,
                    Admin = isAdmin,
                    Timestamp = x.Timestamp,
                    ChatWith = isAdmin ? chat.Requested_by : chat.Accepted_by,
                    MessageId = x.ID,
                    RequestId = chat.ID
                }).FirstOrDefault();

                if (messageLast != null)
                {
                    lastMessagesDTO.Add(messageLast);
                }
            }
            return lastMessagesDTO;
        }

        private int SaveDBChanges()
        {
            int result = 0;
            try
            {
                result = _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving request to db: {ex.ToString()}");
            }
            return result;
        }
    }
}