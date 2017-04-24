using System;
using AutoMapper;
using InsuredTraveling.DI;
using InsuredTraveling.Models;

namespace InsuredTraveling.Helpers
{
    public static class SaveSavaPoliciesHelper
    {
        public static int SaveSavaPolicies(ISavaPoliciesService _savaPoliciesService,
                                           IUserService _userService,
                                           ISava_setupService _savaSetupService,
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
                }
                return returnValue;
            }
            else
            {
                return -1;
            }

        }
    }
}