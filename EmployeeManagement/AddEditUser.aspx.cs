using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace EmployeeManagement
{
    public partial class AddEditUser : System.Web.UI.Page
    {
        public AddEditUser()
        {

        }
        public Users user;
        public List<OfficeEntity> offices;
        public List<Departments> Departments;
        public List<PracticeGroups> PracticeGroups;
        public List<PermissionEntity> FileRepositoryModulePermissions
        , FileRepositoryFileCategoryPermissions
        , FileRepositoryRepositoryPermissions
        , RiskManagerModulePermissions
        , RiskManagerBoardList
        , RiskManagerUserLevelPermissions
        , BusinessCardsPermissions
        , DAC6UserLevelPermissions
        , DAC6Permissions;
        public List<ApplicationEntity> ApplicationList;
        public int IsBusinessCardAllowed = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            try
            {


                if (Session["UserEntity"] != null)
                {
                    Users userEntity = new Users();
                    userEntity = GeneralUtilities.GetCurrentUser();

                    if (userEntity.IsActive)
                    {

                        if (userEntity.UserTypeID == 1 || userEntity.UserTypeID == 3)
                        {

                        }
                        else
                        {
                            Response.Redirect("AccessDenied.aspx", false);
                        }

                    }
                    UserDAL _userDAL = new UserDAL();
                    //if (userEntity.UserTypeID == 2)
                    {


                        ApplicationList = _userDAL.GetApplicationsList();
                        if (ApplicationList != null)
                            foreach (var application in ApplicationList)
                            {
                                if (application.ID == 1)
                                {
                                    FileRepositoryModulePermissions = _userDAL.GetPermissionsList();
                                    FileRepositoryFileCategoryPermissions = _userDAL.GetFilePermissionsList();
                                    FileRepositoryRepositoryPermissions = _userDAL.GetRepositoryPermissionsList();
                                }
                                else if (application.ID == 2)
                                {
                                    RiskManagerModulePermissions = _userDAL.GetRiskManagerModulePermissionsList();
                                    RiskManagerBoardList = _userDAL.GetAllRiskManagerUserBoards();
                                    RiskManagerUserLevelPermissions = _userDAL.GetAllRiskManagerUserLevelPermissions();
                                }
                                else if (application.ID == 3)
                                {
                                    BusinessCardsPermissions = _userDAL.GetAllBusinessCardsPermissionsList();
                                }
                                else if (application.ID == 4)
                                {
                                    DAC6Permissions = _userDAL.GetAllDAC6PermissionsList();
                                    DAC6UserLevelPermissions = _userDAL.GetAllADAC6UserLevelPermissions();
                                }
                            }
                    }
                    offices = new LocationDAL().GetAllLocations();
                    Departments = new SettingsDAL().GetAllDepartment();
                    PracticeGroups = new SettingsDAL().GetAllPracticeGroups();
                    
                    user = GeneralUtilities.GetCurrentUser();

                    if (ApplicationList != null)
                    {
                        foreach (var item in ApplicationList)
                        {
                            if (item.ID == 3)
                            {
                                IsBusinessCardAllowed = 1;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string Inser_User(Users userEntity)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                return new UserDAL().AddUser(userEntity);

                //if (userEntity.ProfileImage.ToLower().EndsWith(".jpg") || userEntity.ProfileImage.ToLower().EndsWith(".png") || userEntity.ProfileImage.ToLower().EndsWith(".gif") || userEntity.ProfileImage.ToLower().EndsWith(".jpeg"))
                //{

                //}
                //else
                //{
                //    string response = "Please upload only image file";
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string Update_User(Users userEntity, bool IsPermissions)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (userEntity.ID == 1)
                {
                    //return "You Cannot Update This User";
                }
                return new UserDAL().UpdateUser(userEntity, true);
                //if (userEntity.ProfileImage.ToLower().EndsWith(".jpg") || userEntity.ProfileImage.ToLower().EndsWith(".png") || userEntity.ProfileImage.ToLower().EndsWith(".jpeg"))
                //{

                //}
                //else
                //{
                //    string response = "Please upload only image file";
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static UserEntity GetUserInfoByID(string ID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (!string.IsNullOrEmpty(ID))
                {
                    ID = ID.Replace(" ", "+");
                    byte[] data = Convert.FromBase64String(ID.ToString());
                    ID = Encoding.UTF8.GetString(data).Split('>')[1];
                }

                int empID = Convert.ToInt32(ID);
                return new UserDAL().GetUserInfoByID(empID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string SendResetPasswordLink(string userName)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                string accountID = HttpContext.Current.Session["UserAccountID"].ToString();

                //if (ConfigurationManager.AppSettings["onPrem"] == "0")
                //{
                //    HttpContext.Current.Session["UserAccountID"] = accountID;
                //}
                //else
                //{
                //    accountID = "";
                //    HttpContext.Current.Session["UserAccountID"] = "";
                //}

                Security security = new Security();
                return security.SendResetPasswordLink(userName, accountID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

    }
}