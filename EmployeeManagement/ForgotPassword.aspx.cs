using Database.DAL;
using LogApp;
using System;
using System.Configuration;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services;

namespace EmployeeManagement
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);
            if (!Page.IsPostBack)
            {
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    Response.Redirect("ResetPassword.aspx", false);
                }
            }
        }

      

        [WebMethod]
        [ScriptMethod]
        public static string SendResetPasswordLink(string userName, string accountID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    HttpContext.Current.Session["UserAccountID"] = accountID;
                }
                else
                {
                    accountID = "";
                    HttpContext.Current.Session["UserAccountID"] = "";
                }

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