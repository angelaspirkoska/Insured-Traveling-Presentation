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
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
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
        public bool CheckCode(string code)
        {
            List<discount_codes> DiscountCodes = GetAllDiscounts();
            foreach (discount_codes discount in DiscountCodes)
            {
                if (discount.Discount_Name == code && discount.End_Date >= DateTime.Now)
                {
                    return true;
                }

            }

            return false;
        }
        public bool DiscountCodeExist(string name)
        {
            List<discount_codes> DiscountCodes = GetAllDiscounts();
            foreach (discount_codes discount in DiscountCodes)
            {
                if (discount.Discount_Name == name)
                {
                    return true;
                }
               
            }
            return false;
        }
    }
}
