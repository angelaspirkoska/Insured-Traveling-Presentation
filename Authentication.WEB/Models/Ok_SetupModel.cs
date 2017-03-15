using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class Ok_SetupModel
    {
   
        public int id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public int Sms_Code_Seconds { get; set; }
      
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public int NumberOfAttempts { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public int NumberOfNews { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public System.TimeSpan NotificationTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public int NumberOfLastMsg { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string InsuranceCompany { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]

        public int VersionNumber { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public string Modified_Date { get; set; }
        public string Modified_By { get; set; }
        public sbyte SSNValidationActive { get; set; }
    }
}