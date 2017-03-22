using AutoMapper;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class DiscountService : IDiscountService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public void AddDiscount(DiscountModel discount)
        {
            discount_codes discount2 = _db.discount_codes.Create();
            discount2 = Mapper.Map<DiscountModel, discount_codes>(discount);
            _db.discount_codes.Add(discount2);
            _db.SaveChanges();
        }

        public void DeleteDiscount(int id)
        {
            var o = _db.discount_codes.Where(x => x.ID == id);
            if (o != null)
            {
                _db.discount_codes.Remove(o.ToArray().First());
                _db.SaveChanges();
            }

        }

        public List<discount_codes> GetAllDiscounts()
        {
            return _db.discount_codes.ToList();
        }

        public discount_codes GetLast()
        {
            return _db.discount_codes.ToArray().Last();
        }
    }
}