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
using System.Net.Mail;
using Authentication.WEB.Services;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/SavaMobile")]
    [System.Web.Http.Authorize]
    public class SavaMobileController : ApiController
    {
        private readonly ISavaPoliciesService _savaPoliciesService;
        private readonly IUserService _userService;
        private readonly ISava_setupService _savaSetupService;
        private readonly IEventsService _es;
        private readonly IEventUserService _eus;
        private readonly IUserService _us;
        private readonly ISavaVoucherService _svs;
        private readonly ISavaAdPicService _savaAdService;
        private readonly IPointsRequestService _prs;
        private readonly ISalePointsService _sps;

        public SavaMobileController(ISavaPoliciesService savaPoliciesService,
                                    IUserService userService,
                                    ISava_setupService savaSetupService, IEventsService es, IEventUserService eus, IUserService us,
                                    IRolesService rs,
                                    ISavaVoucherService svs,
                                    ISavaAdPicService savaAdService,
                                    IPointsRequestService prs,
                                    ISalePointsService sps)
        {
            
            _savaPoliciesService = savaPoliciesService;
            _userService = userService;
            _savaSetupService= savaSetupService;
            _es = es;
            _eus = eus;
            _us = us;
            _svs = svs;
            _savaAdService = savaAdService;
            _prs = prs;
            _sps = sps;
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
               
                    var user = _userService.GetUserDataByUsername(model.Username);
                if (_us.GetUserBySellerID(model.IDSeller) == null)
                {
                    throw new Exception("Internal error: Seller ID does not exist");
                }
                    string SellerEmail = _us.GetUserEmailBySellerID(model.IDSeller);
              
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

                                SavaVoucherModel SavaVoucher = new SavaVoucherModel();
                                SavaVoucher.id_policyHolder = user.EMBG;
                                SavaVoucher.points_used = points;
                                SavaVoucher.id_seller = model.IDSeller;
                                SavaVoucher.timestamp = DateTime.Now;

                                if (SendSavaEmailHelper.SendVaucerEmail(model, user.Email, userPoints, SellerEmail) )
                                {
                                    if (SendSavaEmailHelper.SendVaucerEmailToSeller(model, user, userPoints, SellerEmail))
                                    {
                                      user.Points = userPoints;
                                        _userService.UpdateUserPoints(user);
                                        _svs.AddSavaVoucher(SavaVoucher);

                                        return Ok();

                                    }else
                                    {
                                        throw new Exception("Internal error: The seller email is not send");
                                    }
                                }
                                else
                                    throw new Exception("Internal error: The user email is not send");
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
        [System.Web.Http.Route("CalculatePoints/{username}")]
        public JObject CalculatePoints(string username)
        {
            if (username != null)
            {
                if (RoleAuthorize.IsUser("Sava_Sport+", username) || RoleAuthorize.IsUser("Sava_Sport_VIP", username))
                {
                    var user = _userService.GetUserDataByUsername(username);
                    JObject data = new JObject();
                    data.Add("Points", user.Points);
                    return data;
                }else
                {
                    JObject data = new JObject();
                   
                    data.Add("Points", 0);
                    data.Add("Message", "Internal error: User is not verified.");
                    return data;
                    
                }

            }
            else
            {
                throw new Exception("Internal error: Empty username");
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetAdPicture")]
        public JObject GetAdPicture()
        {
            SavaAdPicturesModel PicModel = new SavaAdPicturesModel();
            var listPictures = _savaAdService.GetAdPictures();

            Random rnd1 = new Random();
            int br = listPictures.Count();
            int randNum = rnd1.Next(0, br);

            
            PicModel.ImageLocation = listPictures[randNum].ImageLocation;
            PicModel.ImageLocation = System.Configuration.ConfigurationManager.AppSettings["webpage_apiurl"].ToString() + "/SavaAdPictures/" + listPictures[randNum].ImageLocation;
            PicModel.Title = listPictures[randNum].Title;
            


            var adInfo = new JObject();
            adInfo.Add("PictureLocation", PicModel.ImageLocation);
            adInfo.Add("Title", PicModel.Title);
            return adInfo;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetSalePoints")]
        public JObject GetSalePoints()
        {
          var SalePoints = _sps.GetSalePoints();
            
            var SalePointsObject = new JObject();
           
            SalePointsObject.Add("SalePoints", JToken.FromObject(SalePoints));
            
            return SalePointsObject;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("AddPolicyPoints")]
        public JObject AddPolicyPoints(JObject IDjson)
        {

            JObject data = new JObject();
            string PolicyNumber = (string)IDjson["PolicyNumber"];
            string SSN = (string)IDjson["SSN"];
            AuthRepository _repo = new AuthRepository();
            

            if (PolicyNumber != null && PolicyNumber != "" && PolicyNumber != " " && SSN != null && SSN != " ")
            {
                var PolicyUser = _userService.GetUserBySSN(SSN);
                if (PolicyUser == null)
                {
                    data.Add("Message", "User with this SSN does not exist.");
                    data.Add("Status", "false");

                    return data;
                }
                else
                {
                    PointsRequestModel p_request = new PointsRequestModel();
                    p_request.id_user = PolicyUser.Id;
                    p_request.policy_id = PolicyNumber;
                    p_request.ssn = SSN;
                    p_request.DateCreated = DateTime.Now;
                    _prs.AddPoints_Request(p_request);

                    //if (_roleAuthorize.IsUser("Sava_Sport+", PolicyUser.UserName))
                    //{
                    //    data.Add("Message", "You are already Sava Sport + user. You can use your discount points.");
                    //    data.Add("Status", "false");

                    //    return data;
                    //}
                    //else
                    //{

                        // Ako postoi polisa so toj broj cekor 4
                        if (_savaPoliciesService.GetSavaPolicyIdByPolicyNumber(PolicyNumber) != null)
                        {

                            if (_savaPoliciesService.GetSavaPoliciesForList(SSN, PolicyNumber).Count() != 0)
                            {
                                string message = "";
                                if (RoleAuthorize.IsUser("Sava_normal", PolicyUser.UserName))
                                {
                                    string userRole = "Сава+ корисник на Сава осигурување";

                                    _repo.AddUserToRole(PolicyUser.Id, "Sava_Sport+");

                                    _prs.ChangeFlagStatus(PolicyNumber);
                                    message = "Sucessfully chaged user role, User is now Sava Sport +";
                                    SendSavaEmailHelper.SendEmailForUserChangeRole(PolicyUser.Email, PolicyUser.FirstName, PolicyUser.LastName, userRole);
                                }

                                var Sava_admin = _savaSetupService.GetLast();
                                float? UserSumPremiums = _userService.GetUserSumofPremiums(SSN);
                                if (UserSumPremiums == null)
                                {
                                    UserSumPremiums = 0;
                                }
                                //if (_roleAuthorize.IsUser("Sava_Sport+", PolicyUser.UserName))
                                //{
                                    if (Sava_admin.vip_sum <= UserSumPremiums)
                                    {
                                        string userRole = "VIP корисник на Сава осигурување";
                                        message = "Sucessfully chaged user role, User is now Sava Sport VIP";
                                        SendSavaEmailHelper.SendEmailForUserChangeRole(PolicyUser.Email, PolicyUser.FirstName, PolicyUser.LastName, userRole);
                                        _repo.AddUserToRole(PolicyUser.Id, "Sava_Sport_VIP");
                                    }
                                //}


                                data.Add("Message", message);
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

                        }
                        else
                        {

                            data.Add("Message", "Policy does not exist yet, try again later");
                            data.Add("Status", "false");

                            return data;
                        }

                    //}
                }
            }

            {
                throw new Exception("Internal error: Empty Fields");
            }

        }

     
    }
}