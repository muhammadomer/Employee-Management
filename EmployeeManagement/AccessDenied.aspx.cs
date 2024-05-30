using System;
using System.Web.Helpers;

namespace EmployeeManagement
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            AFDiv.InnerHtml = AntiForgery.GetHtml().ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);
        }
    }
}