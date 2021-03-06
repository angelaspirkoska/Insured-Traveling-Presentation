
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
    using System.Collections.Generic;
    
public partial class first_notice_of_loss
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public first_notice_of_loss()
    {

        this.documents_first_notice_of_loss = new HashSet<documents_first_notice_of_loss>();

        this.first_notice_of_loss_archive = new HashSet<first_notice_of_loss_archive>();

    }


    public int ID { get; set; }

    public int PolicyId { get; set; }

    public int ClaimantId { get; set; }

    public string Relation_claimant_policy_holder { get; set; }

    public int Policy_holder_bank_accountID { get; set; }

    public int Claimant_bank_accountID { get; set; }

    public string Destination { get; set; }

    public System.DateTime Depart_Date_Time { get; set; }

    public System.DateTime Arrival_Date_Time { get; set; }

    public string Transport_means { get; set; }

    public int Additional_infoID { get; set; }

    public float Total_cost { get; set; }

    public Nullable<bool> Web_Mobile { get; set; }

    public Nullable<bool> Short_Detailed { get; set; }

    public string CreatedBy { get; set; }

    public System.DateTime CreatedDateTime { get; set; }

    public Nullable<int> ChatId { get; set; }

    public string ModifiedBy { get; set; }

    public Nullable<System.DateTime> Modified_Datetime { get; set; }

    public string FNOL_Number { get; set; }



    public virtual additional_info additional_info { get; set; }

    public virtual bank_account_info Policy_holder_bank_account_info { get; set; }

    public virtual bank_account_info Claimant_bank_account_info { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<documents_first_notice_of_loss> documents_first_notice_of_loss { get; set; }

    public virtual insured insured { get; set; }

    public virtual travel_policy travel_policy { get; set; }

    public virtual health_insurance health_insurance { get; set; }

    public virtual luggage_insurance luggage_insurance { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<first_notice_of_loss_archive> first_notice_of_loss_archive { get; set; }

    public virtual aspnetuser aspnetuser { get; set; }

}

}
