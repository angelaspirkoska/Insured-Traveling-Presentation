using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class PointsRequestModel
    {
        public string id { get; set; }
        public string id_user { get; set; }
        public string ssn { get; set; }
        public string policy_id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }
        public bool flag { get; set; }

    }
}