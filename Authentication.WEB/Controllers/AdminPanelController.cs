using InsuredTraveling.Filters;
using System;
using System.Web.Mvc;
using InsuredTraveling.DI;
using static InsuredTraveling.Models.AdminPanel;
using InsuredTraveling.Models;
using System.Web;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.Controllers
{
    //[RoleAuthorize(roles: "Admin")]
    [CustomActionFilter]
    [SessionExpire]
    public class AdminPanelController : Controller
    {
        private IRolesService _rs;
        private IOkSetupService _okss;
        private IUserService _us;
        private IDiscountService _ds;


        public AdminPanelController(IRolesService rs, IOkSetupService okss,IUserService us, IDiscountService ds)
        {
            _rs = rs;
            _okss = okss;
            _us = us;
            _ds = ds;         
        }

        [HttpGet]
        public ActionResult Index()
        {
            var roles = _rs.GetAllRoles();
            var ok_setup = _okss.GetAllOkSetups();

            ViewBag.Ok_setup = ok_setup;
            ViewBag.Roles = roles;

            //View Bag Discount
            var discount = _ds.GetAllDiscounts();
            ViewBag.Discount = discount;
            ViewBag.TabIndex = "1";
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
                ViewBag.TabIndex = "1";
                return View("Index");
            }
            var discount = _ds.GetAllDiscounts();
            ViewBag.Discount = discount;
            ViewBag.AddRoleMsg = "NOk";
            ViewBag.TabIndex = "1";
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
            var discount = _ds.GetAllDiscounts();
            ViewBag.Discount = discount;
            ViewBag.Roles = roles;
            ViewBag.Ok_setup = ok_setup;
          
            ViewBag.TabIndex = "2";
            return View("Index");
        }

        [Route("Delete_OK_setup_Record")]
        public ActionResult Delete_OK_setup_Record(int id)
        {
            _okss.DeleteOkSetup(id);
            var roles = _rs.GetAllRoles();
            var ok_setup = _okss.GetAllOkSetups();
            var discount = _ds.GetAllDiscounts();
            ViewBag.Discount = discount;
            ViewBag.Ok_setup = ok_setup;
            ViewBag.Roles = roles;
            ViewBag.TabIndex = "2";
            return View("Index");
        }

        [HttpPost]
        [Route("AddDiscount")]
        public ActionResult AddDiscount(AdminPanelModel dis)
        {
            var ok_setup = _okss.GetAllOkSetups();
            var roles = _rs.GetAllRoles();
            ViewBag.Roles = roles;
            ViewBag.Ok_setup = ok_setup;
            var discount = _ds.GetAllDiscounts();
            ViewBag.Discount = discount;

            if (ModelState.IsValid)
            {
                DiscountModel dm = new DiscountModel();

                dm.Discount_Coef = dis.Discount_Coef;
                dm.Discount_Name = dis.Discount_Name;
                dm.Start_Date = dis.Start_Date;
                dm.End_Date = dis.End_Date;

                try
                {
                    _ds.AddDiscount(dm);
                }
                catch (Exception ex)
                {
                    ViewBag.AddOk_SetupMsg = ex.ToString();
                }               
            }else
            {
                ViewBag.Message = "Registration failed";
            }

            ViewBag.TabIndex = "3";
            return View("Index");
        }

        [HttpGet]
        [Route("DeleteDiscount")]
        public ActionResult DeleteDiscount(int id)
        {
            var ok_setup = _okss.GetAllOkSetups();
            var roles = _rs.GetAllRoles();
            ViewBag.Roles = roles;
            ViewBag.Ok_setup = ok_setup;
            var discount = _ds.GetAllDiscounts();
            ViewBag.Discount = discount;
            ViewBag.TabIndex = "3";
            try
            {
                _ds.DeleteDiscount(id);

            }
            catch (Exception ex)
            {
                ViewBag.AddOk_SetupMsg = ex.ToString();
            }

            return View("Index");
        }

        [HttpPost]
        [Route("ConfigureRatingEngine")]
        public ActionResult ConfigureRatingEngine(HttpPostedFileBase excelConfigFile)
        {
            if (excelConfigFile != null && excelConfigFile.ContentLength > 0)
            {
                var path = @"~/ExcelConfig/config_file.xlsx";
                path = System.Web.HttpContext.Current.Server.MapPath(path);
                excelConfigFile.SaveAs(path);
                ExcelFileViewModel e = new ExcelFileViewModel();
                e.Path = path;
                return View("PolicyForm", e);
            }
            return View("Index");
        }
    }
}