using System.Collections.Generic;

namespace InsuredTraveling.DTOs
{
    public class MessageRequestsDTO
    {
        public int RequestNumber { get; set; }
        public List<RequestDTO> Requests { get; set; }
    }
}