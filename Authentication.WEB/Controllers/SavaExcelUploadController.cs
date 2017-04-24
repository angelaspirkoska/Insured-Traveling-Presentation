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
using Newtonsoft.Json.Linq;
using InsuredTraveling.Filters;
using Authentication.WEB.Services;
using System.Net.Mail;

namespace InsuredTraveling.Controllers
{
    public class SavaExcelUploadController : Controller
    {
        private readonly ISavaPoliciesService _sp;
        private readonly ISava_setupService _savaSetupService;
        private readonly IUserService _userService;
        private readonly IPolicyService _ps;
        private readonly IRolesService _rs;

        public SavaExcelUploadController(ISavaPoliciesService sp,
                                         ISava_setupService savaSetupService,
                                         IUserService userService, IPolicyService ps, IRolesService rs)
        {
            _sp = sp;
            _savaSetupService = savaSetupService;
            _userService = userService;
            _ps = ps;
            _rs = rs;
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
                foreach (SavaPolicyModel policy in model)
                {
                    AuthRepository _repo = new AuthRepository();
                    RoleAuthorize _roleAuthorize = new RoleAuthorize();
                    
                    _sp.AddSavaPolicy(policy);
                    _sp.SumDiscountPoints(policy.SSN_policyHolder, policy.discount_points);
                    _userService.UpdatePremiumSum(policy.SSN_policyHolder, policy.premium);
                    //var Sava_admin =  _savaSetupService.GetActiveSavaSetup();
                    var Sava_admin = _savaSetupService.GetLast();
                    float? UserSumPremiums = _userService.GetUserSumofPremiums(policy.SSN_policyHolder);
                    
                    if (UserSumPremiums == null)
                    {
                        UserSumPremiums = 0;
                    }
                    var PolicyUser = _userService.GetUserBySSN(policy.SSN_policyHolder);
                    
                    if (_roleAuthorize.IsUser("Sava_normal", PolicyUser.UserName))
                    {
                        string userRole = "Сава+ корисник на Сава осигурување";
                        SendSavaEmail(PolicyUser.Email, PolicyUser.FirstName, PolicyUser.LastName, userRole);
                        _repo.AddUserToRole(PolicyUser.Id, "Sava_Sport+");
                    }
                    if (!_roleAuthorize.IsUser("Sava_Sport_VIP", PolicyUser.UserName))
                    {
                        if (Sava_admin.vip_sum <= UserSumPremiums)
                        {
                            string userRole = "VIP корисник на Сава осигурување";
                            SendSavaEmail(PolicyUser.Email, PolicyUser.FirstName, PolicyUser.LastName, userRole);
                            _repo.AddUserToRole(PolicyUser.Id, "Sava_Sport_VIP");
                        }
                    }
                    
                }
             
            }
            catch
            {
                ViewBag.Success = false;
            }
            return View(model);
        }
       
        private void SendSavaEmail(string email, string ime, string prezime, string userRole)
        {
            
            var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/EmailHeaderSuccess.png"));
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            string mailBody = string.Format(@"   
                     <div style='margin-left:20px'>
                     <img style='width:700px' src=""cid:{0}"" />
                     <p> <b> Почитувани, </b></p>                  
                     <br />"   + ime + " " + prezime +  
                 "<br /> <br />" + "Вие станавте " + userRole + "  <br />  <b>Честитки. </b> </div><br />"
            , inlineLogo.ContentId);

            var view = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
            view.LinkedResources.Add(inlineLogo);
            MailService mailService = new MailService(email);
            mailService.setSubject("Промена на корисничи привилегии");
            mailService.setBodyText(email, true);
            mailService.AlternativeViews(view);
            mailService.sendMail();
        }

        [HttpPost]
        [Route("CheckPolicyNumberExist")]
        public JObject CheckPolicyNumberExist(string policy_number)
        {
            var JSONObject = new JObject();
            JSONObject.Add("Error", "");
            if (policy_number == null || policy_number == "")
            {
                JSONObject.Add("Error", "Internal error: Empty Input");
                throw new Exception("Internal error: Empty Input");
            }
            try
            {
                var result = _sp.GetSavaPolicyIdByPolicyNumber(policy_number);
                if (result == null)
                {

                    JSONObject.Add("Exists", "false");
                }
                else
                {
                    JSONObject.Add("Exists", "true");
                }

            }
            catch(Exception ex)
            {
                JSONObject.Add("Error", "Internal error");
                throw new Exception("Internal error",ex);
            }
            
            return JSONObject;
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
                policyModel.SSN_insured = (dr.ItemArray[2]).ToString();
                policyModel.SSN_policyHolder = (dr.ItemArray[3]).ToString();
                string tempExpiry = (dr.ItemArray[4]).ToString();


                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;
                DateTime startDate1 = String.IsNullOrEmpty(tempExpiry) ? new DateTime() : DateTime.ParseExact(tempExpiry, dateTimeFormat, CultureInfo.InvariantCulture);

                policyModel.expiry_date = startDate1;
                policyModel.premium = Convert.ToInt32(dr.ItemArray[5]);
                policyModel.email_seller = (dr.ItemArray[6]).ToString();
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