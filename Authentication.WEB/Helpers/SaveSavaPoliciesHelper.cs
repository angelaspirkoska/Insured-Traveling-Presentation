using System;
using AutoMapper;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.Models;

namespace InsuredTraveling.Helpers
{
    public static class SaveSavaPoliciesHelper
    {
        public static int SaveSavaPolicies(ISavaPoliciesService _savaPoliciesService,
                                           IUserService _userService,
                                           ISava_setupService _savaSetupService,
                                           RoleAuthorize _roleAuthorize, 
                                           SavaPolicyModel model)
        {
            var policyHolder = _userService.GetUserBySSN(model.SSN_policyHolder);
            if (policyHolder != null)
            {
                var policy = Mapper.Map<SavaPolicyModel, sava_policy>(model);
                var savaSetup = _savaSetupService.GetActiveSavaSetup();
                var percent = savaSetup != null ? savaSetup.points_percentage : 1;
                var points = Math.Round(policy.premium / percent);
                policy.discount_points = (float) points;
                int returnValue = _savaPoliciesService.SaveSavaPolicy(policy);

                if (returnValue != -1)
                {
                    var currentPoints = policyHolder.Points;
                    policyHolder.Points = (float)points + currentPoints;
                    _userService.UpdateUserPoints(policyHolder);
                    _userService.UpdatePremiumSum(model.SSN_policyHolder, policy.premium, policy.expiry_date );
                }
                ChangeUserRole(_savaSetupService, _userService, _roleAuthorize, policyHolder);
                return returnValue;
            }
            else
            {
                return -1;
            }

        }

        public static bool ChangeUserRole(ISava_setupService _savaSetupService, 
                                          IUserService _userService,
                                          RoleAuthorize _roleAuthorize,
                                          aspnetuser policyHolder)
        {
            try
            {
                AuthRepository _repo = new AuthRepository();
                var Sava_admin = _savaSetupService.GetLast();
                float? UserSumPremiums = _userService.GetUserSumofPremiums(policyHolder.EMBG);

                if (UserSumPremiums == null)
                {
                    UserSumPremiums = 0;
                }

                if (_roleAuthorize.IsUser("Sava_normal", policyHolder.UserName))
                {
                    string userRole = "Сава+ корисник на Сава осигурување";
                    SendSavaEmailHelper.SendEmailForUserChangeRole(policyHolder.Email, policyHolder.FirstName, policyHolder.LastName, userRole);
                    _repo.AddUserToRole(policyHolder.Id, "Sava_Sport+");
                }
                if (_roleAuthorize.IsUser("Sava_Sport+", policyHolder.UserName))
                {
                    if (Sava_admin.vip_sum <= UserSumPremiums)
                    {
                        string userRole = "VIP корисник на Сава осигурување";
                        SendSavaEmailHelper.SendEmailForUserChangeRole(policyHolder.Email, policyHolder.FirstName, policyHolder.LastName, userRole);
                        _repo.AddUserToRole(policyHolder.Id, "Sava_Sport_VIP");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
           
        }
    }
}