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
    
    public partial class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string Office { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public Nullable<int> CityID { get; set; }
        public string State { get; set; }
        public string GPS_Postal { get; set; }
        public string Mail_Postal { get; set; }
        public int CountryID { get; set; }
        public string Telephone { get; set; }
    
        public virtual Cities Cities { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual Regions Regions { get; set; }
    }
}
