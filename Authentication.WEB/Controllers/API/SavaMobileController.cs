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
using Microsoft.Ajax.Utilities;
using Rotativa;
using Newtonsoft.Json.Linq;
using InsuredTraveling.Filters;

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/SavaMobile")]
    public class SavaMobileController : ApiController
    {
        private readonly ISavaPoliciesService _savaPoliciesService;
        private readonly IUserService _userService;
        private readonly ISava_setupService _savaSetupService;
        private readonly RoleAuthorize _roleAuthorize;

        public SavaMobileController(ISavaPoliciesService savaPoliciesService,
                                    IUserService userService,
                                    ISava_setupService savaSetupService)
        {
            _savaPoliciesService = savaPoliciesService;
            _userService = userService;
            _savaSetupService= savaSetupService;
            _roleAuthorize = new RoleAuthorize();
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CreatePolicy")]
        public IHttpActionResult CreatePolicy(QRCodeSavaPolicy model)
        {
            if (model != null)
            {
                if (String.IsNullOrEmpty(model.PolicyNumber) || String.IsNullOrEmpty(model.EmailSeller) || String.IsNullOrEmpty(model.ExpireDate) || String.IsNullOrEmpty(model.Premium)
                    || String.IsNullOrEmpty(model.SSNHolder) || String.IsNullOrEmpty(model.SSNInsured))
                {
                    throw new Exception("Internal error: Not all parametars are valid");
                }
                else
                {
                    var policy = Mapper.Map<QRCodeSavaPolicy, SavaPolicyModel>(model);
                    var policyId = SaveSavaPoliciesHelper.SaveSavaPolicies(_savaPoliciesService, _userService, _savaSetupService, _roleAuthorize, policy);
                    if (policyId != -1)
                    {
                        return Ok();
                    }
                    else
                    {
                        throw new Exception("Internal error: Not able to save policy");
                    }
                }
               
            }
            else
            {
                throw new Exception("Internal error: Empty Policy");
            }
           
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("UsePoints")]
        public IHttpActionResult UsePoints(UsePointsModel model)
        {
            if (model != null)
            {
                var user = _userService.GetUserDataByUsername(model. Username);
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
                                if (SendSavaEmailHelper.SendVaucerEmail(model, user.Email, userPoints))
                                {
                                    user.Points = userPoints;
                                    _userService.UpdateUserPoints(user);
                                    return Ok();
                                }
                                else
                                    throw new Exception("Internal error: The email is not send");
                            }
                            else
                                throw new Exception("Internal error: The user has less points");
                        }
                        else
                            throw new Exception("Internal error: The points value is not in the valid format");

                    }
                    else
                        throw new Exception("Internal error: The points value is null");
                }
                else
                    throw new Exception("Internal error: User not exists");
            }
            else
                throw new Exception("Internal error: Empty JSON");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CalulatePoints/{username}")]
        public JObject CalulatePoints(string username)
        {
            if (username != null)
            {
                var user = _userService.GetUserDataByUsername(username);
                JObject data = new JObject();
                data.Add("Points", user.Points);
                return data;
            }
            else
            {
                throw new Exception("Internal error: Empty username");
            }
        }
    }
}