using AutoMapper;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class PointsRequestService : IPointsRequestService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        public void AddPoints_Request(PointsRequestModel RequestModel)
        {
            points_requests Points_request = _db.points_requests.Create();
            Points_request = Mapper.Map<PointsRequestModel, points_requests>(RequestModel);
            _db.points_requests.Add(Points_request);
            _db.SaveChanges();
        }
        public void ChangeFlagStatus(bool flag)
        {
           
            var request = _db.points_requests.Last();
            request.flag = flag;
           
            _db.points_requests.Attach(request);
            _db.Entry(request).State = EntityState.Modified;
            _db.SaveChanges();
        }

    }
}