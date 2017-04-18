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
using System.Globalization;
using Rotativa;

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

        public IHttpActionResult UsePoints(UsePointsModel model)
        {
            if (model != null)
            {
                var user = _userService.GetUserByEmail(model.UserEmail);
                if (user != null)
                {
                    if (model.Points != null)
                    {
                        float points = -1;
                        float.TryParse(model.Points, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out points);
                        if (points != -1)
                        {
                            if (user.Points >= points)
                            {
                                var userPoints = user.Points - points;
                                return Ok();
                            }
                            else
                                throw new Exception("Internal error: The user have less points");
                        }
                        else
                            throw new Exception("Internal error: The points value is not in the valid format");

                    }
                    else
                        throw new Exception("Internal error: The points value is null");
                }
                else
                    throw new Exception("Internal error: User not exist");
            }
            else
                throw new Exception("Internal error: Empty JSON");
        }
    }
}