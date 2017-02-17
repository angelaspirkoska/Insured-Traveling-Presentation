using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class Request
    {
        public int requestId { get; set; }
        public int messageId { get; set; }

        public string username { get; set; }
    }
}