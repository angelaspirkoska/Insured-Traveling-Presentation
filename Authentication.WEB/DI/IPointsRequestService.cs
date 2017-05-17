using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public interface IPointsRequestService
    {
        void AddPoints_Request(PointsRequestModel RequestModel);
        void ChangeFlagStatus(bool flag);
    }
}