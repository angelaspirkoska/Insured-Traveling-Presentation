using Authentication.WEB.Services;
using AutoMapper;
using InsuredTraveling.DI;
using InsuredTraveling.Models;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Hosting;

namespace InsuredTraveling.Schedulers
{
    [DisallowConcurrentExecution]
    public class EmailJob : IJob
    {
        private ISavaVoucherService _SavaVoucher ;
        private IPointsRequestService _PointsRequest;
        private IUserService _us;
       
       

        public EmailJob(ISavaVoucherService SavaVoucher, IPointsRequestService PointsRequest, IUserService us)
        {
            _SavaVoucher = SavaVoucher;
            _PointsRequest = PointsRequest;
            _us = us;
          

        }
        public void Execute(IJobExecutionContext context)
        {
       
           // var dateCreated = "29/05/2017"; // treba dejttajm now

            var mailService = new MailService("atanasovski46@gmail.com");
            var emailBody = "Dear Ivan now is " + DateTime.Now;
            
            mailService.setSubject("Shceduler");

            DateTime createdDate = DateTime.Now;
            string dateCreated = DateTime.Now.ToString("dd/MM/yyyy");
            try
            {
               
            var dateTime = ConfigurationManager.AppSettings["DateFormat"];
            var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;
            
                createdDate = String.IsNullOrEmpty(dateCreated) ? DateTime.UtcNow.Date : DateTime.ParseExact(dateCreated, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            catch(Exception ex)
            {

            }
           

            //User List
            var ListOfUsers = GetAllRegisteredUsersCreatedToday(dateCreated);
            if (ListOfUsers.Count() != 0)
            {
                string UserFilePath = GetRegisteredUsersSearchResultsAsExcelDocument(ListOfUsers);
                Attachment ListOfUsersExcel = new Attachment(UserFilePath, MediaTypeNames.Application.Octet);
                mailService.attach(ListOfUsersExcel);
            }else
            {
                emailBody += "Нема нови корисници";
            }

            //PolicyRequest - Validate account
            var ListOfPolicyRequest = GetAllPolicyRequest(dateCreated);
            if (ListOfPolicyRequest.Count() != 0)
            {
                string PolicyRequestFilePath = GetPointsRequestSearchResultsAsExcelDocument(ListOfPolicyRequest);
                Attachment ListOfPolicyRequestExcel = new Attachment(PolicyRequestFilePath, MediaTypeNames.Application.Octet);
                mailService.attach(ListOfPolicyRequestExcel);

            }
            else
            {
                emailBody += "Нема нови побарувања за валидација";
            }

            //Points Used 
            var ListOfPointsUsed = GetAllUsedPoints(dateCreated);
            if (ListOfPointsUsed.Count() != 0)
            {
                string PointsUsedFilePath = GetPointsUsedSearchResultsAsExcelDocument(ListOfPointsUsed);
                Attachment PointsUsedtExcel = new Attachment(PointsUsedFilePath, MediaTypeNames.Application.Octet);
                mailService.attach(PointsUsedtExcel);
            }else
            {
                emailBody += "Нема нови потрошени поени";
            }



            
            mailService.setBodyText(emailBody);
            mailService.sendMail();

        }
        public List<SearchRegisteredUser> GetAllRegisteredUsersCreatedToday(string dateCreated)
        {
           
            var dateTime = ConfigurationManager.AppSettings["DateFormat"];
            var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;

            DateTime createdDate = String.IsNullOrEmpty(dateCreated) ? DateTime.UtcNow.Date : DateTime.ParseExact(dateCreated, dateTimeFormat, CultureInfo.InvariantCulture);
            List<aspnetuser> data = new List<aspnetuser>();
            data = _us.GetAllUsersCreatedTodayForSavaAdmin(createdDate);
            var jsonObject = new JObject();
            var searchModel = data.Select(Mapper.Map<aspnetuser, SearchRegisteredUser>).ToList();

            return searchModel;
        }

        public List<PointsRequestModel> GetAllPolicyRequest(string dateCreated)
        {
            var dateTime = ConfigurationManager.AppSettings["DateFormat"];
            var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;

            DateTime createdDate = String.IsNullOrEmpty(dateCreated) ? DateTime.UtcNow.Date : DateTime.ParseExact(dateCreated, dateTimeFormat, CultureInfo.InvariantCulture);


            List<points_requests> data = new List<points_requests>();
            data = _PointsRequest.GetPointsRequest(createdDate);

            var jsonObject = new JObject();
            var searchModel = data.Select(Mapper.Map<points_requests, PointsRequestModel>).ToList();

            return searchModel;
        }

        public List<SavaVoucherModel> GetAllUsedPoints(string dateCreated)
        {
            var dateTime = ConfigurationManager.AppSettings["DateFormat"];
            var dateTimeFormat = dateTime != null && (dateTime.Contains("yy") && !dateTime.Contains("yyyy")) ? dateTime.Replace("yy", "yyyy") : dateTime;

            DateTime createdDate = String.IsNullOrEmpty(dateCreated) ? DateTime.UtcNow.Date : DateTime.ParseExact(dateCreated, dateTimeFormat, CultureInfo.InvariantCulture);


            List<sava_voucher> data = new List<sava_voucher>();
            data = _SavaVoucher.GetPointsRequest(createdDate);

            var jsonObject = new JObject();
            var searchModel = data.Select(Mapper.Map<sava_voucher, SavaVoucherModel>).ToList();

            return searchModel;
        }

        public string GetRegisteredUsersSearchResultsAsExcelDocument(List<SearchRegisteredUser> users)
        {
            if (users == null || !users.Any())
                return null;

            //string username = System.Web.HttpContext.Current.User.Identity.Name;
            //var logUser = _us.GetUserIdByUsername(username);

            string fileName = "ReportRegistredUsers_" + /*+ DateTime.Now.ToShortDateString()+*/ "_" + Guid.NewGuid().ToString() + ".xlsx";
            var path = @"~/ExcelSearchResults/RegisteredUsers/" + fileName;
            path = HostingEnvironment.MapPath(path); //System.Web.HttpContext.Current.Server.MapPath(path);
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                fileName = "ReportRegistredUsers" + DateTime.Now.ToShortDateString() + Guid.NewGuid().ToString();
                path = @"~/ExcelSearchResults/RegisteredUsers/" + fileName;
                newFile = new FileInfo(path);
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(InsuredTraveling.Resource.RegisteredUserExcelTitle);
                //Add the headers
                worksheet.Cells[1, 1].Value = InsuredTraveling.Resource.SearchTable_UserName;
                worksheet.Cells[1, 2].Value = InsuredTraveling.Resource.Policy_HolderSSN;
                worksheet.Cells[1, 3].Value = InsuredTraveling.Resource.SearchTable_FirstName;
                worksheet.Cells[1, 4].Value = InsuredTraveling.Resource.SearchTable_LastName;
                worksheet.Cells[1, 5].Value = InsuredTraveling.Resource.SearchTable_Email;
                worksheet.Cells[1, 6].Value = InsuredTraveling.Resource.SearchTable_RoleName;
                worksheet.Cells[1, 7].Value = InsuredTraveling.Resource.SearchTable_CreatedOn;
                worksheet.Cells[1, 8].Value = InsuredTraveling.Resource.SearchTable_ActiveInactiveUser;
                worksheet.Cells[1, 9].Value = InsuredTraveling.Resource.SearchTable_UserPoints;

                int counter = 2;

                foreach (SearchRegisteredUser user in users)
                {
                    worksheet.Cells[counter, 1].Value = user.Username;
                    worksheet.Cells[counter, 2].Value = user.embg;
                    worksheet.Cells[counter, 3].Value = user.FirstName;
                    worksheet.Cells[counter, 4].Value = user.LastName;
                    worksheet.Cells[counter, 5].Value = user.Email;
                    worksheet.Cells[counter, 6].Value = user.RoleName;
                    worksheet.Cells[counter, 7].Value = user.CreatedOn;
                    worksheet.Cells[counter, 8].Value = user.ActiveInactive;
                    worksheet.Cells[counter, 9].Value = user.Points == null ? "0" : user.Points;
                    counter++;
                }

                worksheet.View.PageLayoutView = true;

                // set some document properties
                package.Workbook.Properties.Title = InsuredTraveling.Resource.RegisteredUserExcelTitle;
                package.Workbook.Properties.Author = "SavaAdmin";
                package.Workbook.Properties.Comments = "";

                // set some extended property values
                package.Workbook.Properties.Company = "Sava Osiguruvanje";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "SavaAdmin");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");
                // save our new workbook and we are done!
                try
                {
                    package.Save();
                }
                catch(Exception Ex)
                {

                }

            }

            //JObject jsonPath = new JObject();
            //jsonPath.Add("path", path);
            return path;
            //return jsonPath;
            // return File(path, "application/vnd.ms-excel", "PoliciesSearchResults.xlsx");
        }
        public string GetPointsRequestSearchResultsAsExcelDocument(List<PointsRequestModel> requests)
        {
            if (requests == null || !requests.Any())
                return null;

            //string username = System.Web.HttpContext.Current.User.Identity.Name;
            //var logUser = _us.GetUserIdByUsername(username);

            string fileName = "ReportPolicyValidationRequests" /*+ DateTime.Now.ToShortDateString()*/ + "_" + Guid.NewGuid().ToString() + ".xlsx";
            var path = @"~/ExcelSearchResults/RegisteredUsers/" + fileName;
            path = HostingEnvironment.MapPath(path); //System.Web.HttpContext.Current.Server.MapPath(path);
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                fileName = "ReportPolicyValidationRequests_" + DateTime.Now.ToShortDateString() +"_"+ Guid.NewGuid().ToString();
                path = @"~/ExcelSearchResults/RegisteredUsers/" + fileName;
                newFile = new FileInfo(path);
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(InsuredTraveling.Resource.RegisteredUserExcelTitle);
                //Add the headers
                worksheet.Cells[1, 1].Value = InsuredTraveling.Resource.SearchRequestNumber;
                worksheet.Cells[1, 2].Value = InsuredTraveling.Resource.FNOL_Number;
                worksheet.Cells[1, 3].Value = InsuredTraveling.Resource.Sava_ExcelSSN_Holder;
                worksheet.Cells[1, 4].Value = InsuredTraveling.Resource.SearchTable_CreatedOn;
                worksheet.Cells[1, 5].Value = InsuredTraveling.Resource.SearcPolicyRequestStatus;
              

                int counter = 2;

                foreach (PointsRequestModel request in requests)
                {
                    worksheet.Cells[counter, 1].Value = request.id;
                    worksheet.Cells[counter, 2].Value = request.policy_id;
                    worksheet.Cells[counter, 3].Value = request.ssn;
                    worksheet.Cells[counter, 4].Value = request.DateCreated.ToString("dd/MM/yyyy HH:mm:ss ");
                    worksheet.Cells[counter, 5].Value = request.flag;
                    counter++;
                }

                worksheet.View.PageLayoutView = true;

                // set some document properties
                package.Workbook.Properties.Title = InsuredTraveling.Resource.RegisteredUserExcelTitle;
                package.Workbook.Properties.Author = "SavaScheduleService";
                package.Workbook.Properties.Comments = "";

                // set some extended property values
                package.Workbook.Properties.Company = " ";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "SavaScheduleService");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");
                // save our new workbook and we are done!
                try
                {
                    package.Save();
                }
                catch (Exception Ex)
                {

                }

            }

            //JObject jsonPath = new JObject();
            //jsonPath.Add("path", path);
            return path;
            //return jsonPath;
            // return File(path, "application/vnd.ms-excel", "PoliciesSearchResults.xlsx");
        }
        public string GetPointsUsedSearchResultsAsExcelDocument(List<SavaVoucherModel> voucher)
        {
            if (voucher == null || !voucher.Any())
                return null;

            //string username = System.Web.HttpContext.Current.User.Identity.Name;
            //var logUser = _us.GetUserIdByUsername(username);

            string fileName = "ReportUsedPointsRequests_" + /*DateTime.Now.ToShortDateString() +*/ "_" + Guid.NewGuid().ToString() + ".xlsx";
            var path = @"~/ExcelSearchResults/RegisteredUsers/" + fileName;
            path = HostingEnvironment.MapPath(path); //System.Web.HttpContext.Current.Server.MapPath(path);
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                fileName = "ReportUsedPointsRequests" +/* DateTime.Now.ToShortDateString() +*/ "_" + Guid.NewGuid().ToString();
                path = @"~/ExcelSearchResults/RegisteredUsers/" + fileName;
                newFile = new FileInfo(path);
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(InsuredTraveling.Resource.RegisteredUserExcelTitle);
                //Add the headers
                worksheet.Cells[1, 1].Value = InsuredTraveling.Resource.SearchUsedPointsNumber;
                worksheet.Cells[1, 2].Value = InsuredTraveling.Resource.Sava_ExcelSSN_Holder;
                worksheet.Cells[1, 3].Value = InsuredTraveling.Resource.Sava_ExcelSeller_id;
                worksheet.Cells[1, 4].Value = InsuredTraveling.Resource.SearchUsedPointsNumber;
                worksheet.Cells[1, 5].Value = InsuredTraveling.Resource.SearchTable_CreatedOn;


                int counter = 2;

                foreach (SavaVoucherModel request in voucher)
                {
                    worksheet.Cells[counter, 1].Value = request.id;
                    worksheet.Cells[counter, 2].Value = request.id_policyHolder;
                    worksheet.Cells[counter, 3].Value = request.id_seller;
                    worksheet.Cells[counter, 4].Value = request.points_used;
                    worksheet.Cells[counter, 5].Value = request.timestamp.ToString("dd/MM/yyyy HH:mm:ss ");
                    counter++;
                }

                worksheet.View.PageLayoutView = true;

                // set some document properties
                package.Workbook.Properties.Title = InsuredTraveling.Resource.RegisteredUserExcelTitle;
                package.Workbook.Properties.Author = "SavaScheduleService";
                package.Workbook.Properties.Comments = "";

                // set some extended property values
                package.Workbook.Properties.Company = " ";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "SavaScheduleService");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");
                // save our new workbook and we are done!
                package.Save();

            }

            //JObject jsonPath = new JObject();
            //jsonPath.Add("path", path);
            return path;
            //return jsonPath;
            // return File(path, "application/vnd.ms-excel", "PoliciesSearchResults.xlsx");
        }



    }
}