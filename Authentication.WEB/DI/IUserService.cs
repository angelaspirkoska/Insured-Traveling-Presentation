﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public interface IUserService
    {
        List<aspnetuser> GetSavaUsersByRoleName(string role);
        bool SetUserUpdated(string username);
        List<aspnetuser> GetAllUsersCreatedTodayForSavaAdmin(DateTime createdDate);
        int UpdateUser(User editedUser);
        aspnetuser GetUserDataByUsername(string Username);
        aspnetuser GetUserById(string id);
        string GetUserIdByUsername(string Username);
        IQueryable<SelectListItem> GetPolicyNumberListByUsername(string Username);
        List<travel_policy> GetPoliciesByUsernameToList(string Username, string Prefix);
        bool IsSameLoggedUserAndInsured(string UsernameLoggedUser, int SelectedInsured);
        string GetUserSsnByUsername(string username);
        List<aspnetuser> GetUsersByRoleName(string Role);
        void UpdateSsnById(string id, string ssn);
        bool ChangeStatus(string username);
        aspnetuser GetUserBySSN(string ssn);
        bool UpdateUserPoints(aspnetuser user);
        aspnetuser GetUserByEmail(string email);
        void UpdatePremiumSum(string policyHolder, float PolicyPremium);
        float? GetUserSumofPremiums(string policyHolder);
    }
}
