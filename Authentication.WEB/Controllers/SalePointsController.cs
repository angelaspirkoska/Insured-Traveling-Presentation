﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.Models;

namespace InsuredTraveling.Controllers
{
    public class SalePointsController : Controller
    {
        // GET: SalePoints
        public ActionResult Index()
        {    
            return View();
        }
    }
}