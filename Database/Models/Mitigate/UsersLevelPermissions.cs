//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database.Models.Mitigate
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersLevelPermissions
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> LevelPermissionID { get; set; }
    
        public virtual LevelPermissions LevelPermissions { get; set; }
    }
}
