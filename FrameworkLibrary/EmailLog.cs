//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrameworkLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailLog
    {
        public long ID { get; set; }
        public string SenderName { get; set; }
        public string SenderEmailAddress { get; set; }
        public string ToEmailAddresses { get; set; }
        public string FromEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string VisitorIP { get; set; }
        public string RequestUrl { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastModified { get; set; }
        public string ServerMessage { get; set; }
    }
}
