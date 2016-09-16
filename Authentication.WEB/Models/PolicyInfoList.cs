using InsuredTraveling;
using System.Linq;

namespace Authentication.WEB.Models
{
    public class PolicyInfoList
    {
        public IQueryable<country> countries { get; set; }
        public IQueryable<retaining_risk> franchises { get; set; }
        public IQueryable<policy_type> policies { get; set; }
        public IQueryable<additional_charge> doplatokList { get; set; }
    }
}
