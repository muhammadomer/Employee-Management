using LogApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace EmployeeManagement
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                Log4Net.FilePath = Server.MapPath("./Logs");
                Log4Net.FileName = "Log";
                Log4Net.EnableDBLOG = true;
                Log4Net.EnableERRORLOG = true;
                Log4Net.EnableGENERALLOG = true;
                Log4Net.FileSize = 10;
                Log4Net.TotalFiles = 10;
                Log4Net.Activate(true);
                Log4Net.WriteLog("Application Log initiazlize successfully", LogType.GENERALLOG);

            }
            catch (Exception ex)
            {
                Log4Net.WriteLog(ex.ToString(), LogType.GENERALLOG);
            }
        }
        protected void Sesstion_Start()
        {
            // sesstion Time 60000 minutes
            Session.Timeout = 60000;
            HttpContext.Current.Session["UserAccountID"] = "";
        }
        
    }
}