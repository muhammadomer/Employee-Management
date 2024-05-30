//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database.Models.FileManagement
{
    using System;
    
    public partial class GetAllUsersForFileApproval_Result
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Contact_Number { get; set; }
        public string Job_Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string Office { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string Direct_Number { get; set; }
        public string Mobile_Number { get; set; }
        public string Telephone { get; set; }
        public string Fax_Number { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string GPS_Postal { get; set; }
        public string Mail_Postal { get; set; }
        public string Country { get; set; }
        public int UserTypeID { get; set; }
        public Nullable<bool> Allow_Card { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string ResetHash { get; set; }
        public Nullable<System.DateTime> ResetRequestDate { get; set; }
        public Nullable<System.DateTime> LastPasswordChange { get; set; }
        public int WrongPaswordAttempts { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public string GoogleAuthenticatorSecretKey { get; set; }
        public bool IsGoogleAuthenticatorEnabled { get; set; }
        public string TwoFAEmailCode { get; set; }
        public Nullable<System.DateTime> TwoFAEmailCodeDateTime { get; set; }
    }
}
