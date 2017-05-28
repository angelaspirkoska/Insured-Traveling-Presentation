using InsuredTraveling.Models;
using System;
using System.Collections.Generic;

namespace InsuredTraveling.DI
{
    public interface IPointsRequestService
    {
        void AddPoints_Request(PointsRequestModel RequestModel);
        void ChangeFlagStatus(bool flag);
        List<points_requests> GetPointsRequest(DateTime createdDate);
    }
}