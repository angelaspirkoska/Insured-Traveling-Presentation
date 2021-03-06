﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public class UserService : IUserService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetPolicyNumberListByUsername(string Username)
        {
            string ID = _db.aspnetusers.Where(a => a.UserName == Username).Single().Id;
            var PolicyNumbers =  _db.travel_policy.Where(p => p.Created_By == ID).Select(p => new SelectListItem
           {
               Text = p.Policy_Number,
               Value = p.ID.ToString()
           });

            return PolicyNumbers;
        }

        public int UpdateUser(User editedUser)
        {

            aspnetuser editedUserDb = GetUserDataByUsername(editedUser.UserName);
            editedUserDb.UserName = editedUser.UserName;
            editedUserDb.FirstName = editedUser.FirstName;
            editedUserDb.LastName = editedUser.LastName;
            editedUserDb.City = editedUser.City;
            editedUserDb.Address = editedUser.Address;
            editedUserDb.Municipality = editedUser.Municipality;
            editedUserDb.MobilePhoneNumber = editedUser.MobilePhoneNumber;
            editedUserDb.Email = editedUser.Email;
            editedUserDb.DateOfBirth = editedUser.DateOfBirth;
            editedUserDb.EMBG = editedUser.EMBG;
            editedUserDb.Gender = editedUser.Gender;
            editedUserDb.PassportNumber = editedUser.PassportNumber;
            editedUserDb.PostalCode = editedUser.PostalCode;
            editedUserDb.PhoneNumber = editedUser.PhoneNumber;
            var userRole = editedUserDb.aspnetroles.FirstOrDefault();
            if (userRole != null && userRole.Name != editedUser.Role)
            {
                aspnetrole previousUserRole = _db.aspnetroles.Where(x => x.Name == userRole.Name).FirstOrDefault();
                if(previousUserRole != null)
                    editedUserDb.aspnetroles.Remove(previousUserRole);
                AuthRepository _repo = new AuthRepository();
                try
                {
                    _repo.AddUserToRole(editedUserDb.Id, editedUser.Role);
                }
                catch (Exception ex)
                {
                }
            }

            int result = -1;

            try
            {
                result = _db.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }

            return result;
        }

        public List<aspnetuser> GetUsersByRoleName(string Role)
        {
            aspnetrole r = _db.aspnetroles.Where(x => x.Name == Role).FirstOrDefault();
            return _db.aspnetusers.Where(x => x.aspnetroles.FirstOrDefault().Name.Contains(Role)).ToList();
        }

        public List<travel_policy> GetPoliciesByUsernameToList(string Username, string Prefix)
        {
            string ID = _db.aspnetusers.Where(a => a.UserName == Username).Single().Id;
            var policies = _db.travel_policy.Where(p => p.Created_By == ID && p.Payment_Status == true && p.Policy_Number.Contains(Prefix)).ToList();
            return policies;
        }

        public bool ChangeStatus(string username)
        {
            aspnetuser user = _db.aspnetusers.Where(x => x.UserName == username).FirstOrDefault();
            int status = user.Active.Value;
            switch (status)
            {
                case 1: user.Active = 0; break;
                case 0: user.Active = 1; break;
            }

            var result = _db.SaveChanges();
            return result == 1;
        }
        public aspnetuser GetUserById(string id)
        {
          return  _db.aspnetusers.Where(x => x.Id == id).ToArray().Last();
        }

        public aspnetuser GetUserDataByUsername(string username)
        {
            aspnetuser a = _db.aspnetusers.Where(x => x.UserName == username).ToArray().FirstOrDefault();
            return a;
        }




        public string GetUserIdByUsername(string Username)
        {
            return _db.aspnetusers.Where(x => x.UserName == Username).Select(x => x.Id).FirstOrDefault();
        }

        public string GetUserSsnByUsername(string username)
        {
            return _db.aspnetusers.Where(x => x.UserName == username).Select(x => x.EMBG).SingleOrDefault();
           
        }

        public bool IsSameLoggedUserAndInsured(string UsernameLoggedUser, int SelectedInsuredId)
        {
            string SSNLoggedUser = _db.aspnetusers.Where(x => x.UserName == UsernameLoggedUser).Select(x => x.EMBG).First(); 
            
            string SSNSelectedInsured = _db.insureds.Where(x => x.ID == SelectedInsuredId).Select(x => x.SSN).First();

            return SSNLoggedUser.Equals(SSNSelectedInsured);

        }

        public void UpdateSsnById(string id, string ssn)
        {
            var user = _db.aspnetusers.Where(x => x.Id.Equals(id)).FirstOrDefault();
            user.EMBG = ssn;
            _db.SaveChanges();
        }

        public List<aspnetuser> GetAllUsers()
        {
            return _db.aspnetusers.ToList();
        }
    }
}
