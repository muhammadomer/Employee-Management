//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersApplications
    {
        public int ID { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> Application_ID { get; set; }
    
        public virtual Applications Applications { get; set; }
        public virtual Users Users { get; set; }
    }
}
