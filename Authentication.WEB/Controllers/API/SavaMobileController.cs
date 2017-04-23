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

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/SavaMobile")]
    public class SavaMobileController : ApiController
    {
        public readonly ISavaPoliciesService _savaPoliciesService;
        public readonly IUserService _userService;
        private readonly ISava_setupService _savaSetupService;
        private readonly IEventsService _es;
        private readonly IEventUserService _eus;
        private readonly IUserService _us;

        public SavaMobileController(ISavaPoliciesService savaPoliciesService,
                                    IUserService userService,
                                    ISava_setupService savaSetupService, IEventsService es, IEventUserService eus, IUserService us)
        {
            _savaPoliciesService = savaPoliciesService;
            _userService = userService;
            _savaSetupService= savaSetupService;
            _es = es;
            _eus = eus;
            _us = us;
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
        [System.Web.Http.Route("SetAttending")]
        public JObject SetAttending(JObject IDjson)
        {
            
            string username = (string)IDjson["username"];
            string eventId = (string)IDjson["eventId"];
            JObject data = new JObject();

            string userId = _us.GetUserIdByUsername(username);
            if (userId != null && eventId != null)
            {
                if (_eus.AddUserAttending(userId, int.Parse(eventId)))
                {
                    data.Add("Successful", "True");
                    data.Add("Error message", "");
                }
                else
                {
                    data.Add("Successful", "False");
                    data.Add("Error message", "Database write failure ");
                };
            }
            else
            {
                throw new Exception("Internal error: Empty userID or eventID");
            }
            return data;
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