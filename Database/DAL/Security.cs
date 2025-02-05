﻿using Base32;
using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;
using OtpSharp;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Database.DAL
{
    public class Security:BaseDAL
    {
        SqlConnection sqlConnection;
        private SettingsDAL _settingsDAL;
        private string TC_APIUrl;
        JsonDeserializer deserial;
        public Security()
        {
            _settingsDAL = new SettingsDAL();
            TC_APIUrl = employeesEntities.Applications.Where(x => x.ID == 5).Select(x => x.App_Url).FirstOrDefault();
            deserial = new JsonDeserializer();
        }
        private Users GetEmailOnUsername(string userName)
        {
            try
            {
                var singleRecord = employeesEntities.Users.Where(x => x.Username == userName && x.IsDeleted == false)
                    .Select(x => new {
                        x.First_Name, x.Last_Name,x.Email
                    }).FirstOrDefault();

                Users user = new Users();
                user.First_Name = singleRecord.First_Name;
                user.Last_Name = singleRecord.Last_Name;
                user.Email = singleRecord.Email;
                return user;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
            finally
            {

            }
        }
        
        /*
        public string SendResetPasswordLinkForAdmin(string userName)
        {
            try
            {
                
                string feedBack = "Your userName does not exist";
                UserDAL userDAL = new UserDAL();
                string userType = userDAL.GetUserType(userName);
                if (userDAL.IfUsernameAlreadyExist(userName, 0) && !String.IsNullOrWhiteSpace(userType) && userType == "Super Admin")
                {
                    Users userEntity = GetEmailOnUsername(userName);
                    if (!String.IsNullOrWhiteSpace(userEntity.Email))
                    {
                        string uniqueGuid = Guid.NewGuid().ToString();
                        sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                        string updateUserQuery = "Update Users Set ResetHash=@resetHash,ResetRequestDate=@resetDate Where UserName=@userName";
                        SqlCommand updateUser = new SqlCommand(updateUserQuery, sqlConnection);
                        updateUser.Parameters.AddWithValue("@resetHash", uniqueGuid);
                        updateUser.Parameters.AddWithValue("@resetDate", DateTime.Now);
                        updateUser.Parameters.AddWithValue("@userName", userName);
                        sqlConnection.Open();
                        updateUser.ExecuteNonQuery();
                        sqlConnection.Close();
                        string footerFontStyle = "color:#aaaaaa;font-size:11px;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:16px;color:#aaaaaa";
                        string greetingsFontStyle = "color:#aaaaaa;font-size:16px;font-weight:bold;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:21px;color:#141823";
                        string bodyFontStyle = "color:#aaaaaa;font-size:16px;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:21px;color:#141823";
                        string linkFontStyle = "color:#aaaaaa;font-size:16px;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:21px;color:#3b5998";
                        string url = "http://" + HttpContext.Current.Request.Url.Authority;
                        //string imagePath = url + "/images/email-logo.jpg";
                        string encryptedUserID = Cryptography.Cryptography.Encrypt(userName);
                        //string encryptedUserID = Cryptography.Cryptography.encodeSTROnUrl(encodedUserID);
                        string reseturl = url + "/admin/ChangePassword.aspx?id=" + uniqueGuid + "&uid=" + encryptedUserID;
                        StringBuilder emailBody = new StringBuilder();
                        //emailBody.Append("<a href=" + url + "><img src=" + imagePath + "></a><hr/>");
                        emailBody.Append("<font style='" + greetingsFontStyle + "'>Hi " + userEntity.First_Name + "</font><br/><br/>");
                        emailBody.Append("<font style='" + bodyFontStyle + "'>You recently requested to reset your password.</font><br/><br/>");
                        emailBody.Append("<a href='" + reseturl + "'><font style='" + linkFontStyle + "'>Click here to reset your password</font></a><br/><br/>");
                        emailBody.Append("<font style='" + bodyFontStyle + "'>If you did not make this request and deem this to be malicious, please alert your system administrator immediately.</font><br/><br/>");
                        emailBody.Append("<font style='" + footerFontStyle + "'>This message was sent to " + userEntity.Email + " by the Employee Management software.</font><br/>");
                        DentonsEmployeesSettings smtpSettings = settingsDAL.GetGeneralSettings();
                        if (!string.IsNullOrWhiteSpace(smtpSettings.MailUsername) && !string.IsNullOrWhiteSpace(smtpSettings.MailServer) && !string.IsNullOrWhiteSpace(smtpSettings.MailPassword) && smtpSettings.SMTPPort != 0)
                        {
                            Log4Net.WriteLog("SMTP Settings found.", LogType.GENERALLOG);
                            if (SendEmail("Password Reset", emailBody.ToString(), userEntity.Email, smtpSettings))
                            {
                                Log4Net.WriteLog("Reset password link sent successfully using clinet settings to this Email:"+ userEntity.Email, LogType.GENERALLOG);
                                feedBack = "link sent";
                            }
                        }
                    }
                }
                return feedBack;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        */
        public string SendResetPasswordLink(string userName, string accountId)
        {
            try
            {
                string AppName = "Employee Management";
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    AppName = ConfigurationManager.AppSettings["OtherApplicationName"].ToString();
                }

                string feedBack = "Username not found.";
                if(accountId != "")
                {
                    feedBack = "Invalid Username or Account Id.";
                }
                UserDAL userDAL = new UserDAL();
                if (userDAL.IfUsernameAlreadyExist(userName, 0))
                {
                    Users userEntity = GetEmailOnUsername(userName);
                    if (!String.IsNullOrWhiteSpace(userEntity.Email) && !String.IsNullOrWhiteSpace(userDAL.GetUserType(userName)))
                    {
                        string uniqueGuid = Guid.NewGuid().ToString();
                        sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                        string updateUserQuery = "Update Users Set ResetHash=@resetHash,ResetRequestDate=@resetDate Where UserName=@userName";
                        SqlCommand updateUser = new SqlCommand(updateUserQuery, sqlConnection);
                        updateUser.Parameters.AddWithValue("@resetHash", uniqueGuid);
                        updateUser.Parameters.AddWithValue("@resetDate", DateTime.Now);
                        updateUser.Parameters.AddWithValue("@userName", userName);
                        sqlConnection.Open();
                        updateUser.ExecuteNonQuery();
                        sqlConnection.Close();
                        string footerFontStyle = "color:#aaaaaa;font-size:11px;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:16px;color:#aaaaaa";
                        string greetingsFontStyle = "color:#aaaaaa;font-size:16px;font-weight:bold;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:21px;color:#141823";
                        string bodyFontStyle = "color:#aaaaaa;font-size:16px;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:21px;color:#141823";
                        string linkFontStyle = "color:#aaaaaa;font-size:16px;font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;line-height:21px;color:#3b5998";
                        string url = "http://" + HttpContext.Current.Request.Url.Authority;
                        //string imagePath = url + "/images/email-logo.jpg";
                        string encryptedUserID = Cryptography.Cryptography.Encrypt(userName + "###" + accountId);
                        //string encryptedUserID = Cryptography.Cryptography.encodeSTROnUrl(encodedUserID);
                        string reseturl = url + "/ChangePassword.aspx?id=" + uniqueGuid + "&uid=" + encryptedUserID;
                        StringBuilder emailBody = new StringBuilder();
                        //emailBody.Append("<a href=" + url + "><img src=" + imagePath + "></a><hr/>");
                        emailBody.Append("<font style='" + greetingsFontStyle + "'>Hi " + userEntity.First_Name + "</font><br/><br/>");
                        emailBody.Append("<font style='" + bodyFontStyle + "'>You recently requested to reset your password.</font><br/><br/>");
                        emailBody.Append("<a href='" + reseturl + "'><font style='" + linkFontStyle + "'>Click here to reset your password</font></a><br/><br/>");
                        emailBody.Append("<font style='" + bodyFontStyle + "'>If you did not make this request and deem this to be malicious, please alert your system administrator immediately.</font><br/><br/>");
                        emailBody.Append("<font style='" + footerFontStyle + "'>This message was sent to " + userEntity.Email + " by the "+ AppName + " Software.</font><br/>");
                        DentonsEmployeesSettings smtpSettings = _settingsDAL.GetGeneralSettings();
                        if (!string.IsNullOrWhiteSpace(smtpSettings.MailUsername) && !string.IsNullOrWhiteSpace(smtpSettings.MailServer) && !string.IsNullOrWhiteSpace(smtpSettings.MailPassword) && smtpSettings.SMTPPort != 0)
                        {
                            Log4Net.WriteLog("SMTP Settings found.", LogType.GENERALLOG);
                            if (SendEmail("Password Reset", emailBody.ToString(), userEntity.Email, smtpSettings))
                            {
                                Log4Net.WriteLog("Reset password link sent successfully using client settings to this Email:" + userEntity.Email, LogType.GENERALLOG);
                                feedBack = "link sent";
                            }
                            else
                            {
                                
                                feedBack = "Failed to sent email, please verify SMTP settings";
                            }
                        }
                        else
                        {
                            
                            feedBack = "SMTP setting not configured.";
                        }
                    }
                    Log4Net.WriteLog("feedBack : " + feedBack, LogType.GENERALLOG);
                }
                return feedBack;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        
        private bool SendEmail(string subject, string emailBody, string userEmail, DentonsEmployeesSettings smtpSettings)
        {
            try
            {
                string AppName = "Employee Management";
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    AppName = ConfigurationManager.AppSettings["OtherApplicationName"].ToString();
                }

                //if(smtpSettings.MailServer == "" || smtpSettings.MailUsername == "") { }
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(smtpSettings.MailServer);

                mail.From = new MailAddress(smtpSettings.MailUsername, AppName);
                mail.To.Add(userEmail);
                mail.Subject = subject;
                mail.Body = emailBody;
                mail.IsBodyHtml = true;
                smtpServer.Port = smtpSettings.SMTPPort;
                smtpServer.Credentials = new System.Net.NetworkCredential(smtpSettings.MailUsername, smtpSettings.MailPassword);
                smtpServer.EnableSsl = smtpSettings.EnableSSL;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpServer.UseDefaultCredentials = false;
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }
        
        public ResetPasswordEntity GetUserIdForPasswordReset(string uid)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                SqlCommand cmd = new SqlCommand("Select ID, ResetRequestDate,LastPasswordChange,Username From Users where ResetHash= @resetHash", sqlConnection);
                cmd.Parameters.AddWithValue("@resetHash", uid);
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DateTime passwordResetRequestDateTime = DateTime.Now;
                DateTime? lastPasswordChange = DateTime.Now;
                ResetPasswordEntity resetDetails = new ResetPasswordEntity();

                while (reader.Read())
                {
                    resetDetails.UserId = int.Parse(reader.GetValue(0).ToString());
                    passwordResetRequestDateTime = Convert.ToDateTime(reader.GetValue(1).ToString());
                    try
                    {
                        lastPasswordChange = Convert.ToDateTime(reader.GetValue(2).ToString());
                    }
                    catch (Exception ex)
                    {
                        lastPasswordChange = null;
                    }

                    resetDetails.UserName = reader.GetValue(3).ToString();
                    Log4Net.WriteLog("Password Request Date Time: " + passwordResetRequestDateTime.ToString(), LogType.GENERALLOG);
                }
                TimeSpan linkExpiryTime = DateTime.Now - passwordResetRequestDateTime;
                if (lastPasswordChange.HasValue && lastPasswordChange > passwordResetRequestDateTime)
                {
                    Log4Net.WriteLog("Password already changed", LogType.GENERALLOG);
                    resetDetails.LinkExpired = true;
                    resetDetails.UserFeedback = "Password reset link has been already used for reset password.";
                }
                else if (linkExpiryTime.TotalHours > 24)
                {
                    resetDetails.LinkExpired = true;
                    resetDetails.UserFeedback = "Password reset link has been expired. Password reset can be use within 24 hours.";
                }

                sqlConnection.Close();
                return resetDetails;
            }
            catch (Exception ex)
            {
                Log4Net.WriteLog("Error occured while getting reset pass request detail.", LogType.ERRORLOG);
                Log4Net.WriteException(ex);
                return null;
            }
            finally
            {
                if (sqlConnection != null)
                    ((IDisposable)sqlConnection).Dispose();
            }
        }
        
        public bool ChangePassword(string password, int userId)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                byte[] passwordSalt = Cryptography.Cryptography.GenerateSalt();
                string passwordHash = Cryptography.Cryptography.ComputeHash(password, passwordSalt);
                string query = "Update Users Set Password=@password,LastPasswordChange=GETDATE() WHERE ID=@userId";
                SqlCommand updatePassword = new SqlCommand(query, sqlConnection);
                updatePassword.Parameters.AddWithValue("@password", passwordHash);
                updatePassword.Parameters.AddWithValue("@userId", userId);
                sqlConnection.Open();
                updatePassword.ExecuteNonQuery();
                sqlConnection.Close();
				// Update User in Training Courses
                /*Users userForTrainingCourses = new Users();
                userForTrainingCourses.UserEdit = new UserAddModel();
                userForTrainingCourses.UserEdit.Email = userDetail.Email + "$$$" + currentUser.CompanyId + "$$$" + userDetail.TCLevelPermissionId + "$$$" + userDetail.TCLevelPermissionId + "$$$" + userDetail.Email;
                userForTrainingCourses.UserEdit.FirstName = userDetail.First_Name;
                userForTrainingCourses.UserEdit.LastName = userDetail.Last_Name;
                userForTrainingCourses.UserEdit.Password = password;
                userForTrainingCourses.UserEdit.ConfirmPassword = password;

                var json = new JavaScriptSerializer().Serialize(userForTrainingCourses);
                var client = new RestClient(APIUrl + "api/UserService/EditUser");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse APIresponse = client.Execute(request);
                int StatusCode = (int)APIresponse.StatusCode;
                if (StatusCode == 200)
                {
                    employeesEntities.SaveChanges();
                }*/
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
            finally
            {
                if (sqlConnection != null)
                    ((IDisposable)sqlConnection).Dispose();
            }
        }
        
        public void UpdateWrongPasswordAttempts(int userId, int previousAttempts, bool reset)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                int passwordAttempts = 0;
                bool accountStatus = true;
                if (!reset)
                {
                    passwordAttempts = previousAttempts + 1;
                }
                if (passwordAttempts >= 3)
                {
                    accountStatus = false;
                }
                string updatePasswordAttempts = "Update Users Set WrongPaswordAttempts=@wrongPaswordAttempts,IsActive=@active Where ID=@userId";
                SqlCommand updateUserAttempts = new SqlCommand(updatePasswordAttempts, sqlConnection);
                updateUserAttempts.Parameters.AddWithValue("@wrongPaswordAttempts", passwordAttempts);
                updateUserAttempts.Parameters.AddWithValue("@active", accountStatus);
                updateUserAttempts.Parameters.AddWithValue("@userId", userId);
                sqlConnection.Open();
                updateUserAttempts.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);

            }
            finally
            {
                if (sqlConnection != null)
                    ((IDisposable)sqlConnection).Dispose();
            }
        }
        
        public bool If2FAEnabled(int userID)
        {
            return employeesEntities.Users.Where(x=>x.ID == userID).Select(x => x.TwoFactorEnabled).FirstOrDefault();
        }
        
        public bool DisableGoogleAuthenticator()
        {
            try
            {
                Users userEntity = new Users();
                userEntity = GeneralUtilities.GetCurrentUser();
                Users user = employeesEntities.Users.Where(x => x.ID == userEntity.ID).FirstOrDefault();
                if (user != null)
                {
                    user.IsGoogleAuthenticatorEnabled = false;
                    user.GoogleAuthenticatorSecretKey = null;
                    user.TwoFactorEnabled = false;
                    employeesEntities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }
        
        public List<string> EnableGoogleAuthenticator()
        {
            try
            {
                List<string> QRCodeInformation = new List<string>();
                Users userEntity = new Users();
                userEntity = GeneralUtilities.GetCurrentUser();
                Users user = employeesEntities.Users.Where(x=>x.ID == userEntity.ID).FirstOrDefault();
                if (!user.TwoFactorEnabled)
                {
                    if (!String.IsNullOrWhiteSpace(userEntity.Username))
                    {
                        byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
                        string issuer = "SinglePoint";
                        string issuerEncoded = HttpUtility.UrlEncode(issuer);
                        string sharedKey = FormatKey(Base32Encoder.Encode(secretKey));
                        string barcodeUrl = KeyUrl.GetTotpUrl(secretKey, userEntity.Username) + "&issuer=" + issuerEncoded;
                        QRCodeInformation.Add(Base32Encoder.Encode(secretKey));
                        QRCodeInformation.Add(barcodeUrl);
                        QRCodeInformation.Add(sharedKey);
                    }
                }
                else
                {
                    QRCodeInformation.Add("2");
                }
                return QRCodeInformation;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        
        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }
        
        public string EnableGoogleAuthenticatorWithVerificationCode(string Code, string SecretKey)
        {
            try
            {
                byte[] secretKey = Base32Encoder.Decode(SecretKey);
                long timeStepMatched = 0;
                string feedBack = "1";
                var otp = new Totp(secretKey);
                Users userEntity = GeneralUtilities.GetCurrentUser();

                if (otp.VerifyTotp(Code.Trim(), out timeStepMatched, new VerificationWindow(2, 2)))
                {
                    
                    Users user = employeesEntities.Users.Where(x => x.ID == userEntity.ID).FirstOrDefault();
                    if (!user.TwoFactorEnabled)
                    {
                        user.IsGoogleAuthenticatorEnabled = true;
                        user.GoogleAuthenticatorSecretKey = SecretKey;
                        user.TwoFactorEnabled = true;
                        //employeesEntities.Entry(user).State = EntityState.Modified;
                        employeesEntities.SaveChanges();
                        Log4Net.WriteLog("Verify OTP Code", LogType.GENERALLOG);
                    }
                    else
                    {
                        return "2";
                    }
                    return feedBack;
                }
                else
                {
                    Log4Net.WriteLog("Not Verify OTP Code", LogType.GENERALLOG);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
    
    }
}
