using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using Database.Cryptography;
using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;

namespace EmployeeManagement
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CsrfHandler.Validate(this.Page, forgeryToken);

                Users userEntity = GeneralUtilities.GetCurrentUser();
                //string resetPasswordHash = Request.QueryString["id"].Split(',')[0];
                string resetPasswordHash = Request.QueryString["id"];
                LogApp.Log4Net.WriteLog("resetHash: " + resetPasswordHash, LogApp.LogType.GENERALLOG);
                if (resetPasswordHash != null)
                {
                    string userID = Request.QueryString["uid"].Split(',')[0];
                    LogApp.Log4Net.WriteLog("userID: " + userID, LogApp.LogType.GENERALLOG);
                    string decodedUserId = userID;
                    //if (userID.Contains()
                    //{
                        decodedUserId = userID.Replace(" ", "+");
                    //}

                    int mod4 = decodedUserId.Length % 4;
                    if (mod4 > 0)
                    {
                        decodedUserId += new string('=', 4 - mod4);
                    }

                    LogApp.Log4Net.WriteLog("decodedUserId: " + decodedUserId, LogApp.LogType.GENERALLOG);

                    string decryptedUserId = Cryptography.Decrypt(decodedUserId);

                    var splitAccount = decryptedUserId.Split(new string[] { "###" }, StringSplitOptions.None);
                    Session["UserAccountID"] = splitAccount[1];



                    Security manageUserPassword = new Security();
                    ResetPasswordEntity getResetPasswordStatus = manageUserPassword.GetUserIdForPasswordReset(resetPasswordHash);
                    if (getResetPasswordStatus.LinkExpired)
                    {
                        LogApp.Log4Net.WriteLog("Link expired", LogApp.LogType.GENERALLOG);
                        changePasswordCaption.InnerHtml = "<span style='font-weight:bold'>Reset Password: </span>" + getResetPasswordStatus.UserFeedback;
                        //logOutOption.Visible = false;
                        changePasswordArea.Visible = false;
                        Session["UserName"] = "";
                        Session["UserId"] = "";
                    }
                    else
                    {
                        LogApp.Log4Net.WriteLog("Link working", LogApp.LogType.GENERALLOG);
                        //logOutOption.Visible = false;
                        changePasswordCaption.InnerHtml = "<span style='font-weight:bold'>Reset Password: </span> Please review the safety tips and then provide your new password below.";
                        Session["UserName"] = getResetPasswordStatus.UserName;
                        Session["UserId"] = getResetPasswordStatus.UserId;
                    }
                }
                if (userEntity == null && resetPasswordHash == null)
                {
                    if (!Page.IsCallback)
                    {
                        Response.Redirect("Login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                else if (resetPasswordHash == null)
                {
                    if (String.IsNullOrWhiteSpace(userEntity.LastPasswordChange.ToString()))
                    {
                        changePasswordCaption.InnerHtml = "Your Password must be changed before you continue";
                        Session["UserName"] = userEntity.Username;
                        Session["UserId"] = userEntity.ID;
                    }
                }
                string stringVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.ToString();
                lblVersionInfo.InnerText = "v " + stringVersion.Substring(0, stringVersion.LastIndexOf("."));
            }
            catch (Exception ex)
            {
                LogApp.Log4Net.WriteLog("Exception on page load Change Password", LogApp.LogType.ERRORLOG);
                LogApp.Log4Net.WriteException(ex);
            }
        }
        
        [WebMethod]
        [ScriptMethod]
        public static string ResetPassword(string password, int userId)
        {
            string resetStatus = "Sorry, Unable to change your password";
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                var regex = SettingsDAL.regex;

                var match = Regex.Match(password, regex);

                if (match.Success)
                {
                    Users userEntity = GeneralUtilities.GetCurrentUser();
                    if (userEntity != null)
                    {
                        bool isPasswordSame = Cryptography.VerifyHash(password, userEntity.Password);
                        if (isPasswordSame)
                        {
                            return "New Password should not be same as old password";
                        }
                        if (password == userEntity.Username)
                        {
                            return "New Password should not be same as Username";
                        }
                    }
                    Security userSecurityManagement = new Security();
                    if (userSecurityManagement.ChangePassword(password, userId))
                    {
                        //UsersDAL newUserDalObj = new UsersDAL();
                        HttpContext.Current.Session["UserEntity"] = null;
                        HttpContext.Current.Session["UserName"] = null;
                        HttpContext.Current.Session["UserId"] = null;
                        resetStatus = "Password changed sucessfully";
                    }
                    return resetStatus;
                }
                else
                {
                    resetStatus = "Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.";
                    return resetStatus;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}