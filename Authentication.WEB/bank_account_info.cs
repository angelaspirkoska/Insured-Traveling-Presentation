
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
    
public partial class bank_account_info
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public bank_account_info()
    {

        this.first_notice_of_loss = new HashSet<first_notice_of_loss>();

        this.first_notice_of_loss1 = new HashSet<first_notice_of_loss>();

        this.first_notice_of_loss_archive = new HashSet<first_notice_of_loss_archive>();

        this.first_notice_of_loss_archive1 = new HashSet<first_notice_of_loss_archive>();

    }


    public int ID { get; set; }

    public int Account_HolderID { get; set; }

    public string Account_Number { get; set; }

    public int BankID { get; set; }



    public virtual insured insured { get; set; }

    public virtual bank bank { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<first_notice_of_loss> first_notice_of_loss { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<first_notice_of_loss> first_notice_of_loss1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<first_notice_of_loss_archive> first_notice_of_loss_archive { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<first_notice_of_loss_archive> first_notice_of_loss_archive1 { get; set; }

}

}
