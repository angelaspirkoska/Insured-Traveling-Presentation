﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using InsuredTraveling.DI;

namespace InsuredTraveling
{
    public class ServicesRegistration : Module
    {

        private string connStr;
        public ServicesRegistration(string connString)
        {
            this.connStr = connString;
        }
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<RolesService>().As<IRolesService>().InstancePerRequest();
            builder.RegisterType<OkSetupService>().As<IOkSetupService>().InstancePerRequest();
            builder.RegisterType<InsuredsService>().As<IInsuredsService>().InstancePerRequest();
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerRequest();
            builder.RegisterType<PolicyService>().As<IPolicyService>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<FirstNoticeOfLossService>().As<IFirstNoticeOfLossService>().InstancePerRequest();
            builder.RegisterType<PolicyTypeService>().As<IPolicyTypeService>().InstancePerRequest();
            builder.RegisterType<LuggageInsuranceService>().As<ILuggageInsuranceService>().InstancePerRequest();
            builder.RegisterType<TransactionsService>().As<ITransactionsService>().InstancePerRequest();
            builder.RegisterType<HealthInsurance>().As<IHealthInsurance>().InstancePerRequest();
            builder.RegisterType<AdditionalChargesService>().As<IAdditionalChargesService>().InstancePerRequest();
            builder.RegisterType<FranchiseService>().As<IFranchiseService>().InstancePerRequest();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerRequest();
            builder.RegisterType<PolicyInsuredService>().As<IPolicyInsuredService>().InstancePerRequest();
            builder.RegisterType<BankAccountService>().As<IBankAccountService>().InstancePerRequest();           

            base.Load(builder);


        }
    }

}