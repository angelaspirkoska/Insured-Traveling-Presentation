using InsuredTraveling.Models;
using Authentication.WEB.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InsuredTraveling.Models.AdminPanel;
using System.Configuration;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net.Mime;

namespace InsuredTraveling
{
    public class AuthRepository : IDisposable
    {
        private readonly AuthContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        public async Task<IdentityResult> CreateApplicationUser(UserModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                InsuranceCompany = "Sava",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                MobilePhoneNumber = userModel.MobilePhoneNumber,
                Gender = userModel.Gender,
                DateOfBirth = userModel.DateOfBirth,
                CreatedOn = DateTime.UtcNow

            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            var result2 = _userManager.AddToRole(user.Id, "Sava_normal");

            return result;
        }

        public int AddClient(Client c)
        {
            Client client = new Client
            {
                Id = c.Id,
                Secret = Helper.GetHash(c.Secret),
                Name = c.Name,
                ApplicationType = Enums.ApplicationTypes.NativeConfidential,
                Active = true,
                RefreshTokenLifeTime = 1234,
                AllowedOrigin = "*"
            };

            _ctx.Clients.Add(client);
            var result = _ctx.SaveChanges();
            return result;

        }

        public async void RefreshToken(string refresh_token = "")
        {
            var uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/token");
            var client = new HttpClient { BaseAddress = uri };
            IDictionary<string, string> userData = new Dictionary<string, string>();
            userData.Add("client_id", "InsuredTravel");
            userData.Add("refresh_token", refresh_token);
            userData.Add("grant_type", "refresh_token");
            HttpContent content = new FormUrlEncodedContent(userData);
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var responseMessage = client.PostAsync(uri, content).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = returnContent(responseMessage);
                await Task.WhenAll(responseBody);
                dynamic data = JObject.Parse(responseBody.Result);
                string token = data.access_token;
                string refresh_token2 = data.refresh_token;
                if (!String.IsNullOrEmpty(token))
                {
                    string encryptedToken = HttpUtility.UrlEncode(EncryptionHelper.Encrypt(token));
                    HttpCookie cookieToken = new HttpCookie("token", encryptedToken);
                    cookieToken.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookieToken);

                    //string encryptedRefreshToken = HttpUtility.UrlEncode(EncryptionHelper.Encrypt(refresh_token2));
                    HttpCookie cookieRefreshToken = new HttpCookie("refresh_token", refresh_token2);
                    cookieRefreshToken.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookieRefreshToken);
                }
            }
        }

        public async Task<string> returnContent(HttpResponseMessage responseMessage)
        {
            return await responseMessage.Content.ReadAsStringAsync();
        }

        public async Task<IdentityResult> CreateApplictionUserWeb(User userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                InsuranceCompany = "Sava",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                MobilePhoneNumber = userModel.MobilePhoneNumber,
                Gender = userModel.Gender,
                DateOfBirth = userModel.DateOfBirth.Value,
                PhoneNumber = userModel.PhoneNumber,
                PassportNumber = userModel.PassportNumber,
                Municipality = userModel.Municipality,
                PostalCode = userModel.PostalCode,
                Address = userModel.Address,
                EMBG = userModel.EMBG,
                City = userModel.City,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = userModel.CreatedBy != null ? userModel.CreatedBy : " "
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            var result2 = _userManager.AddToRole(user.Id, userModel.Role);

            try
            {
                if (result.Succeeded)
                {
                    var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/SAVAMAK728x90.jpg"));
                    inlineLogo.ContentId = Guid.NewGuid().ToString();
                    var FacebookLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/facebook@2x.png"));
                    FacebookLogo.ContentId = Guid.NewGuid().ToString();
                    var TwitterLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/twitter@2x.png"));
                    TwitterLogo.ContentId = Guid.NewGuid().ToString();

                 string mailBody = string.Format(@"   <div>
                  <div >
                  <a href='https://mk.sava.insure/'> <img style='width: 100%; max-width: 1000px; ' src=""cid:{0}"" /> </a>
                    <p> <b>Добредојдoвте на Сава Спорт + мобилната апликација</b> .</p>                  
                     <br /> <br /> 
                     <br /> Ве молиме почекајте 24 часа вашите податоци бидат ажурирани.
                     <br /> <br />Ви благодариме што одлучивте да ја користите SAVA Спорт + апликацијата. </div>"
                + " <div style='border-top: 1px solid #BBBBBB; max-width: 1000px; width:100%; max-width: 1000px; line-height:1px; height:1px; font-size:1px; '>&nbsp;</div> "
                + @" <div style=' text-align: center;'> <a href='https://www.facebook.com/sava.mk'> <img style='width:32px; max-width:35px' src=""cid:{1}"" /></a> <a href='https://twitter.com/Savamk'><img style='width:32px; max-width:35px' src=""cid:{2}"" /></a> </div>"
                + "<br /> "
               , inlineLogo.ContentId, FacebookLogo.ContentId, TwitterLogo.ContentId);
                   
                    var view = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Plain);
                    var view2 = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);

                    view2.LinkedResources.Add(inlineLogo);
                    view2.LinkedResources.Add(FacebookLogo);
                    view2.LinkedResources.Add(TwitterLogo);

                    MailService mailService = new MailService(userModel.Email);

                    mailService.setSubject("Моја Сава. Успешно креиран корисник.");

                    mailService.setBodyText(mailBody, true);

                    mailService.AlternativeViews(view);
                    mailService.AlternativeViews(view2);
                    //ALTERNATIVE VIEW
                    mailService.AlternativeViews(view);

                    mailService.sendMail();

                }
            }
            catch (Exception e)
            {
                return null;
            }

            return result;
        }

        public IdentityResult AddUserToRole(string userID, string roleName)
        {
            var roles = _userManager.GetRoles(userID);
            foreach (var r in roles)
            {
                _userManager.RemoveFromRole(userID, r);
            }
            var result = _userManager.AddToRole(userID, roleName);

            return result;
        }



        public IdentityResult AddRole(Roles r)
        {
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role.Name = r.Name;
            var result = roleManager.Create(role);
            return result;
        }

        public ApplicationUser FindUser2(string username, string password)
        {
            return _userManager.Find(username, password);
        }


        public async Task<IdentityResult> FindUserByUsername(string username)
        {
            var r = await _userManager.FindByNameAsync(username);
            if (r != null)
            {
                return new IdentityResult("Username alredy exists");
            }
            return new IdentityResult("OK");
        }

        public string GetUserID(string username)
        {
            var r = _userManager.FindByName(username);
            if (r != null)
            {
                return r.Id;
            }
            return null;
        }
        public async Task<IdentityResult> FillUserDetails(UserModel_Detail user_details)
        {
            var user = await _userManager.FindByNameAsync(user_details.username);
            if (user != null)
            {
                user.PhoneNumber = user_details.PhoneNumber;
                user.PassportNumber = user_details.PassportNumber;
                user.Municipality = user_details.Municipality;
                user.PostalCode = user_details.PostalCode;
                user.City = user_details.City;
                user.Address = user_details.Address;
                user.EMBG = user_details.EMBG;

                var r1 = _userManager.Update(user);

                if (r1.Succeeded)
                {
                    string body = "Welcome to Insured Traveling  " + " " + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + ConfigurationManager.AppSettings["webpage_url"] + "/validatemail".Replace("CS.aspx", "CS_Activation.aspx") + "?ID=" + user.Id + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    MailService mailService = new MailService("slobodanka@optimalreinsurance.com", "signup@insuredtraveling.com");
                    mailService.setSubject("Account Activation Validation");
                    mailService.setBodyText(body, true);
                    mailService.sendMail();
                }

                return r1;
            }
            return new IdentityResult("The username is not valid");
        }

        public async void ActivateAccount(string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
               
                    var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/SAVAMAK728x90.jpg"));
                    inlineLogo.ContentId = Guid.NewGuid().ToString();
                    var FacebookLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/facebook@2x.png"));
                    FacebookLogo.ContentId = Guid.NewGuid().ToString();
                    var TwitterLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/twitter@2x.png"));
                    TwitterLogo.ContentId = Guid.NewGuid().ToString();

                    string mailBody = string.Format(@"   <div>
                  <div >
                  <a href='https://mk.sava.insure/'> <img style='width: 100%; max-width: 1000px; ' src=""cid:{0}"" /> </a>
                    <p> <b>Добредојдoвте на Сава Спорт + мобилната апликација</b> .</p>                  
                     <br /> <br /> 
                     <br /> Ве молиме почекајте 24 часа вашите податоци бидат ажурирани.
                     <br /> <br />Ви благодариме што одлучивте да ја користите SAVA Спорт + апликацијата. </div>"
                   + " <div style='border-top: 1px solid #BBBBBB; max-width: 1000px; width:100%; max-width: 1000px; line-height:1px; height:1px; font-size:1px; '>&nbsp;</div> "
                   + @" <div style=' text-align: center;'> <a href='https://www.facebook.com/sava.mk'> <img style='width:32px; max-width:35px' src=""cid:{1}"" /></a> <a href='https://twitter.com/Savamk'><img style='width:32px; max-width:35px' src=""cid:{2}"" /></a> </div>"
                   + "<br /> "
                  , inlineLogo.ContentId, FacebookLogo.ContentId, TwitterLogo.ContentId);

                    var view = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Plain);
                    var view2 = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);

                    view2.LinkedResources.Add(inlineLogo);
                    view2.LinkedResources.Add(FacebookLogo);
                    view2.LinkedResources.Add(TwitterLogo);

                    MailService mailService = new MailService(user.Email);

                    mailService.setSubject("Моја Сава. Успешно креиран корисник.");

                    mailService.setBodyText(mailBody, true);

                    mailService.AlternativeViews(view);
                    mailService.AlternativeViews(view2);
                    //ALTERNATIVE VIEW
                    mailService.AlternativeViews(view);

                    mailService.sendMail();

                    try
                    {
                        var inlineLogo2 =
                           new LinkedResource(
                               System.Web.HttpContext.Current.Server.MapPath(
                                   "~/Content/img/EmailHeaderWelcome1.png"));
                        inlineLogo.ContentId = Guid.NewGuid().ToString();

                        string body2 = string.Format(@"   
                            <div style='margin-left:20px'>
                            <p> <b>Welcome to My Sava </b> - the standalone platform for online sales of insurance policies.</p>                  
                            <br /> <br /> To inform you that the following user was registered at MySava : 
                            <br />" + "Username: " + user.UserName +
                                                     "<br /> " + "SSN: " + user.EMBG +
                                                     "<br /> " + "Email: " + user.Email +
                                                     //"<br /> " + "Role: " + user.Role +
                                                     "<br /> <br />Thanks </div>", inlineLogo2.ContentId);

                        MailService mailService2 = new MailService("atanasovski46@gmail.com", "webs.sava@sava.mk");
                        mailService2.setSubject("My Sava - User registered on Sava");
                        mailService2.setBodyText(body2, true);

                        mailService2.sendMail();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }


        public bool ValidateMail(string ID)
        {
            var user = _userManager.FindById(ID);
            if (user != null)
            {
                user.EmailConfirmed = true;
                _userManager.Update(user);
                return true;
            }
            return false;
        }




        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void SendEmailForForgetPasswordByUserName(string username)
        {
            var r = _userManager.FindByName(username);
            if (r != null)
            {
                string body = "Моја сава ресетирање на корисничка лозинка " + " " + ",";
                body += "<br /><br />Ве молам притиснете на следниот линк за да ја ресетирате вашата лозинка";
                body += "<br /><a href = '" + ConfigurationManager.AppSettings["webpage_url"] + "/forgetpassword".Replace("CS.aspx", "CS_Activation.aspx") + "?ID=" + r.Id + "'>Click here to reset your password.</a>";
                body += "<br /><br />";
                MailService mailService = new MailService(r.Email, "webs.sava@sava.mk");
                mailService.setSubject("Ресетирање на корисничка лозинка");
                mailService.setBodyText(body, true);
                mailService.sendMail();
            }
        }

        public void SendEmailForForgetPasswordByEmail(string email)
        {
            
            var r = _userManager.FindByEmail(email);
            if (r != null)
            {
                string body = "Welcome to Insured Traveling " + " " + ",";
                body += "<br /><br />Please click the following link to reset your password";
                body += "<br /><a href = '" + ConfigurationManager.AppSettings["webpage_url"] + "/forgetpassword".Replace("CS.aspx", "CS_Activation.aspx") + "?ID=" + r.Id + "'>Click here to reset your password.</a>";
                body += "<br /><br />Thanks";
                MailService mailService = new MailService("atanasovski46@gmail.com", "webs.sava@sava.mk");
                mailService.setSubject("Account Reset Password");
                mailService.setBodyText(body, true);
                mailService.sendMail();
            }
        }

        public bool ValidUsernameOrMail(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                if (_userManager.FindByEmail(s) != null)
                {
                    SendEmailForForgetPasswordByEmail(s);
                    return true;
                }
                else if (_userManager.FindByName(s) != null)
                {
                    SendEmailForForgetPasswordByUserName(s);
                    return true;
                }
            }
            return false;
        }
        public async Task<IdentityResult> ChangePassword(ForgetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.ID);
            if (user != null)
            {
                var removePass = _userManager.RemovePassword(user.Id);
                _userManager.Update(user);
                if (removePass.Succeeded)
                {
                    var addPass = _userManager.AddPassword(user.Id, model.Password);
                    _userManager.Update(user);
                    var checkPass = _userManager.CheckPassword(user, model.Password);
                    return addPass;
                }
            }
            return new IdentityResult();
        }

        public IdentityResult ChangePasswordByUsername(ForgetPasswordModel model)
        {
            var user = _userManager.FindByName(model.username);
            if (user != null)
            {
                var removePass = _userManager.RemovePassword(user.Id);
                _userManager.Update(user);
                if (removePass.Succeeded)
                {
                    var addPass = _userManager.AddPassword(user.Id, model.Password);
                    _userManager.Update(user);
                    var checkPass = _userManager.CheckPassword(user, model.Password);
                    return addPass;
                }
            }

            return new IdentityResult();
        }

        public async Task<IdentityResult> SendSmsCode(string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                var user = (String.IsNullOrEmpty(username)) ? null : await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    if (user.MobilePhoneNumber != null)
                    {
                        if (user.AccessFailedCount > 5)
                            return new IdentityResult("You have reached the limited numbers of attempts, try again tomorrow");
                        SMSvalidation s = new SMSvalidation();
                        string code = s.SendMessage();
                        user.ActivationCodeSMS = code;
                        var result = _userManager.Update(user);
                        return result;
                    }
                }
            }
            return new IdentityResult("Failed");
        }

        public async Task<IdentityResult> ConfirmSmsCode(string username, string code)
        {
            if (!String.IsNullOrEmpty(username))
            {

                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    if (user.ActivationCodeSMS != null)
                    {
                        if (user.ActivationCodeSMS == code)
                        {
                            user.PhoneNumberConfirmed = true;
                            var result = _userManager.Update(user);

                            return result;
                        }
                        else
                        {
                            user.AccessFailedCount += 1;
                            _userManager.Update(user);
                            return new IdentityResult("The code provided is not valid");
                        }
                    }
                }
            }
            return new IdentityResult("Failed");
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

    }
}