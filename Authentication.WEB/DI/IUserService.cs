﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public interface IUserService
    {
        aspnetuser GetUserDataByUsername(string Username);

        aspnetuser GetUserById(string id);

        string GetUserIdByUsername(string Username);

        IQueryable<SelectListItem> GetPolicyNumberListByUsername(string Username);

        bool IsSameLoggedUserAndInsured(string UsernameLoggedUser, int SelectedInsured);
    }
}
