namespace InsuredTraveling.DTOs
{
    public class MessageMobileDTO : BaseRequestIdDTO
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public bool Admin{ get; set; }
    }
}