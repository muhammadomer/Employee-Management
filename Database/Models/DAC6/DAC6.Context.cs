﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database.Models.DAC6
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DAC6Entities : DbContext
    {
        public DAC6Entities()
            : base("name=DAC6Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DAC6Modules> DAC6Modules { get; set; }
        public virtual DbSet<DAC6UsersModules> DAC6UsersModules { get; set; }
        public virtual DbSet<LevelPermission> LevelPermission { get; set; }
        public virtual DbSet<UserLevelPermission> UserLevelPermission { get; set; }
    
        public virtual int ChangeHashCode(string oldKey, string newKey)
        {
            var oldKeyParameter = oldKey != null ?
                new ObjectParameter("OldKey", oldKey) :
                new ObjectParameter("OldKey", typeof(string));
    
            var newKeyParameter = newKey != null ?
                new ObjectParameter("NewKey", newKey) :
                new ObjectParameter("NewKey", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ChangeHashCode", oldKeyParameter, newKeyParameter);
        }
    }
}
