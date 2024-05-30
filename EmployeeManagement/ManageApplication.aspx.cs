using Database.Cryptography;
using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using Database.Models.SinglePointCloud;
using LogApp;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace EmployeeManagement
{
    public partial class ManageApplication : System.Web.UI.Page
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

                if (Session["UserEntity"] != null)
                {
                    string SecureLicense = "1";
                    try
                    {
                        SecureLicense = ConfigurationManager.AppSettings["SecureLicense"];
                    }
                    catch { SecureLicense = "1"; }
                    int DBId = 0;
                    SinglePoint_CloudEntities singlePointCloudEntities = new SinglePoint_CloudEntities();
                    Users userEntity = new Users();
                    userEntity = GeneralUtilities.GetCurrentUser();
                    if (userEntity.UserTypeID != 2)
                    {
                        Response.Redirect("AccessDenied.aspx", false);
                        //Response.Redirect("Applications.aspx", false);
                        return;
                    }
                    int appID = Convert.ToInt32(Request.QueryString["appID"].Split(',')[0]);

                    ApplicationEntity application = new ApplicationEntity();
                    try
                    {
                        application = GeneralUtilities.GetApplicationUrlByID(appID);
                        if (application == null || application.ID == 0)
                        {
                            Log4Net.WriteLog("Application is not active OR not found with ID  : " + appID, LogType.GENERALLOG);
                            Response.Redirect("AccessDenied.aspx", false);
                            //Response.Redirect("Applications.aspx", false);
                            return;
                        }
                        else
                        {
                            bool IsApplicationAssignedToUser = new UserDAL().IsApplicationAssignedToUser(userEntity.ID, appID);
                            if(!IsApplicationAssignedToUser)
                            {
                                Log4Net.WriteLog("Application not active for this user : " + userEntity.Username, LogType.GENERALLOG);
                                Response.Redirect("AccessDenied.aspx", false);
                                return;
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        
                    }

                    string QueryFromURL = "";
                    try
                    {
                        //Log4Net.WriteLog("QueryFromURL: " + QueryFromURL, LogType.GENERALLOG);
                        //QueryFromURL = Session["QueryFromURL"].ToString();
                        //QueryFromURL = QueryFromURL.Substring(QueryFromURL.IndexOf('!') + 1);
                        //Log4Net.WriteLog("sending QueryFromURL: " + QueryFromURL, LogType.GENERALLOG);
                    }
                    catch{ }

                    if (application.ID == 2)
                    {
                        int iTotal = 0;
                        UserDAL obj = new UserDAL();
                        
                        if (SecureLicense == "0" || obj.IsValidLicense(out iTotal, Secure.LicInformation.ServerLicense.MitigateServer) != Secure.LicInformation.ServerStatus.Expire)                        
                        {
                            string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                            if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                            {
                                DBId = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.AccountId).FirstOrDefault();
                            }

                            Log4Net.WriteLog("Logging To Mitigate", LogType.GENERALLOG);
                            Log4Net.WriteLog("Query String", LogType.GENERALLOG);
                            string queryParameter = Cryptography.Encrypt(userEntity.ID + "$$$" + userEntity.Username + "$$$" + userEntity.Password + "$$$" + DBId + "$$$" + userEntity.SupportEmails);
                            queryParameter = queryParameter.Replace("+", "%2B");
                            queryParameter = queryParameter.Replace(",", "%2C%20");
                            queryParameter = queryParameter.Replace("/", "%2F");
                            application.ApplicationURL += "Account/Login";
                            Log4Net.WriteLog(application.ApplicationURL + "?AccountId=" + queryParameter, LogType.GENERALLOG);
                            Response.Redirect(application.ApplicationURL + "?AccountId=" + queryParameter, false);
                            Context.ApplicationInstance.CompleteRequest();

                        }
                        else
                        {
                            Response.Redirect("Applications.aspx", false);
                        }
                    }
                    else if (application.ID == 4)
                    {
                        Log4Net.WriteLog("Logging To DAC6", LogType.GENERALLOG);
                        string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                        if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                        {
                            DBId = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.AccountId).FirstOrDefault();
                            Log4Net.WriteLog("Account Id found with DB Id: " + DBId, LogType.GENERALLOG);
                        }
                        else
                        {
                            Log4Net.WriteLog("Account id not found.", LogType.GENERALLOG);
                        }
                        
                        string queryParameter = Cryptography.Encrypt(userEntity.ID + "$$$" + userEntity.Username + "$$$" + userEntity.Password + "$$$" + DBId + "$$$" + userEntity.SupportEmails + "$$$" + QueryFromURL);
                        queryParameter = queryParameter.Replace("+", "%2B");
                        queryParameter = queryParameter.Replace(",", "%2C%20");
                        queryParameter = queryParameter.Replace("/", "%2F");
                        Log4Net.WriteLog("Query String: " + queryParameter, LogType.GENERALLOG);
                        application.ApplicationURL += "Account/Login";


                        if (Convert.ToInt32(ConfigurationManager.AppSettings["DirectDAC6"]) == 1)
                        {
                            HttpContext.Current.Session["UserEntity"] = null;
                            HttpContext.Current.Session.Abandon();
                        }
                        Log4Net.WriteLog(application.ApplicationURL + "?AccountId=" + queryParameter, LogType.GENERALLOG);
                        Response.Redirect(application.ApplicationURL + "?AccountId=" + queryParameter, false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else if (application.ID == 5) 
                    {
                        string dbName_SinglePoint = ConfigurationManager.AppSettings["CloudDB"];
                        string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                        if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                        {
                            DBId = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.AccountId).FirstOrDefault();
                            Log4Net.WriteLog("Account Id found with DB Id: " + DBId, LogType.GENERALLOG);
                        }
                        else
                        {
                            Log4Net.WriteLog("Account id not found.", LogType.GENERALLOG);
                        }

                        if (DBId > 0)
                        {
                            dbName_SinglePoint += "_" + DBId.ToString();
                        }
                        string Email = HttpContext.Current.Session["Email"].ToString();
                        string Password = HttpContext.Current.Session["Password"].ToString();
                        Log4Net.WriteLog("Logging To Training Courses", LogType.GENERALLOG);
                        string IsAdmin = "false";
                        if (userEntity.TCLevelPermissionId == 1)
                        {
                            IsAdmin = "true";
                        }
                        string queryParameter = Cryptography.Encrypt(Email + "$$$" + Password + "$$$" + userEntity.CompanyId+ "$$$" + IsAdmin + "$$$" + dbName_SinglePoint + "$$$" + userEntity.TCUserId);
                        queryParameter = queryParameter.Replace("+", "%2B");
                        queryParameter = queryParameter.Replace(",", "%2C%20");
                        queryParameter = queryParameter.Replace("/", "%2F");
                        Log4Net.WriteLog("Query String: " + queryParameter, LogType.GENERALLOG);
                        application.ApplicationURL += "Auth/LoginForSinglePoint";

                        Log4Net.WriteLog(application.ApplicationURL + "?AccountId=" + queryParameter, LogType.GENERALLOG);
                        Response.Redirect(application.ApplicationURL + "?AccountId=" + queryParameter, false);        //write redirect
                        Context.ApplicationInstance.CompleteRequest(); // end response
                    }
                    else
                    {
                        string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                        if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                        {
                            DBId = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.AccountId).FirstOrDefault();
                        }
                        application.ApplicationURL += "Login.aspx";
                        string html = "<html><head>";
                        html += "</head><body onload='document.forms[0].submit()'>";
                        html += string.Format("<form name='PostForm' method='POST' action='{0}' style='visibility:hidden'>", application.ApplicationURL);
                        html += "<input id='AccountId' name='AccountId' type='text' value='" + Cryptography.Encrypt(userEntity.ID + "$$$" + userEntity.Username + "$$$" + userEntity.Password + "$$$" + DBId + "$$$" + userEntity.SupportEmails) + "'>";

                        html += "</form></body></html>";
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
                        HttpContext.Current.Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
                        HttpContext.Current.Response.Charset = "ISO-8859-1";
                        HttpContext.Current.Response.Write(html);
                        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                        Log4Net.WriteLog("Form HTML : " + html, LogType.GENERALLOG);
                        Log4Net.WriteLog("------ Successfully transfer " + application.Name + " Application  ------", LogType.GENERALLOG);
                    }

                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }

            }
            catch (Exception ex)
            {
                Log4Net.WriteLog("------ Exception while transfer To Application ------", LogType.ERRORLOG);
                Log4Net.WriteException(ex);
            }
        }
    }
}