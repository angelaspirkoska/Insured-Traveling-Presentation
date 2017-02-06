namespace InsuredTraveling.DTOs
{
    //TODO use once all chat hub DTOs have been refactored with PascalCase
    public class AcknowledgeAdminDTO : BaseRequestIdDTO
    {
        public string EndUser { get; set; }
    }
    public class AcknowledgeEndUserDTO : BaseRequestIdDTO
    {
        public string Admin { get; set; }
    }
}