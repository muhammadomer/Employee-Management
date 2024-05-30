using Database.DAL;
using Database.Models.EmployeeManagement;
using LogApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace EmployeeManagement
{
    public partial class Employees : System.Web.UI.Page
    {

        public List<Users> users;
        public Employees()
        {

        }
        protected void Page_Load(object sender, EventArgs e)
            {
            
            if (!Page.IsPostBack)
            {
                users = new List<Users>();
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            try
            {   
                if (Session["UserEntity"] != null)
                {
                    Users userEntity = (Users)Session["UserEntity"];

                    if (userEntity.IsActive) {

                        if (userEntity.UserTypeID == 1 || userEntity.UserTypeID == 3)
                        {

                        }
                        else
                        {
                            Response.Redirect("AccessDenied.aspx", false);
                        }

                    }
                    
                    
                    users = new UserDAL().GetAllUsers();
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
        public static List<Users> GetAllUsers()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new UserDAL().GetAllUsers();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        
        [WebMethod]
        [ScriptMethod]
        public static bool DeleteUserByID(int userID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (userID == 1)
                {
                    return false;
                }
                
                return new UserDAL().DeleteUserByID(userID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        
        [WebMethod]
        [ScriptMethod]
        public static string GetProfileImage()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                Users userEntity = GeneralUtilities.GetCurrentUser();

                string path = HttpContext.Current.Server.MapPath(userEntity.ProfileImage);
                string imagePath = "";

                if (!string.IsNullOrWhiteSpace(userEntity.ProfileImage) && !userEntity.ProfileImage.Equals("images/user.png") && File.Exists(path) && new FileInfo(path).Length > 0)
                {
                    imagePath = userEntity.ProfileImage;

                    string completePath = HttpContext.Current.Server.MapPath(imagePath);

                    byte[] imageArray = File.ReadAllBytes(completePath);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    imagePath = "data:image/png;base64," + base64ImageRepresentation;

                }
                else
                {
                    imagePath = "/images/user.png";
                }

                return imagePath;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
    
    }
}