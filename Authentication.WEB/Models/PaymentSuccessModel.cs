using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.WEB.Models
{
    public class PaymentSuccessModel
    {
        public string clientId { get; set; }
        public string amount { get; set; }
        public string oid { get; set; }
        public string mdStatus { get; set; }
        public string AuthCode { get; set; }
        public string TransId { get; set; }
    }
}
