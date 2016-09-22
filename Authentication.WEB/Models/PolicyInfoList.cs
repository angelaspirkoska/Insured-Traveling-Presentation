using System.Linq;

namespace InsuredTraveling.Models
{
    public class PolicyInfoList
    {
        public IQueryable<country> countries { get; set; }
        public IQueryable<retaining_risk> franchises { get; set; }
        public IQueryable<policy_type> policies { get; set; }
        public IQueryable<additional_charge> additional_charges { get; set; }
    }
}
