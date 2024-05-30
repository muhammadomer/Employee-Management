using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Database.DAL
{
    public class SettingsDAL : BaseDAL
    {

        public static string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$£%^&*_\-+=`|\\/'(){}[\]:;""'<>,.?])[A-Za-z\d~!@#$£%^&*_\-+=`|\\/'(){}[\]:;\""<>,.?]{8,}$";
        #region Add Region, County, City, Department Functions

        public string AddOrUpdateRegion(Regions region)
        {
            try
            {
                string feedback = "";
                if (IfRegionAlreadyExist(region.Name, region.ID))
                {
                    feedback = "This Region already exists.";
                }
                else
                {
                    if (region.ID != 0)
                    {
                        region.Visible = true;
                        employeesEntities.Regions.AddOrUpdate(region);
                        employeesEntities.SaveChanges();
                        feedback = "Region updated successfully.";
                    }
                    else
                    {
                        region.Visible = true;
                        employeesEntities.Regions.AddOrUpdate(region);
                        employeesEntities.SaveChanges();
                        feedback = "Region inserted successfully.";
                    }
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public string AddOrUpdateCountry(Countries country)
        {
            try
            {
                string feedback = "";
                if (IfCountryAlreadyExist(country.Name, country.ID))
                {
                    feedback = "This Country already exists.";
                }
                else
                {
                    if (country.ID != 0)
                    {
                        country.Visible = true;
                        employeesEntities.Countries.AddOrUpdate(country);
                        employeesEntities.SaveChanges();

                        Country _country = new Country();
                        _country.Code = "11";
                        _country.Name = country.Name;
                        _country.NumCode = 0;
                        _country.PhoneCode = 0;
                        _country.CurrencyCode = "";
                        _country.IsExempted = false;
                        _country.EUMember = false;
                        employeesEntities.Country.AddOrUpdate(_country);
                        employeesEntities.SaveChanges();




                        feedback = "Country updated successfully.";
                    }
                    else
                    {
                        country.Visible = true;
                        employeesEntities.Countries.AddOrUpdate(country);
                        employeesEntities.SaveChanges();
                        Country _country = new Country();
                        _country.Code = "11";
                        _country.Name = country.Name;
                        _country.NumCode = 0;
                        _country.PhoneCode = 0;
                        _country.CurrencyCode = "";
                        _country.IsExempted = false;
                        _country.EUMember = false;
                        employeesEntities.Country.AddOrUpdate(_country);
                        employeesEntities.SaveChanges();

                        feedback = "Country inserted successfully.";
                    }
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public string AddOrUpdateCity(Cities city)
        {
            try
            {
                string feedback = "";
                if (IfCityAlreadyExist(city.Name, city.ID))
                {
                    feedback = "This City already exists.";
                }
                else
                {
                    if (city.ID != 0)
                    {
                        city.Visible = true;
                        employeesEntities.Cities.AddOrUpdate(city);
                        employeesEntities.SaveChanges();
                        feedback = "City updated successfully.";
                    }
                    else
                    {
                        city.Visible = true;
                        employeesEntities.Cities.AddOrUpdate(city);
                        employeesEntities.SaveChanges();
                        feedback = "City inserted successfully.";
                    }
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public string AddOrUpdateDepartment(Departments department)
        {
            try
            {
                string feedback = "";
                if (IfDepartmentAlreadyExist(department.Name, department.ID))
                {
                    feedback = "This Department name already exists.";
                }
                else
                {
                    if (department.ID != 0)
                    {
                        Departments departmentOld = new Departments();
                        departmentOld = employeesEntities.Departments.Where(d => d.ID == department.ID).FirstOrDefault();

                        List<Users> users = employeesEntities.Users.Where(u => u.Department.Equals(departmentOld.Name)).ToList();

                        foreach (var item in users)
                        {
                            item.Department = department.Name;
                            employeesEntities.Entry(item).State = EntityState.Modified;
                        }

                        department.Visible = true;
                        employeesEntities.Departments.AddOrUpdate(department);

                        employeesEntities.SaveChanges();

                        feedback = "Department name updated successfully.";
                    }
                    else
                    {
                        department.Visible = true;
                        employeesEntities.Departments.AddOrUpdate(department);
                        employeesEntities.SaveChanges();
                        feedback = "Department name inserted successfully.";
                    }
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }


        public string AddOrUpdatePracticeGroup(PracticeGroups practicegroups)
        {
            try
            {
                string feedback = "";
                if (IfPracticeGroupAlreadyExist(practicegroups.Name, practicegroups.ID))
                {
                    feedback = "This Practice Group name already exists.";
                }
                else
                {
                    if (practicegroups.ID != 0)
                    {
                        PracticeGroups practicegroupOld = new PracticeGroups();
                        practicegroupOld = employeesEntities.PracticeGroups.Where(d => d.ID == practicegroups.ID).FirstOrDefault();


                        List<Users> users = employeesEntities.Users.Where(u => u.PracticeGroup.Equals(practicegroupOld.Name)).ToList();

                        foreach (var item in users)
                        {
                            item.PracticeGroup = practicegroups.Name;
                            employeesEntities.Entry(item).State = EntityState.Modified;
                        }

                        practicegroups.Visible = true;
                        employeesEntities.PracticeGroups.AddOrUpdate(practicegroups);

                        employeesEntities.SaveChanges();

                        feedback = "Practice Group name updated successfully.";
                    }
                    else
                    {
                        practicegroups.Visible = true;
                        employeesEntities.PracticeGroups.AddOrUpdate(practicegroups);
                        employeesEntities.SaveChanges();
                        feedback = "Practice Group name inserted successfully.";
                    }
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        #endregion

        #region GetList of Region, County, City, Department Functions

        public List<SettingEntity> GetAllRegions()
        {
            try
            {
                List<SettingEntity> regions = new List<SettingEntity>();
                var regionsList = employeesEntities.Regions.Where(x => x.Visible == true).OrderBy(x => x.Name).ToList();
                foreach (var item in regionsList)
                {
                    SettingEntity settingEntity = new SettingEntity();
                    settingEntity.ID = item.ID;
                    settingEntity.RegionName = item.Name;
                    regions.Add(settingEntity);
                }
                return regions;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<SettingEntity> GetAllCountriesList()
        {
            try
            {
                List<SettingEntity> regions = new List<SettingEntity>();
                var countryList = employeesEntities.Country.OrderBy(x => x.Name).ToList();
                foreach (var item in countryList)
                {
                    SettingEntity settingEntity = new SettingEntity();
                    settingEntity.ID = item.Id;
                    settingEntity.CountryName = item.Name;
                    settingEntity.Code = item.Code;
                    regions.Add(settingEntity);
                }
                return regions;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        public List<SettingEntity> GetAllCountries()
        {
            try
            {
                var countries = employeesEntities.Database.SqlQuery<SettingEntity>("SELECT co.ID, co.[Name] AS CountryName, r.ID AS RegionID, r.[Name] AS RegionName " +
                    "FROM Countries co INNER JOIN Regions r ON r.ID = co.RegionID WHERE co.Visible = 1").ToList();
                //var countriesList = employeesEntities.Countries.Where(x => x.Visible == true).OrderBy(x => x.Name).ToList();
                //foreach (var item in countriesList)
                //{
                //    SettingEntity settingEntity = new SettingEntity();
                //    settingEntity.ID = item.ID;
                //    settingEntity.CountryName = item.Name;
                //    settingEntity.RegionID = item.RegionID;
                //    settingEntity.RegionName = item.Regions.Name;
                //    countries.Add(settingEntity);
                //}
                return countries;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<SettingEntity> GetCountriesOfRegion(int regionID)
        {
            try
            {
                List<SettingEntity> countries = new List<SettingEntity>();
                var countriesList = employeesEntities.Countries.Where(x => x.RegionID == regionID && x.Visible == true).OrderBy(x => x.Name).ToList();
                foreach (var item in countriesList)
                {
                    SettingEntity settingEntity = new SettingEntity();
                    settingEntity.ID = item.ID;
                    settingEntity.CountryName = item.Name;
                    countries.Add(settingEntity);
                }
                return countries;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<SettingEntity> GetCitiesOfCountry(int countryID)
        {
            try
            {
                List<SettingEntity> cities = new List<SettingEntity>();
                var citiesList = employeesEntities.Cities.Where(x => x.CountryID == countryID && x.Visible == true).OrderBy(x => x.Name).ToList();
                foreach (var item in citiesList)
                {
                    SettingEntity settingEntity = new SettingEntity();
                    settingEntity.ID = item.ID;
                    settingEntity.CityName = item.Name;
                    cities.Add(settingEntity);
                }
                return cities;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<SettingEntity> GetAllCity()
        {
            try
            {
                //var citiesList = dentonsEmployeesEntities.Cities.Where(x => x.Visible == true).OrderBy(x => x.Name).ToList();
                var cities = employeesEntities.Database.SqlQuery<SettingEntity>("SELECT ci.ID, ci.[Name] AS CityName, co.RegionID, ci.CountryID, " +
                    "r.[Name] AS RegionName, co.[Name] AS CountryName FROM Cities ci INNER JOIN Countries co ON co.ID = ci.CountryID " +
                    "INNER JOIN Regions r On r.ID = co.RegionID WHERE ci.Visible = 1").ToList();
                //foreach (var item in citiesList)
                //{
                //    SettingEntity settingEntity = new SettingEntity();
                //    settingEntity.ID = item.ID;
                //    settingEntity.CityName = item.Name;
                //    settingEntity.RegionID = item.Countries.Regions.ID;
                //    settingEntity.CountryID = item.Countries.ID;
                //    settingEntity.RegionName = item.Countries.Regions.Name;
                //    settingEntity.CountryName = item.Countries.Name;
                //    cities.Add(settingEntity);
                //}
                return cities;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<Departments> GetAllDepartment()
        {
            try
            {
                return employeesEntities.Departments.Where(x => x.Visible == true).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PracticeGroups> GetAllPracticeGroups()
        {
            try
            {
                return employeesEntities.PracticeGroups.Where(x => x.Visible == true).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        #endregion

        #region Delete Region, County, City, Department , Practice Group , Mitigation Team Functions By ID 

        public bool DeleteRegionByID(int regionID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection con = new SqlConnection(employeesEntities.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DeleteConfiguration", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "DeleteRegion";
                            cmd.Parameters.Add("@RiskManagerDB", SqlDbType.VarChar).Value = mitigateEntities.Database.Connection.Database;
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = regionID.ToString();

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool DeleteCountryByID(int regionID, int countryID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection con = new SqlConnection(employeesEntities.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DeleteConfiguration", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "DeleteCountry";
                            cmd.Parameters.Add("@RiskManagerDB", SqlDbType.VarChar).Value = mitigateEntities.Database.Connection.Database;
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = countryID.ToString();

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool DeleteCityByID(int countryID, int cityID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection con = new SqlConnection(employeesEntities.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DeleteConfiguration", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "DeleteCity";
                            cmd.Parameters.Add("@RiskManagerDB", SqlDbType.VarChar).Value = mitigateEntities.Database.Connection.Database;
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = cityID.ToString();

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool DeletePracticeGroupByID(int practicegroupID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection con = new SqlConnection(employeesEntities.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DeleteConfiguration", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "DeletePracticeGroup";
                            cmd.Parameters.Add("@RiskManagerDB", SqlDbType.VarChar).Value = mitigateEntities.Database.Connection.Database;
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = practicegroupID.ToString();

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool DeleteDepartmentByID(int departmentID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection con = new SqlConnection(employeesEntities.Database.Connection.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DeleteConfiguration", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "DeleteDepartment";
                            cmd.Parameters.Add("@RiskManagerDB", SqlDbType.VarChar).Value = mitigateEntities.Database.Connection.Database;
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = departmentID.ToString();

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        #endregion

        // If Region name already exists
        public bool IfRegionAlreadyExist(string regionName, int regionID)
        {
            try
            {
                int numberOfRows = 0;
                if (regionID != 0)
                {
                    numberOfRows = employeesEntities.Regions.Where(x => x.Name == regionName && x.ID != regionID).Count();
                }
                else
                {
                    numberOfRows = employeesEntities.Regions.Where(x => x.Name == regionName).Count();
                }
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

        // If Country name already exists
        public bool IfCountryAlreadyExist(string countryName, int countryID)
        {
            try
            {
                int numberOfRows = 0;
                if (countryID != 0)
                {
                    numberOfRows = employeesEntities.Countries.Where(x => x.Name == countryName && x.ID != countryID).Count();
                }
                else
                {
                    numberOfRows = employeesEntities.Countries.Where(x => x.Name == countryName).Count();
                }
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

        // If City name already exists
        public bool IfCityAlreadyExist(string cityName, int cityID)
        {
            try
            {
                int numberOfRows = 0;
                if (cityID != 0)
                {
                    numberOfRows = employeesEntities.Cities.Where(x => x.Name == cityName && x.ID != cityID).Count();
                }
                else
                {
                    numberOfRows = employeesEntities.Cities.Where(x => x.Name == cityName).Count();
                }
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

        // If Department name already exists
        public bool IfDepartmentAlreadyExist(string departmentName, int departmentID)
        {
            try
            {
                int numberOfRows = 0;
                if (departmentID != 0)
                {
                    numberOfRows = employeesEntities.Departments.Where(x => x.Name == departmentName && x.ID != departmentID).Count();
                }
                else
                {
                    numberOfRows = employeesEntities.Departments.Where(x => x.Name == departmentName).Count();
                }
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


        // If Practice Group name already exists
        public bool IfPracticeGroupAlreadyExist(string practicegroupName, int practicegroupID)
        {
            try
            {
                int numberOfRows = 0;
                if (practicegroupID != 0)
                {
                    numberOfRows = employeesEntities.PracticeGroups.Where(x => x.Name == practicegroupName && x.ID != practicegroupID).Count();
                }
                else
                {
                    numberOfRows = employeesEntities.PracticeGroups.Where(x => x.Name == practicegroupName).Count();
                }
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
        public bool SaveSMTPSettings(DentonsEmployeesSettings smtpSettings)
        {
            try
            {
                DentonsEmployeesSettings dentonsEmployeesSettings = employeesEntities.DentonsEmployeesSettings.SingleOrDefault();

                dentonsEmployeesSettings.MailServer = smtpSettings.MailServer;
                dentonsEmployeesSettings.SMTPPort = smtpSettings.SMTPPort;
                dentonsEmployeesSettings.EnableSSL = smtpSettings.EnableSSL;
                dentonsEmployeesSettings.MailUsername = smtpSettings.MailUsername;
                dentonsEmployeesSettings.MailPassword = smtpSettings.MailPassword;

                employeesEntities.DentonsEmployeesSettings.AddOrUpdate(dentonsEmployeesSettings);
                employeesEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public DentonsEmployeesSettings GetGeneralSettings()
        {
            try
            {
                return employeesEntities.DentonsEmployeesSettings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public bool EnableDisableTwoFA(bool state)
        {
            try
            {
                DentonsEmployeesSettings settings = employeesEntities.DentonsEmployeesSettings.FirstOrDefault();
                settings.TwoFAEnable = state;
                employeesEntities.Entry(settings).State = EntityState.Modified;
                employeesEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool IsEnabledTwoFA()
        {
            try
            {
                return employeesEntities.DentonsEmployeesSettings.Select(x => x.TwoFAEnable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public List<string> GetImagePathAndAddress()
        {
            try
            {
                var currentUser = GeneralUtilities.GetCurrentUser();
                List<string> imagePathAndAddress = new List<string>();
                string imagePath = HttpContext.Current.Server.MapPath("~/upload/Images/Logo");
                string dbId = Convert.ToString(HttpContext.Current.Session["DBId"]);
                dbId = Convert.ToString(HttpContext.Current.Session["UserAccountID"]);
                if (!String.IsNullOrWhiteSpace(dbId))
                {
                    imagePath = HttpContext.Current.Server.MapPath("~/upload/Images/" + dbId + "/" + currentUser.ID);
                }
                else
                {
                    imagePath = HttpContext.Current.Server.MapPath("~/upload/Images/0/" + currentUser.ID);
                }
                imagePath += "/logo.png";
                //imagePath = HttpContext.Current.Server.MapPath(imagePath);
                if (!File.Exists(imagePath))
                {
                    imagePath = "upload/Images/logo.png";
                }
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        imagePath = Convert.ToBase64String(imageBytes);
                    }
                }
                imagePathAndAddress.Add(imagePath);
                DentonsEmployeesSettings settings = employeesEntities.DentonsEmployeesSettings.FirstOrDefault();
                imagePathAndAddress.Add(settings.CompanyName);
                imagePathAndAddress.Add(settings.CompanyAddress);
                imagePathAndAddress.Add(settings.Currency);
                return imagePathAndAddress;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public bool SaveCompanyDetail(DentonsEmployeesSettings companyDetail)
        {
            try
            {
                DentonsEmployeesSettings settings = employeesEntities.DentonsEmployeesSettings.FirstOrDefault();
                settings.CompanyName = companyDetail.CompanyName;
                settings.CompanyAddress = companyDetail.CompanyAddress;
                settings.Currency = companyDetail.Currency;
                employeesEntities.DentonsEmployeesSettings.AddOrUpdate(settings);
                employeesEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }


    }
}
