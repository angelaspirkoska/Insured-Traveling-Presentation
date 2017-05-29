﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace InsuredTraveling
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class InsuredTravelingEntity : DbContext
{
    public InsuredTravelingEntity()
        : base("name=InsuredTravelingEntity")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<additional_charge> additional_charge { get; set; }

    public virtual DbSet<additional_info> additional_info { get; set; }

    public virtual DbSet<aspnetrole> aspnetroles { get; set; }

    public virtual DbSet<aspnetuserclaim> aspnetuserclaims { get; set; }

    public virtual DbSet<bank_account_info> bank_account_info { get; set; }

    public virtual DbSet<bank_prefix> bank_prefix { get; set; }

    public virtual DbSet<bank> banks { get; set; }

    public virtual DbSet<client> clients { get; set; }

    public virtual DbSet<company> companies { get; set; }

    public virtual DbSet<country> countries { get; set; }

    public virtual DbSet<discount_age> discount_age { get; set; }

    public virtual DbSet<discount_country> discount_country { get; set; }

    public virtual DbSet<discount_days> discount_days { get; set; }

    public virtual DbSet<discount_family> discount_family { get; set; }

    public virtual DbSet<discount_group> discount_group { get; set; }

    public virtual DbSet<document> documents { get; set; }

    public virtual DbSet<documents_first_notice_of_loss> documents_first_notice_of_loss { get; set; }

    public virtual DbSet<exchange_rate> exchange_rate { get; set; }

    public virtual DbSet<first_notice_of_loss> first_notice_of_loss { get; set; }

    public virtual DbSet<group> groups { get; set; }

    public virtual DbSet<health_insurance> health_insurance { get; set; }

    public virtual DbSet<health_insurance_info> health_insurance_info { get; set; }

    public virtual DbSet<invoice> invoices { get; set; }

    public virtual DbSet<luggage_insurance> luggage_insurance { get; set; }

    public virtual DbSet<luggage_insurance_info> luggage_insurance_info { get; set; }

    public virtual DbSet<min_premium> min_premium { get; set; }

    public virtual DbSet<news_all> news_all { get; set; }

    public virtual DbSet<p_referent> p_referent { get; set; }

    public virtual DbSet<p_stapki> p_stapki { get; set; }

    public virtual DbSet<policy_additional_charge> policy_additional_charge { get; set; }

    public virtual DbSet<policy_insured> policy_insured { get; set; }

    public virtual DbSet<policy_status> policy_status { get; set; }

    public virtual DbSet<policy_type> policy_type { get; set; }

    public virtual DbSet<refreshtoken> refreshtokens { get; set; }

    public virtual DbSet<retaining_risk> retaining_risk { get; set; }

    public virtual DbSet<travel_duration> travel_duration { get; set; }

    public virtual DbSet<travel_insurance_type> travel_insurance_type { get; set; }

    public virtual DbSet<travel_number> travel_number { get; set; }

    public virtual DbSet<type_insured> type_insured { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<aspnetuserlogin> aspnetuserlogins { get; set; }

    public virtual DbSet<transaction> transactions { get; set; }

    public virtual DbSet<payment_status> payment_status { get; set; }

    public virtual DbSet<language> languages { get; set; }

    public virtual DbSet<additional_charge_name> additional_charge_name { get; set; }

    public virtual DbSet<countries_name> countries_name { get; set; }

    public virtual DbSet<policy_type_name> policy_type_name { get; set; }

    public virtual DbSet<retaining_risk_name> retaining_risk_name { get; set; }

    public virtual DbSet<chat_requests> chat_requests { get; set; }

    public virtual DbSet<message> messages { get; set; }

    public virtual DbSet<first_notice_of_loss_archive> first_notice_of_loss_archive { get; set; }

    public virtual DbSet<discount_codes> discount_codes { get; set; }

    public virtual DbSet<aspnetuser> aspnetusers { get; set; }

    public virtual DbSet<ok_setup> ok_setup { get; set; }

    public virtual DbSet<broker_employees> broker_employees { get; set; }

    public virtual DbSet<travel_policy> travel_policy { get; set; }

    public virtual DbSet<insured> insureds { get; set; }

}

}

