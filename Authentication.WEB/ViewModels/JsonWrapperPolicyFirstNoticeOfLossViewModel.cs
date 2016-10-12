using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using AutoMapper;

namespace InsuredTraveling.ViewModels
{
    [Serializable]
    public class JsonWrapperPolicyFirstNoticeOfLossViewModel
    {
        private ClientViewModel PolicyHolder = new ClientViewModel();
        private List<BankAccountInfoViewModel> PolicyHolderBankAccounts = new List<BankAccountInfoViewModel>();
        private Collection<ClientViewModel> PolicyClients = new Collection<ClientViewModel>();
        private List<BankPrefixViewModel> BankPrefixes = new List<BankPrefixViewModel>();

        public JsonWrapperPolicyFirstNoticeOfLossViewModel()
        {

        }

        public JsonWrapperPolicyFirstNoticeOfLossViewModel(travel_policy policy, List<bank_prefix> bankPrefixes )
        {
            //PolicyHolder = Mapper.Map<insured, ClientViewModel>(policy.insured);
            PolicyHolder = new ClientViewModel
            {
                Id = policy.insured.ID,
                FirstName = policy.insured.Name,
                Lastname = policy.insured.Lastname,
                SSN = policy.insured.SSN,
                Address = policy.insured.Address,
                City = policy.insured.City,
                Email = policy.insured.Email,
                PhoneNumber = policy.insured.Phone_Number
            };

            foreach (var policyInsured in policy.policy_insured)
            {
                PolicyClients.Add(new ClientViewModel
                {
                    Id = policyInsured.insured.ID,
                    FirstName = policyInsured.insured.Name,
                    Lastname = policyInsured.insured.Lastname
                });
            }
            foreach (var bankPrefix in bankPrefixes)
            {
                BankPrefixes.Add(new BankPrefixViewModel
                {
                    Prefix = bankPrefix.Prefix_Number,
                    BankName =  bankPrefix.bank.Name
                });
            }
            //foreach (var bankAccountInfo in policy.insured.bank_account_info)
            //{
            //    PolicyHolderBankAccounts.Add(new BankAccountInfoViewModel
            //    {
            //        Id = bankAccountInfo.ID,
            //        BankName = bankAccountInfo.bank.Name,
            //        AccountNumber = bankAccountInfo.Account_Number
            //    });
            //}
        }
    }
}