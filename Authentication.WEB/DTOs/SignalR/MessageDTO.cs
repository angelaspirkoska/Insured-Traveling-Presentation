using System;

namespace InsuredTraveling.DTOs
{
    public class MessageDTO : BaseRequestIdDTO
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public bool Admin{ get; set; }

    }
}