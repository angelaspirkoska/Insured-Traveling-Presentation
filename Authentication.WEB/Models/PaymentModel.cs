using InsuredTraveling;
using System;

namespace Authentication.WEB.Models
{
    public class PaymentModel
    {
        public travel_policy Pat { get; set; }
        public string clientId { get; set; }
        public string amount { get; set; }
        public string oid { get; set; }
        public string okUrl { get; set; }
        public string failUrl { get; set; }

        public string taksit { get; set; }
        public string rnd { get; set; }
        public string hashstr { get; set; }
        public string currency { get; set; }
        public string storetype { get; set; }
        public string refreshtime { get; set; }

        public string transactionType { get; set; }
        public string storekey { get; set; }
        public byte[] hashbytes { get; set; }
        public byte[] inputbytes { get; set; }
        public string instalmentCount { get; set; }
        public String hash { get; set; }
        public string Password { get; internal set; }
        public string Username { get; internal set; }
    }
}
