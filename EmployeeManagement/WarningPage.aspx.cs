using System;

namespace EmployeeManagement
{
    public partial class WarningPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEntity"] != null)
            {

            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
    }
}