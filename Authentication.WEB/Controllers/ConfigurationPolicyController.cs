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
        private readonly IConfigInsuredService _configInsuredService;
        private readonly IConfigInsuredPolicyService _configInsuredPolicyService;

        public ConfigurationPolicyController(IConfigPolicyTypeService configPolicyTypeService, 
                                             IExcelConfigService excelConfigService,
                                             IConfigPolicyService configPolicyService,
                                             IConfigPolicyValuesService configPolicyValuesService,
                                             IConfigInsuredService configInsuredService,
                                             IConfigInsuredPolicyService configInsuredPolicyService)
        {
            _configPolicyTypeService = configPolicyTypeService;
            _excelConfigService = excelConfigService;
            _configPolicyService = configPolicyService;
            _configPolicyValuesService = configPolicyValuesService;
            _configInsuredService = configInsuredService;
            _configInsuredPolicyService = configInsuredPolicyService;
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
                    int idholder;
                    int idInsured;
                    int idPolicy;
                    var excelConfig = _excelConfigService.GetExcelConfigByPolicyTypeId(configPolicyType.ID);

                    idPolicy = _configPolicyService.AddConfigPolicy(configPolicyType.ID, "no value", model.Start_Date.Value, model.End_Date.Value, false);

                    if(idPolicy != 0)
                    {
                        idholder = _configInsuredService.AddConfigInsured(model.PolicyHolderName, model.PolicyHolderLastName, model.PolicyHolderSSN, model.PolicyHolderPassportNumber_ID, model.PolicyHolderBirthDate.Value, model.PolicyHolderAddress);
                        if (idholder != 0)
                            _configInsuredPolicyService.AddConfigInsuredPolicy(idPolicy, idholder, 1);
                        if (!String.IsNullOrEmpty(model.SSN))
                        {
                            idInsured = _configInsuredService.AddConfigInsured(model.Name, model.LastName, model.SSN, model.PassportNumber_ID, model.BirthDate.Value, model.Address);
                            if (idInsured != 0)
                                _configInsuredPolicyService.AddConfigInsuredPolicy(idPolicy, idInsured, 2);
                        }
                        var policyFormModel = new PolicyFormViewModel();
                        policyFormModel.ExcelID = excelConfig.ID;
                        policyFormModel.ExcelPath = excelConfig.excel_path;
                        policyFormModel.IdPolicy = idPolicy;
                        return RedirectToAction("PolicyForm", "ConfigurationPolicy", policyFormModel);
                    }
                }

            }
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
        public ActionResult PolicyForm(int? excelId, int? idPolicy, FormCollection formCollection)
        {
            if (excelId.HasValue && idPolicy.HasValue)
            {
                var premium = DatabaseCommands.CalculatePremium((int)excelId, formCollection);
                ViewBag.CalculatedPremium = premium;

                var isUpdated = _configPolicyService.UpdateConfigPolicy((int)idPolicy, premium);

                if(isUpdated)
                {
                    var configPolicyValues = DatabaseCommands.InsertConfigPolicyValues((int)excelId, formCollection, (int) idPolicy);
                    var isSuccessful = _configPolicyValuesService.AddConfigPolicyValues(configPolicyValues);
                }
                return View("PolicyPremium");
            }
            return new HttpStatusCodeResult(500, "Something went wrong");
        }
    }
}