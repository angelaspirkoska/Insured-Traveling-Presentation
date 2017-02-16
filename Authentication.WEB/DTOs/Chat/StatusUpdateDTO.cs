namespace InsuredTraveling.DTOs
{
    public class StatusUpdateDTO : BaseRequestIdDTO
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}