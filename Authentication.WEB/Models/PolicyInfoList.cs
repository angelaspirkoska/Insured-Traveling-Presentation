using InsuredTraveling;
using System.Linq;

namespace Authentication.WEB.Models
{
    public class PolicyInfoList
    {
        public IQueryable<country> countries { get; set; }
        public IQueryable<retaining_risk_value> franchises { get; set; }
        public IQueryable<policy_type> policies { get; set; }
        public IQueryable<p_doplatoci> doplatokList { get; set; }
    }
}
