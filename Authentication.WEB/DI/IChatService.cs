using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IChatService
    {
        chat_requests ChatRequest(int requestID);

        List<message> LastTenMessagesByRequest(int requestID);

        List<message> NextTenMessagesByRequest(int requestID, int MessageID);

        List<chat_requests> GetChatsAdmin(string username);
        List<chat_requests> GetChatsEndUser(string username);
    }
}