﻿using Base32;
using Database.Cryptography;
using Database.DAL;
using Database.Models.EmployeeManagement;
using LogApp;
using OtpSharp;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using Secure;
using System.Web.Services;
using System.Drawing;

namespace EmployeeManagement
{
    public partial class DevLogin : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString != null && Request.QueryString.Count > 0)
                {
                    string queryFromPrevUrl = "";
                    queryFromPrevUrl = Request.QueryString[0];
                    queryFromPrevUrl = queryFromPrevUrl.Replace(" ", "+");

                    Session["QueryFromURL"] = queryFromPrevUrl;
                }
                else
                {
                    Session["QueryFromURL"] = "";
                }

                if (!Page.IsPostBack)
                {
                    try
                    {
                        string srcPath = HttpContext.Current.Server.MapPath("images/0/");
                        string destPath = HttpContext.Current.Server.MapPath("images/");

                        string fileName = "user.png";
                        bool CopyFiles = false;
                        using (Image imgSrc = Image.FromFile(srcPath + fileName))
                        {
                            using (Image imgDest = Image.FromFile(destPath + fileName))
                            {
                                if (imgSrc.Width != imgDest.Width || imgSrc.Height != imgDest.Height)
                                {
                                    CopyFiles = true;
                                }
                            }
                        }
                        if (CopyFiles)
                        {
                            System.IO.File.Copy(srcPath + fileName, destPath + fileName, true);
                            fileName = "heading-image.png";
                            System.IO.File.Copy(srcPath + fileName, destPath + fileName, true);
                            fileName = "favicon.ico";
                            System.IO.File.Copy(srcPath + fileName, destPath + fileName, true);
                            fileName = "lock.png";
                            System.IO.File.Copy(srcPath + fileName, destPath + fileName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }

                Users userEntity = (Users)Session["UserEntity"];
                NameValueCollection nvc = Request.Form;
                if (!string.IsNullOrEmpty(nvc["AccountId"]))
                {
                    Log4Net.WriteLog("Account Manage Request From Cloud Admin", LogType.GENERALLOG);
                    string accountDetailEncrypted = nvc["AccountId"];
                    string accountDetailDecrypted = Cryptography.Decrypt(accountDetailEncrypted);

                    string password = accountDetailDecrypted.Split(new string[] { "$$$" }, StringSplitOptions.None)[0];
                    string userName = accountDetailDecrypted.Split(new string[] { "$$$" }, StringSplitOptions.None)[1];
                    string userID = accountDetailDecrypted.Split(new string[] { "$$$" }, StringSplitOptions.None)[2];
                    if (ConfigurationManager.AppSettings["onPrem"] == "1")
                    {
                        Response.Redirect("Login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else if (ConfigurationManager.AppSettings["onPrem"] == "0")
                    {
                        Session["ManageAccount"] = "1";
                        Session["UserAccountID"] = userID;
                        Users userDetail = new UserDAL().UserAuthenticationForCloud(userName);
                        if (userDetail.First_Name != null)
                        {
                            Session["UserEntity"] = userDetail;
                            Response.Redirect("Login.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                }
                else
                {

                    //CsrfHandler.Validate(this.Page, forgeryToken);
                    if (Session["UserEntity"] != null)
                    {
                        if (!String.IsNullOrWhiteSpace(userEntity.LastPasswordChange.ToString()))
                        {
                            if (userEntity.IsActive && userEntity.UserTypeID == 2)
                            {
                                //U0123:Removing DirectDAC6
                                //if (Convert.ToInt32(ConfigurationManager.AppSettings["DirectDAC6"]) == 1)
                                //{
                                //    Response.Redirect("ManageApplication.aspx?appID=4", false);
                                //}
                                //else
                                //{
                                Response.Redirect("Applications.aspx", false);

                                //}
                                Context.ApplicationInstance.CompleteRequest();
                            }
                            else if (userEntity.IsActive && userEntity.UserTypeID == 1)
                            {
                                Response.Redirect("Employees.aspx", false);
                                //Context.ApplicationInstance.CompleteRequest();
                            }
                        }
                        else
                        {
                            if (userEntity.ID == 1)
                            {
                                //Generate Trial License incase if changing the password first time.
                                string ConnectionString = GeneralUtilities.GetConnectionString("DentonsEmployees");
                                string ClientDB = GeneralUtilities.GetDatabaseName("DentonsEmployees");
                                LicInformation objLicense = new LicInformation(ConnectionString, ClientDB);
                                objLicense.GenerateTrialLicenseModule(30, LicInformation.ServerLicense.MitigateServer);
                            }

                            if (Session["ManageAccount"] != null && Session["ManageAccount"].ToString() == "1")
                            {
                                Response.Redirect("Employees.aspx", false);
                            }
                            else
                            {
                                Response.Redirect("ChangePassword.aspx", false);
                            }
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    else if (!Convert.ToBoolean(ConfigurationManager.AppSettings["ServerDown"]))
                    {
                       
                       
                        if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                        {
                            string URL = "Default.aspx?id=" + Session["QueryFromURL"];
                            if (string.IsNullOrEmpty(Session["QueryFromURL"].ToString()))
                                URL = "Default.aspx";
                            Response.Redirect(URL, false);
                        }
                        else
                        {
                            string URL = "Login.aspx?id=" + Session["QueryFromURL"];
                            if (string.IsNullOrEmpty(Session["QueryFromURL"].ToString()))
                                URL = "Login.aspx";
                            Response.Redirect(URL, false);
                        }
                           
                    }
                    
                }



            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string LoginUser(string userName, string password, string accountID)
        {
            //try
            //{

            //    try
            //    {
            //        CsrfHandler.ValidateAntiForgery(HttpContext.Current);
            //    }
            //    catch (Exception ex) { Log4Net.WriteException(ex); }

            //    if (ConfigurationManager.AppSettings["onPrem"] == "0")
            //    {
            //        HttpContext.Current.Session["UserAccountID"] = accountID;
            //    }
            //    else
            //    {
            //        HttpContext.Current.Session["UserAccountID"] = "";
            //    }

            //    string feedback = "";
            //    UserDAL userDAL = new UserDAL();
            //    Users userEntity = new Users();
            //    if (!userDAL.IsAccountDeleted())
            //    {
            //        userEntity = userDAL.CheckUserAuthentication(userName, password);
            //        feedback = userDAL.LoginUser(userEntity);

            //        //U0123:Removing DirectDAC6
            //        //if (feedback == "4" && Convert.ToInt32(ConfigurationManager.AppSettings["DirectDAC6"]) == 1)
            //        if (feedback == "4")
            //        {
            //            string ApplicationID = "";
            //            if (!string.IsNullOrEmpty(HttpContext.Current.Session["QueryFromURL"].ToString()))
            //            {
            //                ApplicationID = HttpContext.Current.Session["QueryFromURL"].ToString().Split('!')[0];
            //                feedback = "6:" + ApplicationID;
            //            }
            //            //else { feedback = "6:4"; }
            //        }
            //    }
            //    else
            //    {
            //        feedback = "Account ID not found.";
            //    }
            //    return feedback;
            //}
            //catch (Exception ex)
            //{
            //    Log4Net.WriteException(ex);
            //    throw ex;
            //}


            try
            {
                try
                {
                    CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                }
                catch (Exception ex) { Log4Net.WriteException(ex); }

                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    HttpContext.Current.Session["UserAccountID"] = accountID;
                }
                else
                {
                    HttpContext.Current.Session["UserAccountID"] = "";
                }

                string feedback = "";
                UserDAL userDAL = new UserDAL();
                Users userEntity = new Users();
                if (!userDAL.IsAccountDeleted())
                {
                    userEntity = userDAL.CheckUserAuthentication(userName, password);
                    feedback = userDAL.LoginUser(userEntity);
                    //U0123:Removing DirectDAC6
                    //  if (feedback == "4" && Convert.ToInt32(ConfigurationManager.AppSettings["DirectDAC6"]) == 1)



                    if (feedback == "4")
                    {
                        string ApplicationID = "";
                        if (!string.IsNullOrEmpty(HttpContext.Current.Session["QueryFromURL"].ToString()))
                        {
                            ApplicationID = HttpContext.Current.Session["QueryFromURL"].ToString().Split('!')[0];
                            feedback = "6:" + ApplicationID;
                        }
                        else
                        {
                            HttpContext.Current.Session["Email"] = userEntity.Email;
                            HttpContext.Current.Session["Password"] = password;

                            if (Convert.ToInt32(ConfigurationManager.AppSettings["DirectMitigate"]) == 1)
                            {
                                feedback = "6";
                            }
                        }
                        //  else { feedback = "6:4"; }
                    }
                    else if (feedback == "2")
                    {
                        HttpContext.Current.Session["Email"] = userEntity.Email;
                        HttpContext.Current.Session["Password"] = password;
                    }
                }
                else
                {
                    feedback = "Account ID not found.";
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }


        }

        [WebMethod]
        [ScriptMethod]
        public static string GetUserEmail(string userName, string password)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                UserDAL userDAL = new UserDAL();
                Users userEntity = new Users();
                userEntity = userDAL.CheckUserAuthentication(userName, password);
                return userEntity.Email;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string VerifyCodeOnLogin(string userName, string password, string accountID, string code)
        {
            try
            {
                try
                {
                    CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                }
                catch (Exception ex) { Log4Net.WriteException(ex); }


                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    HttpContext.Current.Session["UserAccountID"] = accountID;
                }
                else
                {
                    HttpContext.Current.Session["UserAccountID"] = "";
                }

                UserDAL userDAL = new UserDAL();
                Users userEntity = userDAL.CheckUserAuthentication(userName, password);
                string feedBack = "Invalid verification code";
                if (userEntity.TwoFactorEnabled)
                {
                    byte[] secretKey = Base32Encoder.Decode(userEntity.GoogleAuthenticatorSecretKey);
                    long timeStepMatched = 0;
                    var otp = new Totp(secretKey);
                    if (otp.VerifyTotp(code.Trim(), out timeStepMatched, new VerificationWindow(2, 2)))
                    {
                        Log4Net.WriteLog("Verified 2FA Code", LogType.GENERALLOG);
                        HttpContext.Current.Session["UserEntity"] = userEntity;
                        if (userEntity.UserTypeID == 1)
                        {
                            feedBack = "3";
                        }
                        else if (userEntity.UserTypeID == 2)
                        {
                            feedBack = "4";
                        }
                    }
                }
                else
                {
                    if (userDAL.VerifyEmailCode(userEntity, code))
                    {
                        HttpContext.Current.Session["UserEntity"] = userEntity;
                        if (userEntity.UserTypeID == 1)
                        {
                            feedBack = "3";
                        }
                        else if (userEntity.UserTypeID == 2)
                        {
                            feedBack = "4";
                        }
                    }
                }

                //U0123:Removing DirectDAC6
                //if (feedBack == "4" && Convert.ToInt32(ConfigurationManager.AppSettings["DirectDAC6"]) == 1)
                if (feedBack == "4")
                {
                    string ApplicationID = "";
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session["QueryFromURL"].ToString()))
                    {
                        ApplicationID = HttpContext.Current.Session["QueryFromURL"].ToString().Split('!')[0];
                        feedBack = "6:" + ApplicationID;
                    }
                    //else { feedBack = "6:4"; }
                }
                return feedBack;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool Reset2FA(string userName, string password)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                UserDAL userDAL = new UserDAL();
                Users userEntity = userDAL.CheckUserAuthentication(userName, password);
                return userDAL.Reset2FA(userEntity);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string ChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (!string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword) && !string.IsNullOrWhiteSpace(confirmNewPassword))
                {
                    if (newPassword.Equals(confirmNewPassword))
                    {
                        var regex = SettingsDAL.regex;

                        var match = Regex.Match(newPassword, regex);

                        if (match.Success)
                        {
                            UserDAL userDAL = new UserDAL();
                            return userDAL.ChangePassword(oldPassword, newPassword, confirmNewPassword);
                        }
                        else
                        {
                            string response = "Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.";
                            return response;
                        }
                    }
                    else
                    {
                        string response = "The passwords you supplied do not match.";
                        return response;
                    }
                }
                else
                {
                    string response = "Please provide all manadatory fields.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

    }
}