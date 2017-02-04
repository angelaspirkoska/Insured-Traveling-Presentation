using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.SignalR.DTOs
{
    public class SendMessageMobileDTO : BaseRequestIdDTO
    {
        public string From { get; set; }
        public string Message { get; set; }
    }
}