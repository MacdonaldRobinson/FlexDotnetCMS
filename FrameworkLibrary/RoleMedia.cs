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
    
    public partial class RoleMedia
    {
        public long ID { get; set; }
        public long MediaID { get; set; }
        public long RoleID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastModified { get; set; }
    
        public virtual Media Media { get; set; }
        public virtual Role Role { get; set; }
    }
}
