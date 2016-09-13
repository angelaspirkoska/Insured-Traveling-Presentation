using InsuredTraveling;
using System.Linq;

namespace Authentication.WEB.Models
{
    public class PolicyInfoList
    {
        public IQueryable<country> zemjaNaPatuvanjeList { get; set; }
        public IQueryable<p_zemja_na_patuvanje> fransizaList { get; set; }
        public IQueryable<p_zemja_na_patuvanje> vidPolisaList { get; set; }
        public IQueryable<p_doplatoci> doplatokList { get; set; }
    }
}
