using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DTOs.SignalR
{
    public class LastMessagesDTO : MessageDTO
    {
        public DateTime Timestamp { get; set; }
        public string ChatWith { get; set; }
        public int MessageId { get; set; }
    }
}