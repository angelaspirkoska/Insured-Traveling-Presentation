using InsuredTraveling.Models;
using Authentication.WEB.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using InsuredTraveling.Entities;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Net;
using Microsoft.Owin.Security;

namespace InsuredTraveling
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }
        
        public async Task<IdentityResult> RegisterUser(User userModel)
        {

            //string secret = "MobileApp123";
            //Client mobile_client = new Client
            //{
            //    Id = "MobileApp",
            //    Secret = Helper.GetHash(secret),
            //    Name = "Mobile Application",
            //    ApplicationType = Enums.ApplicationTypes.NativeConfidential,
            //    Active = true,
            //    RefreshTokenLifeTime = 1234,
            //    AllowedOrigin = "*"
            //};
            //AuthContext db = new AuthContext();
            //db.Clients.Add(mobile_client);
            //db.SaveChanges();

            //Random r = new Random();
            //string id = "1234" + r.Next(1000, 9999).ToString() + r.Next(100,999).ToString();

            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                InsuranceCompany = "Sava",
                IsValidMail = false,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                MobilePhoneNumber = userModel.MobilePhoneNumber,
                Gender = userModel.Gender,
                DateOfBirth = userModel.DateOfBirth
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);

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
                //_userManager.SendSms(user.Id, "Zdravo");
                //_userManager.SendEmail(user.Id, "Welcome to Optimal Insurance", "Activate your account");

                if (r1.Succeeded)
                {
                    string body = "Welcome to Optimal Insurance " + " " + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + "http://localhost:19655/validatemail".Replace("CS.aspx", "CS_Activation.aspx") + "?ID=" + user.Id + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    MailService mailService = new MailService("slobodanka@optimalreinsurance.com");
                    mailService.setSubject("Account Activation Validation");
                    mailService.setBodyText(body, true);
                    mailService.sendMail();
                }

                return r1;
            }
            return new IdentityResult("The username is not valid");
        }

        public bool ValidateMail(string ID)
        {
            var user = _userManager.FindById(ID);
            if (user != null)
            {
                user.IsValidMail = true;
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

        public void ForgetPassword(string username)
        {
            var r = _userManager.FindByName(username);
            if (r != null)
            {
                string body = "Welcome to Optimal Insurance " + " " + ",";
                body += "<br /><br />Please click the following link to reset your password";
                body += "<br /><a href = '" + "http://localhost:19655/forgetpassword".Replace("CS.aspx", "CS_Activation.aspx") + "?ID=" + r.Id + "'>Click here to reset your password.</a>";
                body += "<br /><br />Thanks";
                MailService mailService = new MailService("slobodanka@optimalreinsurance.com");
                mailService.setSubject("Account Reset Password");
                mailService.setBodyText(body, true);
                mailService.sendMail();
            }
        }

        public void ForgetPassword2(string email)
        {
            var r = _userManager.FindByEmail(email);
            if (r != null)
            {
                string body = "Welcome to Optimal Insurance " + " " + ",";
                body += "<br /><br />Please click the following link to reset your password";
                body += "<br /><a href = '" + "http://localhost:19655/forgetpassword".Replace("CS.aspx", "CS_Activation.aspx") + "?ID=" + r.Id + "'>Click here to reset your password.</a>";
                body += "<br /><br />Thanks";
                MailService mailService = new MailService("slobodanka@optimalreinsurance.com");
                mailService.setSubject("Account Reset Password");
                mailService.setBodyText(body, true);
                mailService.sendMail();
            }
        }
        public bool ValidUsernameOrMail(string s)
        {
            if(_userManager.FindByEmail(s) != null)
            {
                ForgetPassword2(s);
                return true;
            }else if (_userManager.FindByName(s) != null)
            {
                ForgetPassword(s);
                return true;
            }
            return false;
        }
        public async Task<IdentityResult> PasswordChange(ForgetPasswordModel model)
        {
            var user1 = await _userManager.FindByIdAsync(model.ID);
            if (user1 != null)
            {
               // var r1 = _userManager.ChangePassword(user1.Id, user1.PasswordHash, model.Password);
                var r1 =  _userManager.RemovePassword(user1.Id);
                _userManager.Update(user1);
                if (r1.Succeeded)
                {
                    var r2 =  _userManager.AddPassword(user1.Id, model.Password);
                    _userManager.Update(user1);
                    var r3 = _userManager.CheckPassword(user1, model.Password);

                    return r2;
                }
            }
            return new IdentityResult("Wrong user");
        }

        public async Task<IdentityResult> SendSmsCode(string username)
        {
            var user1 = await _userManager.FindByNameAsync(username);
            if (user1 != null)
            {
                if (user1.MobilePhoneNumber != null)
                {
                    if (user1.AccessFailedCount > 5)
                    {
                        return new IdentityResult("You have reached the limited numbers of atempts, try again tomorrow");
                    }
                    SMSvalidation s = new SMSvalidation();
                    string code = s.SendMessage();
                    user1.ActivationCodeSMS = code;
                    var result = _userManager.Update(user1);
                    return result;
                }
            }
            return new IdentityResult("Failed");
        }

        public async Task<IdentityResult> ConfirmSmsCode(string username, string code)
        {
            
            var user1 = await _userManager.FindByNameAsync(username);
            if (user1 != null)
            {
                if (user1.ActivationCodeSMS != null)
                {
                        if (user1.ActivationCodeSMS == code)
                        {
                            user1.PhoneNumberConfirmed = true;
                            var result = _userManager.Update(user1);

                            return result;
                        }
                        else
                        {
                            user1.AccessFailedCount += 1;
                            _userManager.Update(user1);
                            return new IdentityResult("The code provided is not valid");
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

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }
    }
}