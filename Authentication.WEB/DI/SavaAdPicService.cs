using AutoMapper;
using InsuredTraveling.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class SavaAdPicService : ISavaAdPicService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        public List<sava_ad_pictures> GetAdPictures()
        {
            List<sava_ad_pictures> AdPics = _db.sava_ad_pictures.ToList();
            
            return AdPics;
        }
    }
}