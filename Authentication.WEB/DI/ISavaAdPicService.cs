using InsuredTraveling.ViewModels;
using System.Collections.Generic;

namespace InsuredTraveling.DI
{
    public interface ISavaAdPicService
    {
        List<sava_ad_pictures> GetAdPictures();
    }
}