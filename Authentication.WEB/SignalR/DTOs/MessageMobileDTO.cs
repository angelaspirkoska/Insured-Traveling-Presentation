﻿namespace InsuredTraveling.SignalR.DTOs
{
    public class MessageMobileDTO : BaseRequestIdDTO
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
}