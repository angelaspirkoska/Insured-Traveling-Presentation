using InsuredTraveling.DI;
using InsuredTraveling.FormBuilder;
using InsuredTraveling.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class ConfigurationPolicyController : Controller
    {
        private readonly IConfigPolicyTypeService _configPolicyTypeService;
        private readonly IExcelConfigService _excelConfigService;
        private readonly IConfigPolicyService _configPolicyService;
        private readonly IConfigPolicyValuesService _configPolicyValuesService;

        public ConfigurationPolicyController(IConfigPolicyTypeService configPolicyTypeService, 
                                             IExcelConfigService excelConfigService,
                                             IConfigPolicyService configPolicyService,
                                             IConfigPolicyValuesService configPolicyValuesService)
        {
            _configPolicyTypeService = configPolicyTypeService;
            _excelConfigService = excelConfigService;
            _configPolicyService = configPolicyService;
            _configPolicyValuesService = configPolicyValuesService;
        }
        // GET: ConfigurationPolicy
        public ActionResult Index()
        {
            var model = new ConfigurationPolicyViewModel();
            var policyTypes = _configPolicyTypeService.GetAllActivePolicyTypes();
            ViewBag.PolicyTypes = GetPolicyTypeSelectedList(policyTypes);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ConfigurationPolicyViewModel model)
        {
            if(model.PolicyTypeID != 0)
            {
                var configPolicyType = _configPolicyTypeService.GetConfigPolicyTypeByID(model.PolicyTypeID);
                if(configPolicyType !=  null)
                {
                    var excelConfig = _excelConfigService.GetExcelConfigByPolicyTypeId(configPolicyType.ID);
                    var policyFormModel = new PolicyFormViewModel();
                    policyFormModel.ExcelID = excelConfig.ID;
                    policyFormModel.ExcelPath = excelConfig.excel_path;
                    return RedirectToAction("PolicyForm", "ConfigurationPolicy", policyFormModel);
                }
                return View(model);
            }
            else
                return View(model);
        }

        public List<SelectListItem> GetPolicyTypeSelectedList(List<config_policy_type> policyTypes)
        {
            List<SelectListItem> policyTypesSelectedList = new List<SelectListItem>();
            foreach (var policyType in policyTypes)
            {
                policyTypesSelectedList.Add(new SelectListItem() { Value = policyType.ID.ToString(), Text = policyType.policy_type_name });
            }
            return policyTypesSelectedList;
        }

        public ActionResult PolicyForm(PolicyFormViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult PolicyForm(int? excelId, FormCollection formCollection)
        {
            if (excelId.HasValue)
            {
                var premium = DatabaseCommands.CalculatePremium((int)excelId, formCollection);
                ViewBag.CalculatedPremium = premium;

                var configPolicy = DatabaseCommands.InsertPolicyConfigData(1, premium);
                configPolicy.IDPolicy = _configPolicyService.AddConfigPolicy(configPolicy);

                var configPolicyValues = DatabaseCommands.InsertConfigPolicyValues((int)excelId, formCollection, configPolicy);
                var isSuccessful = _configPolicyValuesService.AddConfigPolicyValues(configPolicyValues);

                return View("PolicyPremium");
            }
            return new HttpStatusCodeResult(500, "Something went wrong");
        }
    }
}