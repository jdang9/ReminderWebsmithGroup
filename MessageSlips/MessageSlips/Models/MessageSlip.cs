//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessageSlips.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MessageSlip
    {
        public int mId { get; set; }
        public string Sender { get; set; }
        public string TimeDate { get; set; }
        public string PhoneNum { get; set; }
        public string MessageTask { get; set; }
        public string Location { get; set; }
        public string Other { get; set; }
        public int UserID { get; set; }
    
        public virtual User User { get; set; }
    }
}
