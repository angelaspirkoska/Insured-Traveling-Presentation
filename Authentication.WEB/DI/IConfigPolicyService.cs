﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IConfigPolicyService
    {
        int AddConfigPolicy(int idConfigPolicyType, string rating, DateTime StartDate, DateTime EndDate, bool isPaid);
        bool UpdateConfigPolicy(int idPolicy, string rating);
    }
}
