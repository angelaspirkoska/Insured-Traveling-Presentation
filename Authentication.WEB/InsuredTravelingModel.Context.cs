﻿//------------------------------------------------------------------------------
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
    
        public virtual DbSet<aspnetrole> aspnetroles { get; set; }
        public virtual DbSet<aspnetuserclaim> aspnetuserclaims { get; set; }
        public virtual DbSet<aspnetuserrole> aspnetuserroles { get; set; }
        public virtual DbSet<aspnetuser> aspnetusers { get; set; }
        public virtual DbSet<client> clients { get; set; }
        public virtual DbSet<client1> clients1 { get; set; }
        public virtual DbSet<eurolinkuser> eurolinkusers { get; set; }
        public virtual DbSet<ok_setup> ok_setup { get; set; }
        public virtual DbSet<p_den> p_den { get; set; }
        public virtual DbSet<p_denovi> p_denovi { get; set; }
        public virtual DbSet<p_doplatoci> p_doplatoci { get; set; }
        public virtual DbSet<p_familija> p_familija { get; set; }
        public virtual DbSet<p_fran> p_fran { get; set; }
        public virtual DbSet<p_grupa> p_grupa { get; set; }
        public virtual DbSet<p_kurs> p_kurs { get; set; }
        public virtual DbSet<p_min_premija> p_min_premija { get; set; }
        public virtual DbSet<p_referent> p_referent { get; set; }
        public virtual DbSet<p_stapki> p_stapki { get; set; }
        public virtual DbSet<p_vozrast> p_vozrast { get; set; }
        public virtual DbSet<p_zem> p_zem { get; set; }
        public virtual DbSet<p_zemja_na_patuvanje> p_zemja_na_patuvanje { get; set; }
        public virtual DbSet<patnicko> patnickoes { get; set; }
        public virtual DbSet<patnicko_vid> patnicko_vid { get; set; }
        public virtual DbSet<refreshtoken> refreshtokens { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<aspnetuserlogin> aspnetuserlogins { get; set; }
        public virtual DbSet<transaction> transactions { get; set; }
        public virtual DbSet<news_all> news_all { get; set; }
        public virtual DbSet<luggage_insurance> luggage_insurance { get; set; }
        public virtual DbSet<first_notice_of_loss> first_notice_of_loss { get; set; }
        public virtual DbSet<health_insurance> health_insurance { get; set; }
    }
}
