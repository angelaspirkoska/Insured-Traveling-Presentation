using System.Linq;
using System.Web.Mvc;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.Controllers;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using InsuredTraveling.Models;
using AutoMapper;

namespace InsuredTraveling.Controllers
{
    public class SalePointsController : Controller
    {
        public ISalePointsService _sps;
        public SalePointsController(ISalePointsService sps)
        {
            _sps = sps;
        }
        // GET: SalePoints
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSalePoint(SalePoints SPointModel)
        {
           _sps.AddSalePoint(SPointModel);
            return View();
        }
    }


}