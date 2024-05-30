using Database.Entities;
using Database.Models.EmployeeManagement;
using Database.Models.Mitigate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.EmployeeManagement
{
    partial class Users
    {
        public string UserTypeName { get; set; }
        public string Name { get; set; }
        public bool UserAuthenticated { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool UploadImage { get; set; }
        public bool RemoveImage { get; set; }
        public string SupportEmails { get; set; }
        public int CompanyId { get; set; }
        public UserAddModel UserAdd { get; set; }
        public UserAddModel UserEdit { get; set; }
        public List<Applications> ApplcaiotnsList { get; set; }
        public List<PermissionEntity> FileRepositoryModulePermissionsList { get; set; }
        public List<PermissionEntity> FileTypePermissionsList { get; set; }
        public List<PermissionEntity> FileRepositoryPermissionsList { get; set; }
        public List<PermissionEntity> MitigateModulePermissionsList { get; set; }
        public List<PermissionEntity> MitigateBoardsList { get; set; }
        public List<UsersLevelPermissions> RiskManagerLevelPermissions { get; set; }
        public List<PermissionEntity> BusinessCardsModulesPermissionsList { get; set; }
        public List<PermissionEntity> DAC6ModulesPermissionsList { get; set; }
        public List<Database.Models.DAC6.UserLevelPermission> DAC6UserLevelPermissionsList { get; set; }
    }
    public class UserAddModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public bool IsBackupAdmin { get; set; }
    }
}
