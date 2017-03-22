using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
   public interface IDiscountService
    {
        void AddDiscount(DiscountModel discount);

        void DeleteDiscount(int id);

        List<discount_codes> GetAllDiscounts();

        discount_codes GetLast();

    }

   


}
