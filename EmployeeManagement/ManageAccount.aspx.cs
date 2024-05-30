using Database.DAL;
using LogApp;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services;

namespace EmployeeManagement
{
    public partial class ManageAccount : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DisableGoogleAuthenticator()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                Security security = new Security();
                return security.DisableGoogleAuthenticator();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<string> EnableGoogleAuthenticator()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                Security security = new Security();
                return security.EnableGoogleAuthenticator();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string EnableGoogleAuthenticatorWithVerificationCode(string Code, string SecretKey)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                Security security = new Security();
                return security.EnableGoogleAuthenticatorWithVerificationCode(Code,SecretKey);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        //[WebMethod]
        //[ScriptMethod]
        //public static void VerifyCode(VerifyCodeViewModel model,)
        //{
            
        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    var result = SignInManagerExtensions.TwoFactorSignIn( 
        //        model.Provider,
        //        model.Code,
        //        isPersistent: model.RememberMe,
        //        rememberBrowser: model.RememberBrowser);

        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //           // return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid code.");
        //            return View(model);
        //    }
        //}
    }
}