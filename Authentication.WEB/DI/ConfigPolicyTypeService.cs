using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.Models;
using  System.Configuration;
namespace InsuredTraveling.DI
{
    public class ConfigPolicyTypeService : IConfigPolicyTypeService
    {
        InsuredTravelingEntity  _db = new InsuredTravelingEntity();

        public int AddConfigPolicyType(config_policy_type configPolicyType)
        {
            try
            {
                _db.config_policy_type.Add(configPolicyType);
                _db.SaveChanges();
                return configPolicyType.ID;
            }
            catch(Exception e)
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
        public int UpdatePolicy(ConfigPolicyTypeModel editedPolicy )
        {
            int result = -1;
            try
            {
                config_policy_type editedPolicyDb = GetConfigPolicyTypeByID(editedPolicy.id);
                editedPolicyDb.policy_type_name = editedPolicy.name;
                editedPolicyDb.policy_effective_date = DateTime.ParseExact(editedPolicy.startDate, ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture);
                editedPolicyDb.policy_expiry_date = DateTime.ParseExact(editedPolicy.endDate, ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture);
                editedPolicyDb.status = editedPolicy.status;              
                result = _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}