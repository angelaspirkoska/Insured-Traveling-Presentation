﻿using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class LoginController : Controller
    {
        public static LoginUser l_user;
        // GET: Login
        [HttpPost]
        public ActionResult Index(LoginUser user)
        {
            l_user.Username = user.Username;
            l_user.Password = user.Password;

            if (ModelState.IsValid)
            {
                return LoginUser();
            }  
             
            return View();
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.Request.Cookies["token"] != null)
            {
                HttpCookie c = HttpContext.Request.Cookies["token"];
                c.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Response.Cookies.Remove("token");
                Response.Cookies.Clear();
                HttpContext.Response.Cookies.Set(c);
            }

            l_user = new LoginUser();
            return View();
        }

        public JsonResult LoginUser()
        {
            var jsonData = Json(l_user, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void ReciveToken(string token)
        {
            HttpCookie c = new HttpCookie("token");
            c["t"] = token;
            HttpContext.Response.Cookies.Add(c);
        }
    }
}