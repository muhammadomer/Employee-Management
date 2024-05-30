using Database.Models.BusinessCard;
using Database.Models.DAC6;
using Database.Models.EmployeeManagement;
using Database.Models.FileManagement;
using Database.Models.Mitigate;
using Database.Models.SinglePointCloud;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Database.DAL
{
    public class BaseDAL
    {
        protected MitigateEntities mitigateEntities;
        protected DentonsEmployeesEntities employeesEntities;
        protected FileManagementEntities fileManagementEntities;
        protected BusinessCardEntities businessCardEntities;
        protected DAC6Entities dac6Entities;
        protected SinglePoint_CloudEntities singlePointCloudEntities;
        protected int DBId = 0;
        public BaseDAL()
        {
            //string vRecordDB = System.Configuration.ConfigurationManager.AppSettings["vRecordDB"];
            employeesEntities = new DentonsEmployeesEntities();
            mitigateEntities = new MitigateEntities();
            fileManagementEntities = new FileManagementEntities();
            businessCardEntities = new BusinessCardEntities();
            dac6Entities = new DAC6Entities();
            singlePointCloudEntities = new SinglePoint_CloudEntities();


            string dbName_SinglePoint = ConfigurationManager.AppSettings["CloudDB"];
            string dbName_RiskManager = ConfigurationManager.AppSettings["MitigateDB"];
            string dbName_DAC6 = ConfigurationManager.AppSettings["DAC6DB"];
            string dbName_FileRepository = ConfigurationManager.AppSettings["FileRepositoryDB"];
            string dbName_BusinessCard = ConfigurationManager.AppSettings["BusinessCardDB"];
            if (ConfigurationManager.AppSettings["onPrem"] == "0")
            {
                try
                {
                    string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                    LogApp.Log4Net.WriteLog("Account ID: " + accountID, LogApp.LogType.GENERALLOG);
                    if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                    {
                        DBId = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.AccountId).FirstOrDefault();
                      //  DBId = 1;
                    }

                    if (DBId > 0)
                    {
                        HttpContext.Current.Session["DBId"] = DBId;
                        dbName_SinglePoint += "_" + DBId.ToString();
                        dbName_RiskManager += "_" + DBId.ToString();
                        dbName_DAC6 += "_" + DBId.ToString();
                        dbName_FileRepository += "_" + DBId.ToString();
                        dbName_BusinessCard += "_" + DBId.ToString();
                    }
                    else
                    {
                        dbName_SinglePoint += "_ERROR";
                        dbName_RiskManager += "_ERROR";
                        dbName_DAC6 += "_ERROR";
                        dbName_FileRepository += "_ERROR";
                        dbName_BusinessCard += "_ERROR";
                    }

                }
                catch (Exception ex) {
                    LogApp.Log4Net.WriteLog("Account ID session not found", LogApp.LogType.GENERALLOG); 
                    LogApp.Log4Net.WriteException(ex);

                    dbName_SinglePoint += "_ERROR";
                    dbName_RiskManager += "_ERROR";
                    dbName_DAC6 += "_ERROR";
                    dbName_FileRepository += "_ERROR";
                    dbName_BusinessCard += "_ERROR";
                }
            }
            employeesEntities.Database.Connection.ConnectionString = employeesEntities.Database.Connection.ConnectionString.Replace("DentonsEmployees", dbName_SinglePoint);
            mitigateEntities.Database.Connection.ConnectionString = mitigateEntities.Database.Connection.ConnectionString.Replace("Mitigate", dbName_RiskManager);
            fileManagementEntities.Database.Connection.ConnectionString = fileManagementEntities.Database.Connection.ConnectionString.Replace("FileManagement", dbName_FileRepository);
            businessCardEntities.Database.Connection.ConnectionString = businessCardEntities.Database.Connection.ConnectionString.Replace("BusinessCard", dbName_BusinessCard);
            dac6Entities.Database.Connection.ConnectionString = dac6Entities.Database.Connection.ConnectionString.Replace("DAC6", dbName_DAC6);

            LogApp.Log4Net.WriteLog("employeesEntities CS: " + employeesEntities.Database.Connection.ConnectionString, LogApp.LogType.GENERALLOG);
        }
    }
}
