using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using InsuredTraveling.DI;
using InsuredTraveling.Models;

namespace InsuredTraveling.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private IRolesService _rs;
        private IUserService _us;

        public UserController(IRolesService rs, IUserService us)
        {
            _rs = rs;
            _us = us;
        }

        public ActionResult Index()
        {
            var genderList = Gender();
            var roles = _rs.GetAll().ToList();
            aspnetuser userEdit = _us.GetUserDataByUsername(System.Web.HttpContext.Current.User.Identity.Name);

            if (userEdit == null)
                return View();

            User userEditModel = Mapper.Map<aspnetuser, User>(userEdit);

            foreach (var role in roles)
            {
                if (role.Selected)
                    role.Selected = false;
                if (role.Text == userEditModel.Role)
                    role.Selected = true;
            }

            foreach (var gender in genderList)
            {
                if (gender.Text == userEditModel.Gender)
                    gender.Selected = true;
            }

            ViewBag.Roles = roles;
            ViewBag.Gender = genderList;


            return View(userEditModel);
        }

        private List<SelectListItem> Gender()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Text = "Female",
                Value = "Female"
            });
            data.Add(new SelectListItem
            {
                Text = "Male",
                Value = "Male"
            });
            data.Add(new SelectListItem
            {
                Text = "Other",
                Value = "Other"
            });
            return data;
        }
    }
}