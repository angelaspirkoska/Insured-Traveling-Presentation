using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using InsuredTraveling.DI;
using InsuredTraveling.Helpers;
using AutoMapper;

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/SavaMobile")]
    public class SavaMobileController : ApiController
    {
        public readonly ISavaPoliciesService _savaPoliciesService;
        public readonly IUserService _userService;
        private readonly ISava_setupService _savaSetupService;

        public SavaMobileController(ISavaPoliciesService savaPoliciesService,
                                    IUserService userService,
                                    ISava_setupService savaSetupService)
        {
            _savaPoliciesService = savaPoliciesService;
            _userService = userService;
            _savaSetupService= savaSetupService;
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CreatePolicy")]
        public IHttpActionResult CreatePolicy(QRCodeSavaPolicy model)
        {
            if (model != null)
            {
                var policy = Mapper.Map<QRCodeSavaPolicy, SavaPolicyModel>(model);
                var policyId = SaveSavaPoliciesHelper.SaveSavaPolicies(_savaPoliciesService, _userService, _savaSetupService, policy);
                if (policyId != -1)
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Internal error: Not able to save policy");
                }
            }
            else
            {
                throw new Exception("Internal error: Empty Policy");
            }
           
        }
    }
}