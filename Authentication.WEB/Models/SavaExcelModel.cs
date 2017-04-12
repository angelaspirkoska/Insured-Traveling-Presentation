using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class SavaExcelModel
    {
        [Required]
        public HttpPostedFileBase MyExcelFile { get; set; }

        public string MSExcelTable { get; set; }

        public List<SavaPolicyModel> TableRows = new List<SavaPolicyModel> {
        };
        public SavaPolicyModel SavaModel {get; set;}
         
    }
}