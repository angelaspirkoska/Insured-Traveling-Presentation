﻿namespace Authentication.WEB.Models
{
    public class PaymentSuccessModel
    {
        public string clientId { get; set; }
        public string amount { get; set; }
        public string oid { get; set; }
        public string mdStatus { get; set; }
        public string AuthCode { get; set; }
        public string TransId { get; set; }
        public int PolicyNumber { get; set; }
        public string Policy_Number { get; set; }
        public string email { get; set; }
    }
}
