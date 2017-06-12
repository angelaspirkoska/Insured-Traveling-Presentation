using System;
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
using Newtonsoft.Json.Linq;

namespace InsuredTraveling.Controllers
{
    [SessionExpireAttribute]
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
            if (ModelState.IsValid)
            {
                try
                {
                    _sps.AddSalePoint(SPointModel);
                    ViewBag.Success = "Success";
                }
                catch (Exception ex)
                {
                    ViewBag.Success = "Failure";
                }
            }
            return View("Index");
        }
 

    [HttpGet]
        [Route("GetSalePoints")]
        public JObject GetSalePoints()
        {
            var data = _sps.GetSalePoints();
            var jsonObject = new JObject();
            var array = JArray.FromObject(data.ToArray());
            jsonObject.Add("data", array);
            return jsonObject;
        }

    }
}