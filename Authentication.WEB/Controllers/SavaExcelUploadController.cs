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
    [SessionExpire]
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
                    policy.email_seller = " ";

                    _sp.AddSavaPolicy(policy);
                    _sp.SumDiscountPoints(policy.SSN_policyHolder, policy.discount_points,policy.date_created);
                    _userService.UpdatePremiumSum(policy.SSN_policyHolder, policy.premium, policy.date_created);
                    //var Sava_admin =  _savaSetupService.GetActiveSavaSetup();
                    var Sava_admin = _savaSetupService.GetLast();

                    float? UserSumPremiums = _userService.GetUserSumofPremiums(policy.SSN_policyHolder);
                    if (UserSumPremiums == null)
                    {
                        UserSumPremiums = 0;
                    }
                    var PolicyUser = _userService.GetUserBySSN(policy.SSN_policyHolder);

                    //if (_roleAuthorize.IsUser("Sava_normal", PolicyUser.UserName))
                    //{
                    //    string userRole = "Сава+ корисник на Сава осигурување";
                    //    SendSavaEmail(PolicyUser.Email, PolicyUser.FirstName, PolicyUser.LastName, userRole);
                    //    _repo.AddUserToRole(PolicyUser.Id, "Sava_Sport+");
                    //}

                    if (_roleAuthorize.IsUser("Sava_Sport+", PolicyUser.UserName))
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
            catch(Exception ex)
            {
                ViewBag.Success = false;
                return RedirectToAction("Index", model);
            }
            ViewBag.Success = true;
            return RedirectToAction("Index",model);
        }

        private void SendSavaEmail(string email, string ime, string prezime, string userRole)
        {

            var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/EmailHeaderSuccess.png"));
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            string mailBody = string.Format(@"   
                     <div style='margin-left:20px'>
                     <img style='width:700px' src=""cid:{0}"" />
                     <p> <b> Почитувани, </b></p>                  
                     <br />" + ime + " " + prezime +
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
            catch (Exception ex)
            {
                JSONObject.Add("Error", "Internal error");
                throw new Exception("Internal error", ex);
            }

            return JSONObject;
        }
        [HttpPost]
        [Route("CheckSSN")]
        public JObject CheckSSN( string SSN_Holder)
        {

            var JSONObject = new JObject();
            JSONObject.Add("Error", "");
            if ( SSN_Holder == null || SSN_Holder == "")
            {
                JSONObject.Add("Error", "Internal error: Empty Input");
                throw new Exception("Internal error: Empty Input");
            }
            try
            {
                ValidationService ValService = new ValidationService();

               // bool SSN_result = ValService.validateSSN_Advanced(SSN);
                bool SSN_Holder_result = ValService.validateSSN_Advanced(SSN_Holder);
                string subSSN = "";
                string subSSN_Holder = "";

               
               
                    //if (SSN.Length == 13)
                    //{
                    //    subSSN = SSN.Substring(10, 3);
                    //}

                    if (SSN_Holder.Length == 13)
                    {
                        subSSN_Holder = SSN_Holder.Substring(10, 3);
                    }

                    //if (SSN_result || subSSN == "000")
                    //{
                    //    JSONObject.Add("SSN", "true");                     
                    //}
                    //else
                    //{
                    //    JSONObject.Add("SSN", "false");                       
                    //}

                    if (SSN_Holder_result || subSSN_Holder == "000")
                    {
                        JSONObject.Add("SSN_Holder", "true");
                       
                    }
                    else
                    {
                        JSONObject.Add("SSN_Holder", "false");
                        
                    }

                if (!CheckIfPolicyHolderExist(SSN_Holder))
                {
                    
                    JSONObject.Add("Error2", "1");

                    
                }
            }
            catch (Exception ex)
            {
                JSONObject.Add("Error", "Internal error");
                throw new Exception("Internal error", ex);
            }
        
        
            return JSONObject;

        }

        public bool CheckIfPolicyHolderExist(string SSN_Holder)
        {
            if (_userService.GetUserBySSN(SSN_Holder) == null)
            {
                return false;
            }
            return true;
        }

        public FileResult SavaDocumentDownload()
        {
            try
            {

                string fullpath = Path.Combine(Server.MapPath("~/ExcelSavaTemplate/"), "SAVA_Policy_Template_Final.xlsx");
                return File(fullpath, "ExcelSavaTemplate/xlsx", "SavaExcelPolicy_Template.xlsx");
            } 
            catch (Exception ex)
            {
                throw new Exception("");
            }
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
            var savaSetup = _savaSetupService.GetLast();
            var percentage = savaSetup != null ? savaSetup.points_percentage : 1;
            DataTable dt = GetDataTableFromSpreadsheet(model.MyExcelFile.InputStream, false);
            
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SavaPolicyModel policyModel = new SavaPolicyModel();

                    var policy_number = dr.ItemArray[0].ToString();
                    var SSN_insured = " ";
                    var SSN_policyHolder = dr.ItemArray[1].ToString();
                    var dateCreated = dr.ItemArray[2].ToString();
                    var expiry_date = dr.ItemArray[3].ToString();
                    var premium = dr.ItemArray[4].ToString();
                    //var discount_points = dr.ItemArray[5].ToString();

                    if (policy_number != "" && SSN_insured != "" && SSN_policyHolder != "" && expiry_date != "" && premium != "" && dateCreated != "")
                    {
                        
                        policyModel.policy_number = (dr.ItemArray[0].ToString());
                        policyModel.SSN_insured = " ";
                        policyModel.SSN_policyHolder = (dr.ItemArray[1]).ToString();
                        string tempDateCreated = (dr.ItemArray[2]).ToString();
                       
                        try
                        {
                            double DoubletempDateCreated = double.Parse(tempDateCreated, CultureInfo.InvariantCulture);
                            DateTime conv1 = DateTime.FromOADate(DoubletempDateCreated);
                            var tempDateCreatedReplaced = conv1.Date.ToString("dd'/'MM'/'yyyy");
                            var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                            var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;
                            DateTime startDate1 = String.IsNullOrEmpty(tempDateCreatedReplaced) ? new DateTime() : DateTime.ParseExact(tempDateCreatedReplaced, dateTimeFormat, CultureInfo.InvariantCulture);

                            policyModel.date_created = startDate1;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage += " Невалиден датум на почеток.";

                        }
                        string tempExpiry = (dr.ItemArray[3]).ToString();
                        try
                        {
                            var DoubletempExpiry = double.Parse(tempExpiry, CultureInfo.InvariantCulture);
                            DateTime conv = DateTime.FromOADate(DoubletempExpiry);
                            var tempx = conv.ToString("dd'/'MM'/'yyyy");
                            var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                            var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;
                            DateTime startDate1 = String.IsNullOrEmpty(tempx) ? new DateTime() : DateTime.ParseExact(tempx, dateTimeFormat, CultureInfo.InvariantCulture);

                            policyModel.expiry_date = startDate1;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage += " Невалиден датум на истекување.";
                            
                        }

                        try
                        {
                            policyModel.premium = Math.Abs(Convert.ToInt32(dr.ItemArray[4]));
                            
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage += " Невалидна премија ";
                        }
                        
                        policyModel.discount_points = Convert.ToInt32(Math.Round(policyModel.premium / percentage));
                        model.TableRows.Add(policyModel);
                    }
                }
            }
            catch ( Exception ex)
            {
                ViewBag.ErrorMessage += " невалидна полиса";
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
                        if (row.Descendants<Cell>().ElementAt(i).CellValue != null)
                        {
                            tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                        }
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

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && cell.DataType.Value != null)
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