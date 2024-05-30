using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;
using Secure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services;
using System.Configuration;

namespace EmployeeManagement
{
    public partial class Applications : System.Web.UI.Page
    {
        public Applications()
        {
            
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {



                CsrfHandler.Validate(this.Page, forgeryToken);


                if (ConfigurationManager.AppSettings["DirectMitigate"] == "0")
                {

                    if (Session["UserEntity"] != null)
                    {

                        Security security = new Security();
                        Users userEntity = new Users();
                        userEntity = GeneralUtilities.GetCurrentUser();
                        if (new SettingsDAL().IsEnabledTwoFA())
                        {
                            btnEnableAuthentication.Visible = btnEnableAuthentication1.Visible = true;
                            btnEnableAuthentication.InnerText = btnEnableAuthentication1.InnerText = "Enable Authenticator";
                            if (security.If2FAEnabled(userEntity.ID))
                            {
                                btnEnableAuthentication.InnerText = btnEnableAuthentication1.InnerText = "Disable Authenticator";
                            }
                        }
                        else
                        {
                            btnEnableAuthentication.Visible = btnEnableAuthentication1.Visible = false;
                        }
                        if (userEntity.IsActive && userEntity.UserTypeID != 2)
                        {
                            Response.Redirect("AccessDenied.aspx", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("Login.aspx");
                }

            }
           
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<ApplicationEntity> GetAllUserApplication()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                Users userEntity = new Users();
                userEntity = GeneralUtilities.GetCurrentUser();
                List<ApplicationEntity> applicationList = new List<ApplicationEntity>();
                applicationList = new UserDAL().GetUserApplicationsListByID(userEntity.ID);
                return applicationList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

  
        [WebMethod]
        [ScriptMethod]
        public static string LogOut()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                string redirectPage = null;
                Users userEntity = (Users)HttpContext.Current.Session["UserEntity"];
                if (userEntity.UserTypes.User_Type != null && userEntity.UserTypeID == 1)
                {
                    HttpContext.Current.Session["UserEntity"] = null;
                    redirectPage = "Login.aspx";
                }
                else if (userEntity.UserTypes.User_Type != null && userEntity.UserTypeID == 2)
                {
                    HttpContext.Current.Session["UserEntity"] = null;
                    redirectPage = "Login.aspx";
                }
                HttpContext.Current.Session.Abandon();
                return redirectPage;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static Users GetUserProfile()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                Users userEntity = new Users();
                userEntity = GeneralUtilities.GetCurrentUser();

                Users userProfile = new Users();
                userProfile.ID = userEntity.ID;
                userProfile.First_Name = userEntity.First_Name;
                userProfile.Last_Name = userEntity.Last_Name;
                userProfile.Direct_Number = userEntity.Direct_Number;
                userProfile.Mobile_Number = userEntity.Mobile_Number;
                userProfile.Fax_Number = userEntity.Fax_Number;

                string completePath = HttpContext.Current.Server.MapPath(userEntity.ProfileImage);

                if (!userEntity.ProfileImage.Equals("images/user.png") && File.Exists(completePath) && new FileInfo(completePath).Length > 0)
                {
                    byte[] imageArray = File.ReadAllBytes(completePath);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    userProfile.ProfileImage = "data:image/png;base64," + base64ImageRepresentation;
                }
                else
                {
                    userProfile.ProfileImage = userEntity.ProfileImage;
                }

                return userProfile;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string Update_User(Users userEntity)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                

                Users userEntitySession = (Users)HttpContext.Current.Session["UserEntity"];
                userEntitySession.First_Name = userEntity.First_Name;
                userEntitySession.Last_Name = userEntity.Last_Name;
                userEntitySession.Direct_Number = userEntity.Direct_Number;
                userEntitySession.Mobile_Number = userEntity.Mobile_Number;
                userEntitySession.Fax_Number = userEntity.Fax_Number;
                userEntitySession.ProfileImage = userEntity.ProfileImage;
                HttpContext.Current.Session["UserEntity"] = userEntitySession;
                //userEntitySession.ProfileImage = userEntity.ProfileImage;
                return new UserDAL().UpdateUserLogin(userEntity);
                //if (userEntity.ProfileImage.ToLower().EndsWith(".jpg") || userEntity.ProfileImage.ToLower().EndsWith(".png") || userEntity.ProfileImage.ToLower().EndsWith(".jpeg"))
                //{
                    
                //}
                //else
                //{
                //    string response = "Only jpg, jpeg and png file types allowed.";
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

    }
}