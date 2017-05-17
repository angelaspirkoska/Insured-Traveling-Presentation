using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class PointsRequestModel
    {
        public string id_user { get; set; }
        public string ssn { get; set; }
        public string policy_id { get; set; }
        public DateTime DateCreated { get; set; }
        public bool flag { get; set; }

    }
}