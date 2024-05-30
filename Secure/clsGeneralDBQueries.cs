
using LogApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secure
{
    public class clsGeneralDBQeueries
    {
        public static bool isDBAvailable(string ConnectionString)
        {
            bool boolResult = false;
            try
            {
                //Log4Net.WriteLog("Database connection string [ " + ConnectionString + " ]", LogType.DBLOG);
                Log4Net.WriteLog("Checking DB connection with connection string = " + ConnectionString, LogType.DBLOG);
                SqlConnection connTemp = new SqlConnection(ConnectionString);
                connTemp.Open();

                if (connTemp.State == ConnectionState.Open)
                {
                    boolResult = true;
                    connTemp.Close();
                }
            }
            catch (Exception E)
            {
                Log4Net.WriteException(E);
            }
            Log4Net.WriteLog("Database avaiable status : [ " + boolResult.ToString() + " ].", LogType.GENERALLOG);
            return boolResult;
        }

        public static string GetXML(string Query, string connectionString)
        {
            try
            {
                 using (SqlCommand cmd = new SqlCommand(Query))                
                {
                     SqlDataAdapter adpt = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    int result = -5;
                     using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        adpt.SelectCommand = cmd;

                        cmd.Connection = conn;
                        result = adpt.Fill(ds);


                        if (conn.State == ConnectionState.Open)
                            conn.Close();


                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //object licensevalue = ds.Tables[0].Rows[0]["License"];
                            if (!DBNull.Value.Equals(ds.Tables[0].Rows[0]["License"]))
                            {
                                return ds.Tables[0].Rows[0]["License"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        

        public static int SaveXML(string connectionString,string xml)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    //  string query = "Update ServerInfo set License=@License";
                    string query = "Update DentonsEmployeesSettings set License=@License";
                    int val = 0;
                    // using (SqlCommand cmd = new SqlCommand(query))
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                       ///  cmd.Parameters.Add("@License", SqlDbType.NVarChar).Value = xml;
                        cmd.Parameters.Add("@License", SqlDbType.VarChar).Value = xml;
                        cmd.Connection = conn;
                        val = cmd.ExecuteNonQuery();
                    }
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    return val;
                }

            }
            catch (Exception E)
            {
                LogApp.Log4Net.WriteException(E);
            }

            return -1;
        }

        
    }
}
