using InsuredTraveling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.WEB.Models
{
    public class PolicyInfoList
    {
        public IQueryable<p_zem> zemjaNaPatuvanjeList { get; set; }
        public IQueryable<p_zemja_na_patuvanje> fransizaList { get; set; }
        public IQueryable<p_zemja_na_patuvanje> vidPolisaList { get; set; }
        public IQueryable<p_doplatoci> doplatokList { get; set; }
    }
}
