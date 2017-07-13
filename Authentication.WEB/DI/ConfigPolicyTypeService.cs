using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.Models;
using  System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using InsuredTraveling.FormBuilder;
using OfficeOpenXml;

namespace InsuredTraveling.DI
{
    public class ConfigPolicyTypeService : IConfigPolicyTypeService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddConfigPolicyType(config_policy_type configPolicyType)
        {
            try
            {
                _db.config_policy_type.Add(configPolicyType);
                _db.SaveChanges();
                return configPolicyType.ID;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<config_policy_type> GetAllActivePolicyTypes()
        {
            return _db.config_policy_type.Where(x => x.status == true).ToList();
        }

        public IQueryable<SelectListItem> GetAllActivePolicyTypesDropdown()
        {
            return _db.config_policy_type.Where(x => x.status == true).Select(p => new SelectListItem
            {
                Text = p.policy_type_name.ToString(),
                Value = p.ID.ToString()
            });
        }

        public config_policy_type GetConfigPolicyTypeByID(int id)
        {
            return _db.config_policy_type.Where(x => x.ID == id).FirstOrDefault();
        }

        public config_policy_type GetConfigPolicyTypeByTypeFromID(int id)
        {
            return _db.config_policy_type.Where(x => x.typeFrom == id).OrderByDescending(x => x.ID).FirstOrDefault();
        }

        public List<config_policy_type> GetTypeByName(string TypeName)
        {
            List<config_policy_type> ListType = new List<config_policy_type>();
            if (TypeName == null || TypeName == " " || TypeName == "undefined")
            {
                return _db.config_policy_type.ToList();
            }
            else
            {
                return _db.config_policy_type.Where(x => x.policy_type_name == TypeName).ToList();
            }


        }

        public List<config_policy_type> GetTypeID(string TypeName)
        {
            List<config_policy_type> ListType = new List<config_policy_type>();
            if (TypeName == null || TypeName == " " || TypeName == "undefined")
            {
                return _db.config_policy_type.ToList();
            }
            else
            {
                return _db.config_policy_type.Where(x => x.policy_type_name == TypeName).ToList();
            }
        }

        public List<config_policy_type> GetAllPolicies()
        {
            return _db.config_policy_type.ToList();
        }

        public int EditConfigPolicyType(ConfigPolicyTypeModel editedPolicy)
        {
            var datetimeformat = ConfigurationManager.AppSettings["DateFormat"];
            datetimeformat = datetimeformat.Replace("yy", "yyyy");
            try
            {
                config_policy_type editPolicy = GetConfigPolicyTypeByID(editedPolicy.id);
                editPolicy.policy_type_name = editedPolicy.name;
                editPolicy.policy_effective_date = DateTime.ParseExact(editedPolicy.startDate, datetimeformat,
                    CultureInfo.InvariantCulture);
                editPolicy.policy_expiry_date =
                    DateTime.ParseExact(editedPolicy.endDate, datetimeformat, CultureInfo.InvariantCulture);
                editPolicy.status = editedPolicy.status;
                _db.SaveChanges();
                return editPolicy.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int AddNewPolicyTypeVersion(config_policy_type selectedPolicy)
        {
            try
            {
                _db.config_policy_type.Add(selectedPolicy);
                _db.SaveChanges();
                return selectedPolicy.ID;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public int NumberOfColums(string path)
        {
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];

            var i = 1;
            while (worksheet.Cells[1, i].Value != null)
            {
                i++;
            }
            return i;
        }

        public bool CompleteRows(string path)
        {
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
            bool isCommplete = true;
            int i = 1;
            for (i = 2; worksheet.Cells[i, 1].Value != null; i++)
            {
                if ((worksheet.Cells[i, 4].Value != null & worksheet.Cells[i, 5].Value != null &
                     worksheet.Cells[i, 6].Value != null & worksheet.Cells[i, 7].Value != null &
                     worksheet.Cells[i, 8].Value != null & worksheet.Cells[i, 9].Value != null) |
                    (worksheet.Cells[i, 12].Value != null & worksheet.Cells[i, 14].Value != null))
                {
                    isCommplete = true;
                }
                else
                {
                    isCommplete = false;
                }
            }
            return isCommplete;
        }

        public bool RatingIndicatorValidation(String path)
        {
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
            bool isCommplete = true;
            int i = 1;
            for (i = 2; worksheet.Cells[i, 10].Value != null; i++) { 
                if ((worksheet.Cells[i, 10].Value.ToString() == "1" & worksheet.Cells[i, 12].Value != null) |
                    (worksheet.Cells[i, 10].Value.ToString() == "0" & worksheet.Cells[i, 12].Value == null))
                {
                    isCommplete = true;
                }
                else
                {
                    isCommplete = false;
                    break;
                }
            }
            return isCommplete;
        }

        public bool FunctionsValidation(String path)
        {
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["ConfigurationSetup"];
            int row = 1;
            bool flag = true;
            for (row = 2; worksheet.Cells[row, 15].Value != null; row++)
            {
                if (!worksheet.Cells[row, 15].Value.ToString().Equals("empty"))
                {
                    var formula = worksheet.Cells[row, 15].Formula;
                    string functionName = worksheet.Cells[row, 16 - 1].Value.ToString();
                    if (formula.ToUpper().StartsWith("DGET") | formula.ToUpper().StartsWith("IF") |
                        formula.ToUpper().StartsWith("ROUND") | formula.ToUpper().StartsWith("EXACT") | Regex.IsMatch(formula.ToUpper(),(@"^[a-zA-Z0-9]+$")))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }

            }
            return flag;
        }
    }
}