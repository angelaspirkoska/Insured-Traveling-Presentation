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
        public void ChangeFlagStatus(string PolicyNo)
        {

            points_requests request = _db.points_requests.Where(x => (x.policy_id == PolicyNo )).FirstOrDefault();
            request.flag = true; 
           
            _db.points_requests.Attach(request);
            _db.Entry(request).State = EntityState.Modified;
            _db.SaveChanges();
        }

       
         public List<points_requests> GetPointsRequest(DateTime createdDate)
        {
            DateTime startDate = createdDate.Date;
            DateTime endDate = startDate.AddDays(1).AddTicks(-1);
            var ListPolicyRequest =  _db.points_requests.Where(x => ( x.timestamp  >= startDate && x.timestamp <= endDate)).ToList();
            
            return ListPolicyRequest;
        }

    }
}