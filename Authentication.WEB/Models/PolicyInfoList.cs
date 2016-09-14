using InsuredTraveling;
using System.Linq;

namespace Authentication.WEB.Models
{
    public class PolicyInfoList
    {
        public IQueryable<country> zemjaNaPatuvanjeList { get; set; }
        public IQueryable<retaining_risk_value> FranchiseList { get; set; }
        public IQueryable<policy_type> vidPolisaList { get; set; }
        public IQueryable<p_doplatoci> doplatokList { get; set; }
    }
}
