using System;
using System.Collections.Generic;
using System.Linq;

namespace InsuredTraveling.DTOs.SignalR
{
    public class MessageRequestsDTO
    {
        public int RequestNumber { get; set; }
        public List<RequestDTO> Requests { get; set; }
    }
}