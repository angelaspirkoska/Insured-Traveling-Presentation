﻿using InsuredTraveling.Models;
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
using System.Net.Mail;
using Authentication.WEB.Services;

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/SavaMobile")]
    public class SavaMobileController : ApiController
    {
        private readonly ISavaPoliciesService _savaPoliciesService;
        private readonly IUserService _userService;
        private readonly ISava_setupService _savaSetupService;
        private readonly IEventsService _es;
        private readonly RoleAuthorize _roleAuthorize;
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
            _roleAuthorize = new RoleAuthorize();
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


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("AddPolicyPoints")]
        public JObject AddPolicyPoints(JObject IDjson)
        {

            JObject data = new JObject();
            string PolicyNumber = (string)IDjson["PolicyNumber"];
            string SSN = (string)IDjson["SSN"];
            if (PolicyNumber != null && PolicyNumber != "" && PolicyNumber != " " && SSN != null && SSN != " ")
            {
                // Ako postoi polisa so toj broj cekor 4
                if (_savaPoliciesService.GetSavaPolicyIdByPolicyNumber(PolicyNumber) != null) {

                    // Dali postoi
                    if (_savaPoliciesService.GetSavaPoliciesForList(SSN, PolicyNumber).Count() != 0)
                    {

                        AuthRepository _repo = new AuthRepository();
                        var PolicyUser = _userService.GetUserBySSN(SSN);

                        if (_roleAuthorize.IsUser("Sava_normal", PolicyUser.UserName))
                        {
                            string userRole = "Сава+ корисник на Сава осигурување";
                            
                            _repo.AddUserToRole(PolicyUser.Id, "Sava_Sport+");
                            SendSavaEmail(PolicyUser.Email, PolicyUser.FirstName, PolicyUser.LastName, userRole);
                        }

                        var x = _savaPoliciesService.GetSavaPoliciesForList(SSN, PolicyNumber);
                        data.Add("Message", "Sucessfully added points");
                        data.Add("Status", "valid");
                        
                        return data;
                    }
                    else if (_savaPoliciesService.GetSavaPoliciesForInsuredList(SSN, PolicyNumber).Count() != 0)
                    {
                        
                        data.Add("Message", "User and policy exist, but the user is insured, not policy holder");
                        data.Add("Status", "false");

                        return data;
                    }
                    else
                    {
                        data.Add("Message", "User and policy does not match.!");
                        data.Add("Status", "false");

                        return data;
                    }

                } else
                {
                   
                    data.Add("Message", "Policy does not exist yet, try again later");
                    data.Add("Status", "false");

                    return data;
                }

            }


            {
                throw new Exception("Internal error: Empty Fields");
            }

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
    }
}