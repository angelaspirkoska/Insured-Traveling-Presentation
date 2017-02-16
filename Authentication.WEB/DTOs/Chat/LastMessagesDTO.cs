using System;

namespace InsuredTraveling.DTOs
{
    public class LastMessagesDTO : MessageDTO
    {
        public DateTime Timestamp { get; set; }
        public string ChatWith { get; set; }
        public int MessageId { get; set; }
    }
}