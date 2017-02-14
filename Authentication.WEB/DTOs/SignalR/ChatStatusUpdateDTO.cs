using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DTOs.SignalR
{
    public class ChatStatusUpdateDTO : BaseRequestIdDTO
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}