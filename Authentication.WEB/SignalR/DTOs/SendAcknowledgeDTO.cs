using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.SignalR.DTOs
{
    //TODO use once all chat hub DTOs have been refactored with PascalCase
    public class SendAcknowledgeToAdminDTO : BaseRequestIdDTO
    {
        public string EndUser { get; set; }
    }
    public class SendAcknowledgeToEndUserDTO : BaseRequestIdDTO
    {
        public string Admin { get; set; }
    }
}