
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
    
public partial class document
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public document()
    {

        this.documents_first_notice_of_loss = new HashSet<documents_first_notice_of_loss>();

    }


    public int ID { get; set; }

    public string Name { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<documents_first_notice_of_loss> documents_first_notice_of_loss { get; set; }

    public virtual invoice invoice { get; set; }

}

}
