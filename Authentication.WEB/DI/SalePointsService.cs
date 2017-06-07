using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public class SalePointsService : ISalePointsService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

       
        public void AddSalePoint(SalePoints salePoints)
        {
            var mappedSalePoint = Mapper.Map<SalePoints,sava_sale_points>(salePoints);
            _db.sava_sale_points.Add(mappedSalePoint);
            _db.SaveChanges();
        }

        public List<SalePoints> GetSalePoints()
        {

            IQueryable<sava_sale_points> saleP = _db.sava_sale_points;
            var mappedSalePoint = saleP.Select(Mapper.Map<sava_sale_points, SalePoints>).ToList();
            
            return mappedSalePoint;
        }

    }
}