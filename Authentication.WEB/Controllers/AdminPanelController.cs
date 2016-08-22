using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static InsuredTraveling.Models.AdminPanel;

namespace InsuredTraveling.Controllers
{   
    [Authorize]
    public class AdminPanelController : Controller
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        [HttpGet]
        public ActionResult Index()
        {
            var roles = _db.aspnetroles.ToArray();

            ArrayList roles2 = new ArrayList();

            for (int i = 0; i < roles.Count(); i++)
            {
                Roles r2 = new Roles();
                r2.Id = i + 1;
                r2.Name = roles[i].Name;
                roles2.Add(r2);
            }

            ViewBag.Roles = roles2;
            return View();
        }

        [HttpPost]
        [Route("AddRole")]
        public ActionResult AddRole(Roles r)
        {
            AuthRepository _repo = new AuthRepository();
            var result = _repo.AddRole(r);
            var roles = _db.aspnetroles.ToArray();
            ArrayList roles2 = new ArrayList();
            for(int i = 0; i < roles.Count(); i++)
            {
                Roles r2 = new Roles();
                r2.Id = i + 1;
                r2.Name = roles[i].Name;
                roles2.Add(r2);
            }
            ViewBag.Roles = roles2;

            if (result.Succeeded)
            {
                ViewBag.AddRoleMsg = "Ok";
               
                return View("Index");
            }
            ViewBag.AddRoleMsg = "NOk";
            return View("Index");
        }
    }
}