using System.Web.Script.Services;
using System.Web.Services;

namespace EmployeeManagement
{
    /// <summary>
    /// Summary description for SessionsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [ScriptService]
    public class SessionsService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        //[WebMethod(EnableSession = true)]
        //public string LogOut()
        //{
        //    Session.Abandon();
        //    return "Login.aspx";
        //}
    }
}
