using Database.DAL;
using Database.Models.EmployeeManagement;
using LogApp;
using System;
using System.Web;
using System.Web.Helpers;

namespace EmployeeManagement
{
    public partial class Employee : System.Web.UI.MasterPage
    {
        private SettingsDAL _settingsDAL;
        public string userName;
        public string supportEmails;
        public string userEmail;
        public Employee()
        {
            
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!Page.IsPostBack)
            {
                _settingsDAL = new SettingsDAL();
            }

            try
            {
                CsrfHandler.Validate(this.Page, forgeryToken);

                Security security = new Security();
                
                applications.Visible = false;
                users.Visible = false;
                offices.Visible = false;
                settings.Visible = false;
                license.Visible = false;
                Users userEntity = new Users();
                userEntity = (Users)Session["UserEntity"];
                if (userEntity.First_Name != null)
                {
                    if (_settingsDAL.IsEnabledTwoFA())
                    {
                        btnEnableAuthentication.Attributes.Remove("class");
                        btnEnableAuthentication.InnerText = "Enable Authenticator";
                        if (security.If2FAEnabled(userEntity.ID))
                        {
                            btnEnableAuthentication.InnerText = "Disable Authenticator";
                        }
                    }
                    else
                    {
                        btnEnableAuthentication.Attributes.Add("class","hidden");
                    }
                    userName = userEntity.First_Name + " " + userEntity.Last_Name;
                    userEmail = userEntity.Email;
                    supportEmails = userEntity.SupportEmails;
                    if (userEntity.UserTypeID == 1)
                    {
                        users.Visible = true;
                        offices.Visible = true;
                        settings.Visible = true;
                        license.Visible = true;

                    }
                    else if (userEntity.UserTypeID == 2)
                    {
                        applications.Visible = true;
                    }
                    else if (userEntity.UserTypeID == 3)
                    {
                        users.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }
    }
}