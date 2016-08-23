using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static InsuredTraveling.Models.AdminPanel;

namespace InsuredTraveling.Controllers
{   
    public class AdminPanelController : Controller
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        [HttpGet]
        public ActionResult Index()
        {
                var roles = _db.aspnetroles.ToArray();
                var ok_setup = _db.ok_setup.ToArray();

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
            var roles = _db.aspnetroles.ToArray();
            var ok_setup = _db.ok_setup.ToArray();

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
        public ActionResult AddOK_setup(ok_setup ok)
        {
            ViewBag.AddOk_SetupMsg = "OK";
            try
            {
                _db.ok_setup.Add(ok);
                _db.SaveChanges();
            }
            catch(Exception ex)
            {
                ViewBag.AddOk_SetupMsg = ex.ToString();
            }
            

            var ok_setup = _db.ok_setup.ToArray();
            var roles = _db.aspnetroles.ToArray();

            ViewBag.Roles = roles;
            ViewBag.Ok_setup = ok_setup;
            return View("Index");
        }

        [Route("Delete_OK_setup_Record")]
        public ActionResult Delete_OK_setup_Record(int id)
        {
            var o = _db.ok_setup.Where(x => x.id == id);
            if(o!=null)
            {
                _db.ok_setup.Remove(o.ToArray().First());
                _db.SaveChanges();
            }
            var roles = _db.aspnetroles.ToArray();
            var ok_setup = _db.ok_setup.ToArray();

            ViewBag.Ok_setup = ok_setup;
            ViewBag.Roles = roles;
            return View("Index");
        }
    }
}