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
    
    public partial class message
    {
        public int ID { get; set; }
        public int ConversationID { get; set; }
        public string Text { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string from_username { get; set; }
    
        public virtual conversation conversation { get; set; }
        public virtual chat_requests chat_requests { get; set; }
    }
}
