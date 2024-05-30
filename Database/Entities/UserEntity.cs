using Database.Models.Mitigate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {
            FileRepositoryModulePermissionsList = new List<PermissionEntity>();
            FileTypePermissionsList = new List<PermissionEntity>();
            FileRepositoryPermissionsList = new List<PermissionEntity>();
            MitigateModulePermissionsList = new List<PermissionEntity>();
            MitigateBoardsList = new List<PermissionEntity>();
            RiskManagerUserLevelPermissionsList = new List<UsersLevelPermissions>();
            DAC6UserLevelPermissionsList = new List<Database.Models.DAC6.UserLevelPermission>();
            DAC6ModulesPermissionsList = new List<PermissionEntity>();
        }
        public int UserID { get; set; }
        public int OfficeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContacNumber { get; set; }
        public string JobTitle { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Office { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string PracticeGroup { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string GPSPostal { get; set; }
        public string MailPostal { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Telephone { get; set; }
        public string DirectNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string UserType { get; set; }
        public string ProfileImage { get; set; }
        public string LastPasswordChange { get; set; }
        public int UserTypeID { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool AllowCard { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int WrongPaswordAttempts { get; set; }
        public int TCLevelPermissionId { get; set; }
        public bool UserAuthenticated { get; set; }
        public List<ApplicationEntity> ApplcaiotnsList { get; set; }
        public List<PermissionEntity> FileRepositoryModulePermissionsList { get; set; }
        public List<PermissionEntity> FileTypePermissionsList { get; set; }
        public List<PermissionEntity> FileRepositoryPermissionsList { get; set; }
        public List<PermissionEntity> MitigateModulePermissionsList { get; set; }
        public List<PermissionEntity> MitigateBoardsList { get; set; }
        public List<UsersLevelPermissions> RiskManagerUserLevelPermissionsList { get; set; }
        public List<PermissionEntity> BusinessCardsModulesPermissionsList { get; set; }
        public List<PermissionEntity> DAC6ModulesPermissionsList { get; set; }

        public List<Database.Models.DAC6.UserLevelPermission> DAC6UserLevelPermissionsList { get; set; }
    }
}
