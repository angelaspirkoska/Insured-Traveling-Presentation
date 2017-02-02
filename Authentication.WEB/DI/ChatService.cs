using InsuredTraveling.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class ChatService : IChatService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public chat_requests ChatRequest(int requestID)
        {
            return _db.chat_requests.Where(x => x.ID == requestID).SingleOrDefault();
        }

        public List<chat_requests> GetChatsAdmin(string username)
        {
            return _db.chat_requests.Where(x => x.Accepted_by.Equals(username)).ToList();
        }
        public List<chat_requests> GetChatsEndUser(string username)
        {
            return _db.chat_requests.Where(x => x.Requested_by.Equals(username)).ToList();
        }

        public List<message> LastTenMessagesByRequest(int requestID)
        {
            //Take(10)
            return _db.messages.Where(x => x.ConversationID == requestID).OrderByDescending(dt => dt.ID).Take(10).ToList();


        }

        public List<message> NextTenMessagesByRequest(int requestID, int MessageID)
        {
            return _db.messages.Where(x => x.ConversationID == requestID && x.ID < MessageID).OrderBy(dt => dt.Timestamp).Take(10).ToList();
        }
    }
}