//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database.Models.EmployeeManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class Country
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public Nullable<int> NumCode { get; set; }
        public int PhoneCode { get; set; }
        public Nullable<bool> EUMember { get; set; }
        public Nullable<bool> IsExempted { get; set; }
        public string CurrencyDescription { get; set; }
    }
}
