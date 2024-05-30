using System;
using System.Web.Helpers;

namespace EmployeeManagement
{
    public partial class AuthenticatUser : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);
        }


        public static void Button1_Click(object sender, EventArgs e)
        {
            //string Code = txbVerificationCode.Text;
            //Users user = GeneralUtilities.GetCurrentUser();
            //try
            //{
            //    byte[] secretKey = Base32Encoder.Decode(user.GoogleAuthenticatorSecretKey);
            //    long timeStepMatched = 0;
            //    string recoveryCodes = ";";
            //    var otp = new Totp(secretKey);
            //    Users userEntity = GeneralUtilities.GetCurrentUser();

            //    if (otp.VerifyTotp(Code.Trim(), out timeStepMatched, new VerificationWindow(2, 2)))
            //    {
            //        Log4Net.WriteLog("Verified OTP Code", LogType.GENERALLOG);
            //        Response.Redirect("Employees.aspx", false);

            //    }
            //    else
            //    {
            //        Log4Net.WriteLog("Not Verify OTP Code", LogType.GENERALLOG);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log4Net.WriteException(ex);
            //}
        }
    }
}