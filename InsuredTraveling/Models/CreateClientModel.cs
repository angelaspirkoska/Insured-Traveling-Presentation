using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.WEB.Models
{
    public class CreateClientModel
    {
        public string NameSurename { get; set; }
        public string email { get; set; }
        public string Number { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Passport { get; set; }
        public string EMBG { get; set; }
    }
}
