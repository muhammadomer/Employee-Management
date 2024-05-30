using Database.Entities;
using Database.Models.EmployeeManagement;
//using Database.Models.EmployeeManagement;
using Database.Models.SinglePointCloud;
using LogApp;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Database.DAL
{
    public class GeneralUtilities
    {
        private static SqlConnection sqlConnection;
        private static SqlDataAdapter sqlDataAdapter;
        private static SqlCommand sqlCommand;
        
        public static string GetConnectionString(string dbName)
        {
            try
            {
                dbName = GetDatabaseName(dbName);
                var originalConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                string[] orignalConnectionStringSiplited = originalConnectionString.Split(';');
                string server = orignalConnectionStringSiplited[0].Split('=')[1];
                string sqlUserName = orignalConnectionStringSiplited[2].Split('=')[1];
                string sqlPassword = orignalConnectionStringSiplited[3].Split('=')[1];
                string dbClientDBName = string.Empty;
                string connectionString = "Initial Catalog=" + dbName + ";Data Source=" + server + ";user id=" + sqlUserName + ";password=" + sqlPassword + ";";
                Log4Net.WriteLog("-- Connection String Returned --" + connectionString, LogType.GENERALLOG);
                return connectionString;
            }
            catch (Exception ex)
            {
                Log4Net.WriteLog("-- Error in Getting Connection String --", LogType.ERRORLOG);
                Log4Net.WriteException(ex);
                return "";
            }
        }

        public static string GetDatabaseName(string dbName)
        {
            try
            {
                string dbName_SinglePoint = ConfigurationManager.AppSettings["CloudDB"];
                string dbName_RiskManager = ConfigurationManager.AppSettings["MitigateDB"];
                string dbName_DAC6 = ConfigurationManager.AppSettings["DAC6DB"];
                string dbName_FileRepository = ConfigurationManager.AppSettings["FileRepositoryDB"];
                string dbName_BusinessCard = ConfigurationManager.AppSettings["BusinessCardDB"];

                int DBId = 0;
                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    if (HttpContext.Current.Session["UserAccountID"] != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Session["UserAccountID"].ToString()))
                    {
                        SinglePoint_CloudEntities singlePoint_CloudEntities = new SinglePoint_CloudEntities();
                        string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                        DBId = singlePoint_CloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.AccountId).FirstOrDefault();
                    }
                }

                if (dbName == "DentonsEmployees")
                {
                    if (ConfigurationManager.AppSettings["onPrem"] == "0")
                    {
                        dbName = dbName_SinglePoint + "_" + DBId.ToString();
                    }
                    else
                    {
                        dbName = dbName_SinglePoint;
                    }
                }
                else if (dbName == "Mitigate")
                {
                    if (ConfigurationManager.AppSettings["onPrem"] == "0")
                    {
                        dbName = dbName_RiskManager + "_" + DBId.ToString();
                    }
                    else
                    {
                        dbName = dbName_RiskManager;
                    }
                }
                else if (dbName == "FileManagement")
                {
                    if (ConfigurationManager.AppSettings["onPrem"] == "0")
                    {
                        dbName = dbName_FileRepository + "_" + DBId.ToString();
                    }
                    else
                    {
                        dbName = dbName_FileRepository;
                    }
                }
                else if (dbName == "BusinessCard")
                {
                    if (ConfigurationManager.AppSettings["onPrem"] == "0")
                    {
                        dbName = dbName_BusinessCard + "_" + DBId.ToString();
                    }
                    else
                    {
                        dbName = dbName_BusinessCard;
                    }
                }
                else if (dbName == "DAC6")
                {
                    if (ConfigurationManager.AppSettings["onPrem"] == "0")
                    {
                        dbName = dbName_DAC6 + "_" + DBId.ToString();
                    }
                    else
                    {
                        dbName = dbName_DAC6;
                    }
                }
                Log4Net.WriteLog("-- Database name --" + dbName, LogType.GENERALLOG);
                return dbName;
            }
            catch (Exception ex)
            {
                Log4Net.WriteLog("-- Error in Getting  Database name --", LogType.ERRORLOG);
                Log4Net.WriteException(ex);
                return "";
            }
        }

        public static Users GetCurrentUser()
        {
            try
            {
                Users userEntity = (Users)HttpContext.Current.Session["UserEntity"];
                return userEntity;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        
        public static ApplicationEntity GetApplicationUrlByID(int appID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "select app.[ID],app.[Application Name],app.[App Url] from Applications app where app.ID = @appID and IsDeleted = @isDeleted";
                ApplicationEntity application = new ApplicationEntity();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@appID", appID);
                sqlCommand.Parameters.AddWithValue("@isDeleted", 0);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    application.ID = Convert.ToInt32(row["ID"]);
                    application.Name = row["Application Name"].ToString();
                    application.ApplicationURL = row["App Url"].ToString();
                }
                return application;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        
    }
}
