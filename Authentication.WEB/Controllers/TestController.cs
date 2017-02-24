using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.DI;
using System.Collections.Generic;
using InsuredTraveling.Helpers;
using InsuredTraveling.ViewModels;
using InsuredTraveling.Filters;
using AutoMapper;
using System.IO;
using System.Linq;
using InsuredTraveling.ExcelReader;
using OfficeOpenXml;

namespace InsuredTraveling.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase excelConfigFile)
        {

            if (excelConfigFile != null && excelConfigFile.ContentLength > 0)
            {
                var path = @"~/ExcelConfig/config_file.xlsx";
                path = System.Web.HttpContext.Current.Server.MapPath(path);
                excelConfigFile.SaveAs(path);

                ExcelFileViewModel e = new ExcelFileViewModel();
                e.Path = path;

                return View("PolicyForm",e);
            }

            return View();
        }
    }
}