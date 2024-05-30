using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;

namespace EmployeeManagement
{
    public partial class SystemSettings : System.Web.UI.Page
    {
        
        public SystemSettings()
        {
           
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                if (Session["UserEntity"] != null)
                {
                    Users userEntity = new Users();
                    userEntity = GeneralUtilities.GetCurrentUser();
                    if (userEntity.IsActive && userEntity.UserTypeID != 1)
                    {
                        Response.Redirect("AccessDenied.aspx", false);
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<SettingEntity> GetAllCountriesList()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                return new SettingsDAL().GetAllCountriesList();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        

        [WebMethod]
        [ScriptMethod]   
        public static List<SettingEntity> GetAllRegions()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);                
                return new SettingsDAL().GetAllRegions();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;        
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<SettingEntity> GetAllCountries()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().GetAllCountries();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<SettingEntity> GetCountriesOfRegion(int regionID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().GetCountriesOfRegion(regionID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<SettingEntity> GetCitiesOfCountry(int countryID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().GetCitiesOfCountry(countryID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<SettingEntity> GetAllCities()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().GetAllCity();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<Departments> GetAllDepartments()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().GetAllDepartment();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<PracticeGroups> GetAllPracticeGroups()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);


                return new SettingsDAL().GetAllPracticeGroups();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static DentonsEmployeesSettings GetGeneralSettings()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().GetGeneralSettings();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string AddOrUpdateRegion(Regions region)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().AddOrUpdateRegion(region);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string AddOrUpdateCountry(Countries country)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().AddOrUpdateCountry(country);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string AddOrUpdateCity(Cities city)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().AddOrUpdateCity(city);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string AddOrUpdateDepartment(Departments department)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);                
                return new SettingsDAL().AddOrUpdateDepartment(department);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string AddOrUpdatePracticeGroup(PracticeGroups practicegroup)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);


                return new SettingsDAL().AddOrUpdatePracticeGroup(practicegroup);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        
        [WebMethod]
        [ScriptMethod]
        public static bool SaveSMTPSettings(DentonsEmployeesSettings smtpSettings)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().SaveSMTPSettings(smtpSettings);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DeleteRegionByID(int regionID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().DeleteRegionByID(regionID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DeleteCountryByID(int regionID, int countryID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().DeleteCountryByID(regionID, countryID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DeleteCityByID(int countryID, int cityID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().DeleteCityByID(countryID, cityID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DeleteDepartmentByID(int departmentID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().DeleteDepartmentByID(departmentID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DeletePracticeGroupByID(int practicegroupID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);


                return new SettingsDAL().DeletePracticeGroupByID(practicegroupID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        
        [WebMethod]
        [ScriptMethod]
        public static string TestSMTPSettings(string toEmail, SmtpSettingsEntity smtpSettings)
        {
            try
            {
                string EmailTextChange = "Dentons Employees";
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    EmailTextChange = "MITIGATE PRO Client Access Portal";
                }


                    CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(toEmail);
                    mail.From = new MailAddress(smtpSettings.FromAddress);
                    mail.Subject = EmailTextChange + " - SMTP Test Message";

                    string Body = "This is an email message sent by " + EmailTextChange + "  application while testing the SMTP settings.";
                    mail.Body = Body;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpSettings.Host;
                    smtp.Port = smtpSettings.Port;
                    smtp.Credentials = new System.Net.NetworkCredential
                         (smtpSettings.FromAddress, smtpSettings.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    return "Sent email successfuly";
                }
                catch(Exception ex) {

                    if (ex.InnerException != null)
                    {
                        return ex.InnerException.Message;
                    }
                    else
                    {
                        return ex.Message;

                    }

                }

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;

            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool EnableDisableTwoFA(bool state)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                
                return new SettingsDAL().EnableDisableTwoFA(state);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        [WebMethod]
        [ScriptMethod]
        public static string ChangeEncryptedPassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (!string.IsNullOrWhiteSpace(newPassword) && !string.IsNullOrWhiteSpace(confirmNewPassword))
                {
                    if (newPassword.Equals(confirmNewPassword))
                    {
                        var regex = SettingsDAL.regex;
                        var match = Regex.Match(newPassword, regex);

                        if (match.Success)
                        {
                            UserDAL userDAL = new UserDAL();
                            return userDAL.ChangeEncryptedPassword(oldPassword, newPassword);
                        }
                        else
                        {
                            string response = "3"; // Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.
                            return response;
                        }
                    }
                    else
                    {
                        string response = "4"; // The passwords you supplied do not match.
                        return response;
                    }
                }
                else
                {
                    string response = "5"; // Please provide all manadatory fields.
                    return response;
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
        [WebMethod]
        [ScriptMethod]
        public static List<string> GetReportLogoPath()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                return new SettingsDAL().GetImagePathAndAddress();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        [WebMethod]
        [ScriptMethod]
        public static bool SaveCompanyDetail(DentonsEmployeesSettings companyDetail)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);
                return new SettingsDAL().SaveCompanyDetail(companyDetail);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }
    }
}