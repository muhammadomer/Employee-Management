using Database.Entities;
using LogApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DAL
{
    public class LocationDAL : BaseDAL
    {
        SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        SqlCommand sqlCommand;

        // Get list of location
        public List<OfficeEntity> GetAllLocations()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "SELECT loc.[ID] , loc.[Name] , ISNULL(reg.[Name],'Unassigned') as [RegionName] , [Office] , [Address Line 1] , [Address Line 2] , ISNULL(cit.[Name],'Unassigned') as [CityName] , [State] , [GPS Postal] , [Mail Postal] , ISNULL(con.[Name],'Unassigned') as [CountryName] , [Telephone] FROM [dbo].[Location] loc left join Regions reg on loc.RegionID = reg.ID left join Countries con on loc.CountryID = con.ID left join Cities cit on loc.CityID = cit.ID";
                List<OfficeEntity> officesList = new List<OfficeEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    officesList.Add(new OfficeEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString(),
                        RegionName = row["RegionName"].ToString(),
                        Office = row["Office"].ToString(),
                        AddressLine1 = row["Address Line 1"].ToString(),
                        AddressLine2 = row["Address Line 2"].ToString(),
                        CityName = row["CityName"].ToString(),
                        State = row["State"].ToString(),
                        GPSPostal = row["GPS Postal"].ToString(),
                        MailPostal = row["Mail Postal"].ToString(),
                        CountryName = row["CountryName"].ToString(),
                        Telephone = row["Telephone"].ToString()
                    });
                }
                return officesList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<string> GetAllCountriesName(string colName)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "SELECT [" + colName + "] FROM [dbo].[CountryID]";
                List<string> countryNames = new List<string>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    countryNames.Add(row["Nice Name"].ToString());
                }
                return countryNames;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        // Add Location
        public string AddLocation(OfficeEntity location)
        {
            try
            {
                string feedback = "Office inserted successfully.";
                if (IfLocationAlreadyExist(location.Name, location.ID))
                {
                    feedback = "This Office already exists.";
                }
                else
                {
                    sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                    string query = "INSERT INTO [dbo].[Location] ( [Name],[RegionID],[Office],[Address Line 1],[Address Line 2],[Longitude],[Latitude],[CityID],[State],[GPS Postal],[Mail Postal],[CountryID],[Telephone] ) VALUES ( @name , @regionID , @office , @addressLine1 , @addressLine2, @longitude , @latitude , @cityID , @state , @GPSPostal , @mailPostal , @countryID , @telephone )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@name", location.Name);
                    sqlCommand.Parameters.AddWithValue("@regionID", location.RegionID);
                    sqlCommand.Parameters.AddWithValue("@office", location.Office);
                    sqlCommand.Parameters.AddWithValue("@addressLine1", location.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@addressLine2", location.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@longitude", location.Longitude);
                    sqlCommand.Parameters.AddWithValue("@latitude", location.Latitude);
                    sqlCommand.Parameters.AddWithValue("@cityID", location.CityID);
                    sqlCommand.Parameters.AddWithValue("@state", location.State);
                    sqlCommand.Parameters.AddWithValue("@GPSPostal", location.GPSPostal);
                    sqlCommand.Parameters.AddWithValue("@mailPostal", location.MailPostal);
                    sqlCommand.Parameters.AddWithValue("@countryID", location.CountryID);
                    sqlCommand.Parameters.AddWithValue("@telephone", location.Telephone);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        // Edit Location
        public string EditLocation(OfficeEntity office)
        {
            try
            {
                string feedback = null;
                if (IfLocationAlreadyExist(office.Name, office.ID))
                {
                    feedback = "This Office already exists.";
                }
                else
                {
                    sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                    string query = "UPDATE [dbo].[Location] SET [Name] = @name , [RegionID] = @regionID , [Office] = @office , [Address Line 1] = @addressLine1 , [Address Line 2] = @addressLine2, [Longitude] = @longitude , [Latitude] = @latitude , [CityID] = @cityID , [State] = @state , [GPS Postal] = @GPSPostal , [Mail Postal] = @mailPostal , [CountryID] = @countryID , [Telephone] = @telephone WHERE ID = @locationID";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@name", office.Name);
                    sqlCommand.Parameters.AddWithValue("@regionID", office.RegionID);
                    sqlCommand.Parameters.AddWithValue("@office", office.Office);
                    sqlCommand.Parameters.AddWithValue("@addressLine1", office.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@addressLine2", office.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@longitude", office.Longitude);
                    sqlCommand.Parameters.AddWithValue("@latitude", office.Latitude);
                    sqlCommand.Parameters.AddWithValue("@cityID", office.CityID);
                    sqlCommand.Parameters.AddWithValue("@state", office.State);
                    sqlCommand.Parameters.AddWithValue("@GPSPostal", office.GPSPostal);
                    sqlCommand.Parameters.AddWithValue("@mailPostal", office.MailPostal);
                    sqlCommand.Parameters.AddWithValue("@countryID", office.CountryID);
                    sqlCommand.Parameters.AddWithValue("@telephone", office.Telephone);
                    sqlCommand.Parameters.AddWithValue("@locationID", office.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    feedback = "Office updated successfully.";
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        // Get Location By ID
        public OfficeEntity GetLocationInfoByID(int locationID)
        {
            try
            {
                OfficeEntity office = new OfficeEntity();
                DataTable dataTable = new DataTable();
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "SELECT [ID] , [Name] , [RegionID] , [Office] , [Address Line 1] , [Address Line 2], [Longitude] , [Latitude] , [CityID] , [State] , [GPS Postal] , [Mail Postal] , [CountryID] , [Telephone] FROM [dbo].[Location] where ID =@locationID";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@locationID", locationID);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    office.ID = Convert.ToInt32(row["ID"]);
                    office.Name = row["Name"].ToString();
                    office.RegionID = Convert.ToInt32(row["RegionID"]);
                    office.Office = row["Office"].ToString();
                    office.AddressLine1 = row["Address Line 1"].ToString();
                    office.AddressLine2 = row["Address Line 2"].ToString();
                    office.Longitude = row["Longitude"].ToString();
                    office.Latitude = row["Latitude"].ToString();
                    office.CityID = Convert.ToInt32(row["CityID"]);
                    office.State = row["State"].ToString();
                    office.GPSPostal = row["GPS Postal"].ToString();
                    office.MailPostal = row["Mail Postal"].ToString();
                    office.CountryID =Convert.ToInt32(row["CountryID"]);
                    office.Telephone = row["Telephone"].ToString();
                }
                return office;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        // Delete Location By ID
        public bool DeleteOfficeByID(int officeID)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(employeesEntities.Database.Connection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteConfiguration", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "DeleteOffice";
                        cmd.Parameters.Add("@RiskManagerDB", SqlDbType.VarChar).Value = mitigateEntities.Database.Connection.Database;
                        cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = officeID.ToString();

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        // If location name already exists
        public bool IfLocationAlreadyExist(string locationName, int locationID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "select ID from Location where Name=@name";
                sqlCommand = new SqlCommand(query, sqlConnection);
                if (locationID != 0)
                {
                    query = "select ID from Location where Name=@name and ID != @locationID";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@locationID", locationID);
                }

                sqlCommand.Parameters.AddWithValue("@name", locationName);
                sqlConnection.Open();
                int numberOfRows = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                if (numberOfRows > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }


        // Get Location Id on Name
        public int GetLocationIDOnName(string locationName)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                DataTable dataTable = new DataTable();
                string query = "select ID from Location where Name=@name";
                sqlCommand = new SqlCommand(query, sqlConnection);
                
                sqlCommand.Parameters.AddWithValue("@name", locationName);

                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);

                int ID = -1;
                if(dataTable.Rows.Count > 0)
                {
                    ID = Convert.ToInt32(dataTable.Rows[0]["ID"]);

                }

                return ID;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        // Get Region ID on name
        public int GetRegionIDOnName(string regionName="")
        {
            try
            {
                int ID = -1;
                if(regionName != "")
                {
                    ID = employeesEntities.Regions.Where(a => a.Name.ToLower() == regionName.ToLower()).FirstOrDefault().ID;
                }
                return ID;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        // Get Country Id on name
        public int GetCountryIdOnName(string countryName = "")
        {
            try
            {
                int ID = -1;
                if (countryName != "")
                {
                    ID = employeesEntities.Countries.Where(a => a.Name.ToLower() == countryName.ToLower()).FirstOrDefault().ID;
                }
                return ID;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        // Get City ID on City Name
        public int GetCityIdOnName(string cityName="")
        {
            try
            {
                int ID = -1;
                if (cityName != "")
                {
                    ID = employeesEntities.Cities.Where(a => a.Name.ToLower() == cityName.ToLower()).FirstOrDefault().ID;
                }
                return ID;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        // Get Department ID on Department Name
        public int GetDepartmentIdOnName(string departmentName = "")
        {
            try
            {
                int ID = -1;
                if (departmentName != "")
                {
                    ID = employeesEntities.Departments.Where(a => a.Name.ToLower() == departmentName.ToLower()).FirstOrDefault().ID;
                }
                return ID;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }


        // Get Practice Group ID On Name
        public int GetPracticeGroupIDOnName(string practicegroupName = "")
        {
            try
            {
                int ID = -1;
                if (practicegroupName != "")
                {
                    ID = employeesEntities.PracticeGroups.Where(a => a.Name.ToLower() == practicegroupName.ToLower()).FirstOrDefault().ID;
                }
                return ID;

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }
    }
}
