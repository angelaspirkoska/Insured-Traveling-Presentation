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
    
    public partial class documents_first_notice_of_loss
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public int First_notice_of_lossID { get; set; }
    
        public virtual document document { get; set; }
        public virtual first_notice_of_loss first_notice_of_loss { get; set; }
    }
}
