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

    }
}