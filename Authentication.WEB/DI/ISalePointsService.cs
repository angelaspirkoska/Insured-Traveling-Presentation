using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public interface ISalePointsService
    {
        void AddSalePoint(SalePoints salePoints);
    }
}
