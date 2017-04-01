﻿using InsuredTraveling.Filters;
using System;
using System.Web.Mvc;
using InsuredTraveling.DI;
using static InsuredTraveling.Models.AdminPanel;
using InsuredTraveling.Models;

namespace InsuredTraveling.Controllers
{
    [RoleAuthorize(roles: "Admin")]
    [SessionExpire]
    public class AdminPanelController : Controller
    {
        private IRolesService _rs;
        private IOkSetupService _okss;
        private IUserService _us;

        public AdminPanelController(IRolesService rs, IOkSetupService okss,IUserService us)
        {
            _rs = rs;
            _okss = okss;
            _us = us;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var roles = _rs.GetAllRoles();
            var ok_setup = _okss.GetAllOkSetups();

            ViewBag.Ok_setup = ok_setup;
            ViewBag.Roles = roles;

            return View();
        }


        [HttpPost]
        [Route("AddRole")]
        public ActionResult AddRole(Roles r)
        {
            AuthRepository _repo = new AuthRepository();
            var result = _repo.AddRole(r);
            var roles = _rs.GetAllRoles();
            var ok_setup = _okss.GetAllOkSetups();

            ViewBag.Roles = roles;
            ViewBag.Ok_setup = ok_setup;

            if (result.Succeeded)
            {
                ViewBag.AddRoleMsg = "Ok";

                return View("Index");
            }
            ViewBag.AddRoleMsg = "NOk";
            return View("Index");
        }

        [HttpPost]
        [Route("AddOK_setup")]
        public ActionResult AddOK_setup(Ok_SetupModel ok)
        {
            ViewBag.AddOk_SetupMsg = "OK";
            try {
                ok.Created_By = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
                ok.Created_Date = DateTime.UtcNow;

                _okss.AddOkSetup(ok);
            }
            catch (Exception ex)
            {
                ViewBag.AddOk_SetupMsg = ex.ToString();
            }


            var ok_setup = _okss.GetAllOkSetups();

            var roles = _rs.GetAllRoles();

            ViewBag.Roles = roles;
            ViewBag.Ok_setup = ok_setup;

            return View("Index");
        }

        [Route("Delete_OK_setup_Record")]
        public ActionResult Delete_OK_setup_Record(int id)
        {
            _okss.DeleteOkSetup(id);
            var roles = _rs.GetAllRoles();
            var ok_setup = _okss.GetAllOkSetups();

            ViewBag.Ok_setup = ok_setup;
            ViewBag.Roles = roles;
            return View("Index");
        }
    }
}