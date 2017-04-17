using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.Models;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using InsuredTraveling.DI;
using System.Configuration;
using System.Globalization;

namespace InsuredTraveling.Controllers
{
    public class SavaExcelUploadController : Controller
    {
        private readonly ISavaPoliciesService _sp;
        private readonly ISava_setupService _savaSetupService;
        private readonly IUserService _userService;
        
        public SavaExcelUploadController(ISavaPoliciesService sp,
                                         ISava_setupService savaSetupService,
                                         IUserService userService)
        {
            _sp = sp;
            _savaSetupService = savaSetupService;
            _userService = userService;
        }
        // GET: SavaExcelUpload
        public ActionResult Index()
        {
            var model = new SavaExcelModel();
            ViewBag.IsRepost = 0;
            ViewBag.ErrorMessage = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult SavePolicy(List<SavaPolicyModel> model)
        {
            ViewBag.Success = true;
            try
            {
                _sp.AddSavaPolicyList(model);
            }
            catch
            {
                ViewBag.Success = false;
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SavaExcelModel model)
        {
            ViewBag.IsRepost = 0;
            ViewBag.ErrorMessage = "";
           
            SavaExcelModel SavaExcel = new SavaExcelModel();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!model.MyExcelFile.FileName.EndsWith("xlsx"))
            {
                ViewBag.ErrorMessage = Resource.Sava_ExcelFileError;
                
                return View(model);
            }

            ViewBag.IsRepost = 1;
            var savaSetup = _savaSetupService.GetActiveSavaSetup();
            var percentage = savaSetup != null ? savaSetup.points_percentage : 1;
            DataTable dt = GetDataTableFromSpreadsheet(model.MyExcelFile.InputStream, false);

            foreach (DataRow dr in dt.Rows)
            {
                SavaPolicyModel policyModel = new SavaPolicyModel();
                policyModel.policy_number = Convert.ToInt32(dr.ItemArray[0]);
                policyModel.SSN_insured = (dr.ItemArray[1]).ToString();
                policyModel.SSN_policyHolder = (dr.ItemArray[2]).ToString();
                string tempExpiry = (dr.ItemArray[3]).ToString();

                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;
                DateTime startDate1 = String.IsNullOrEmpty(tempExpiry) ? new DateTime() : DateTime.ParseExact(tempExpiry, dateTimeFormat, CultureInfo.InvariantCulture);

                policyModel.expiry_date = startDate1;
                policyModel.premium = Convert.ToInt32(dr.ItemArray[4]);
                policyModel.email_seller = (dr.ItemArray[5]).ToString();
                policyModel.discount_points = Convert.ToInt32(Math.Round(policyModel.premium / percentage));
                model.TableRows.Add(policyModel);
            }
            return View(model);
        }
        public static DataTable GetDataTableFromSpreadsheet(Stream MyExcelStream, bool ReadOnly)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(sDoc, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                    }
                    
                    dt.Rows.Add(tempRow);

                }
            }

            dt.Rows.RemoveAt(0);
            return dt;
        }
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static string ConvertDataTableToHTMLTable(DataTable dt)
        {
            string ret = "";
            ret = "<table id=" + (char)34 + "tblExcel" + (char)34 + ">";
            ret += "<tr>";
            foreach (DataColumn col in dt.Columns)
            {
                ret += "<td class=" + (char)34 + "tdColumnHeader" + (char)34 + ">" + col.ColumnName + "</td>";
            }
            ret += "</tr>";
            foreach (DataRow row in dt.Rows)
            {
                ret += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ret += "<td class=" + (char)34 + "tdCellData" + (char)34 + ">" + row[i].ToString() + "</td>";
                }
                ret += "</tr>";
            }
            ret += "</table>";
            return ret;
        }
    }
}