﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IConfigPolicyValuesService
    {
        bool AddConfigPolicyValues(List<config_policy_values> values);
    }
}
