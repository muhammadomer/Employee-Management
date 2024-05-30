using Database.Entities;
using Database.Models.EmployeeManagement;
using Database.Models.Mitigate;
using Database.Models.SinglePointCloud;
using LogApp;
using Secure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using RestSharp;
using RestSharp.Serialization.Json;
namespace Database.DAL
{
    public class UserDAL : BaseDAL
    {
        SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        SqlCommand sqlCommand;
        private SettingsDAL _settingsDAL;
        private string TC_APIUrl;
        JsonDeserializer deserial;
        public UserDAL()
        {
            _settingsDAL = new SettingsDAL();
            TC_APIUrl = employeesEntities.Applications.Where(x => x.ID == 4).Select(x => x.App_Url).FirstOrDefault();
            deserial = new JsonDeserializer();
        }

        public List<Users> GetAllUsers()
        {
            try
            {
                Users currentUser = GeneralUtilities.GetCurrentUser();

                List<Users> users = new List<Users>();
                int userId = 1;
                string manageAccount = (string)HttpContext.Current.Session["ManageAccount"];
                
                if (manageAccount != null)
                {
                    userId = 0;
                }
                Log4Net.WriteLog("USERDAL.cs userId: " + userId, LogType.GENERALLOG);
                var usersList = employeesEntities.Database.SqlQuery<Users>("SELECT u.ID, [First Name] AS First_Name, [Last Name] AS Last_Name, Email, " +
                    "[Contact Number] AS Contact_Number, [Job Title] AS Job_Title, Username, Password, ProfileImage, OfficeID, u.Office, Region, Department, " +
                    "[Direct Number] As Direct_Number, [Mobile Number] AS Mobile_Number, u.Telephone, [Fax Number] As Fax_Number, u.[Address Line 1] AS Address_Line_1, " +
                    "u.[Address Line 2] AS Address_Line_2, City, u.State, u.[GPS Postal] AS GPS_Postal, u.[Mail Postal] As Mail_Postal, Country, UserTypeID, " +
                    "[Allow Card] AS Allow_Card, IsDeleted, IsActive, DateCreated, ResetHash, ResetRequestDate, LastPasswordChange, WrongPaswordAttempts, SecurityStamp, " +
                    "TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, GoogleAuthenticatorSecretKey, IsGoogleAuthenticatorEnabled, TwoFAEmailCode, TwoFAEmailCodeDateTime, " +
                    "LevelPermissionID, DAC6LevelPermissionID, PracticeGroup, TCLevelPermissionId,TCUserId, l.[Name],RolePersmission FROM Users u INNER JOIN [Location] l ON l.ID = u.OfficeID ORDER BY [Last Name]")
                    .Where(x => x.ID > userId && x.IsDeleted == false).ToList();
                //employeesEntities.Users.Where(x => x.ID > userId && x.IsDeleted == false).OrderBy(x => x.First_Name)
                //.Select(x => new
                //{
                //    x.ID,
                //    x.First_Name,
                //    x.Last_Name,
                //    x.Email,
                //    x.Mobile_Number,
                //    x.Job_Title,
                //    x.Username,
                //    x.Location.Name,
                //    x.Region,
                //    x.Department,
                //    x.PracticeGroup,
                //    x.UserTypeID,
                //    x.ProfileImage,
                //    x.TCLevelPermissionId
                //}).OrderBy(x => x.Last_Name).ToList();


                Log4Net.WriteLog("USERDAL.cs usersList.count: " + usersList.Count, LogType.GENERALLOG);
                Log4Net.WriteLog("USERDAL.cs currentUser.UserTypeID: " + currentUser.UserTypeID, LogType.GENERALLOG);
                foreach (var item in usersList)
                {

                    
                    Log4Net.WriteLog("USERDAL.cs item.UserTypeID: " + item.Username, LogType.GENERALLOG);
                    if (currentUser.UserTypeID == 3 && item.UserTypeID != 2)
                    {
                        continue;
                    }
                    Users user = new Users();
                    user.ID = item.ID;
                    user.First_Name = item.First_Name;
                    user.Last_Name = item.Last_Name;
                    user.Email = item.Email;
                    user.Mobile_Number = item.Mobile_Number;
                    user.Job_Title = item.Job_Title;
                    user.Username = item.Username;
                    user.Office = employeesEntities.Location.Where(x => x.ID == item.OfficeID).Select(x => x.Name).FirstOrDefault(); ;
                    user.Region = item.Region;
                    user.Department = item.Department;
                    user.PracticeGroup = item.PracticeGroup;
                    user.UserTypeName = employeesEntities.UserTypes.Where(x => x.ID == item.UserTypeID).Select(x => x.User_Type).FirstOrDefault();
                    string tcUserType = "";
                    if (item.TCLevelPermissionId == 1)
                    {
                        tcUserType = " (Admin)";
                    }
                    else if (item.TCLevelPermissionId == 2)
                    {
                        tcUserType = " (User)";
                    }
                    user.UserTypeName += tcUserType;
                    user.ProfileImage = string.IsNullOrEmpty(item.ProfileImage)  ? "images/user.png" : item.ProfileImage;

                    string path = HttpContext.Current.Server.MapPath(user.ProfileImage);
                    if(!File.Exists(path))
                        path = HttpContext.Current.Server.MapPath("images/user.png");

                    if (File.Exists(path) && new FileInfo(path).Length > 0 && !item.ProfileImage.Equals("images/user.png"))
                    {
                        byte[] imageArray = File.ReadAllBytes(path);
                        string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                        user.ProfileImage = "data:image/png;base64," + base64ImageRepresentation;
                    }
                    else
                    {
                        user.ProfileImage = item.ProfileImage;
                    }

                    users.Add(user);
                }

                return users;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public string AddUser(Users userEntity, bool isImport = false)
        {
            try
            {
                string simplePassword = "";
                var currentUser = GeneralUtilities.GetCurrentUser();
                string feedback = null;

                var regex = SettingsDAL.regex;
                var match = Regex.Match(userEntity.Password, regex);



                if (IfUsernameAlreadyExist(userEntity.Username, userEntity.ID))
                {
                    feedback = "This Username already exists.";
                }
                else if (IfEmailAlreadyExist(userEntity.UserTypeID, userEntity.Email, userEntity.ID))
                {
                    feedback = "This Email already exists.";
                }
                else if (!match.Success)
                {
                    feedback = "Passwords must have at least 8 characters with one in uppercase, one lowercase, one number and one special character.";
                }
                else
                {
                    simplePassword = userEntity.Password;
                    byte[] passwordSalt = Cryptography.Cryptography.GenerateSalt();
                    string password = userEntity.Password;
                    userEntity.Password = Cryptography.Cryptography.ComputeHash(userEntity.Password, passwordSalt);
                    userEntity.DateCreated = DateTime.Now;
                    userEntity.ProfileImage = "images/user.png";

                    if (userEntity.UploadImage == true)
                    {
                        string profileImage = HttpContext.Current.Session["UserPImage"].ToString();

                        if (!string.IsNullOrWhiteSpace(profileImage))
                        {
                            string accountID = "0";
                            if (ConfigurationManager.AppSettings["onPrem"] == "0" && !string.IsNullOrWhiteSpace(HttpContext.Current.Session["UserAccountID"].ToString()))
                            {
                                accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                            }

                            profileImage = profileImage.Replace("data:image/png;base64,", "");
                            byte[] bytes = Convert.FromBase64String(profileImage);

                            string ImagePath = "upload/" + accountID + "/" + userEntity.ID.ToString() + "/";
                            var folderName = HttpContext.Current.Server.MapPath("~/" + ImagePath);
                            if (!Directory.Exists(folderName))
                            {
                                Directory.CreateDirectory(folderName);
                            }

                            DirectoryInfo dirInfo = new DirectoryInfo(folderName);
                            foreach (FileInfo file in dirInfo.GetFiles())
                            {
                                file.Delete();
                            }

                            Guid imageName = Guid.NewGuid();
                            File.WriteAllBytes(Path.Combine(folderName, imageName.ToString() + ".png"), Convert.FromBase64String(profileImage));

                            userEntity.ProfileImage = ImagePath + imageName.ToString() + ".png";

                        }
                    }
                    if (userEntity.RemoveImage == true)
                    {
                        userEntity.ProfileImage = "images/user.png";
                    }

                    userEntity.TCUserId = 0;
                    employeesEntities.Users.AddOrUpdate(userEntity);
                    employeesEntities.SaveChanges();



                    if (!userEntity.IsSuperAdmin)
                    {
                        // Insert User in Training Courses
                        Log4Net.WriteLog("Updating user : " + userEntity.First_Name + " on TC with URL : " + TC_APIUrl + "api/UserService/CreateUser", LogType.GENERALLOG);
                        if (userEntity.ApplcaiotnsList != null && userEntity.ApplcaiotnsList.Where(x => x.ID == 5).Count() > 0)
                        {
                            Log4Net.WriteLog("TC service", LogType.GENERALLOG);
                            userEntity.UserAdd = new UserAddModel();
                            userEntity.UserAdd.Email = userEntity.Email + "+" + currentUser.CompanyId;
                            userEntity.UserAdd.FirstName = userEntity.First_Name;
                            userEntity.UserAdd.LastName = userEntity.Last_Name;
                            userEntity.UserAdd.Password = password;
                            userEntity.UserAdd.ConfirmPassword = password;
                            var json = new JavaScriptSerializer().Serialize(userEntity);
                            Log4Net.WriteLog("Calling TC service", LogType.GENERALLOG);
                            var client = new RestClient(TC_APIUrl + "api/UserService/CreateUser");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddParameter("application/json", json, ParameterType.RequestBody);
                            IRestResponse APIresponse = client.Execute(request);                            
                            int StatusCode = (int)APIresponse.StatusCode;
                            int UserId = 0;
                            Log4Net.WriteLog("Response retrun from Training course : " + APIresponse.Content + " wist status code: " + APIresponse.StatusCode, LogType.GENERALLOG);
                            if (StatusCode == 200)
                            {
                                //UserId = APIresponse.Content.Trim() == "" ? 0 : int.Parse(APIresponse.Content);
                                UserId = deserial.Deserialize<int>(APIresponse);
                                if (UserId > 0)
                                {
                                    Log4Net.WriteLog("User Id return from Training course : " + UserId, LogType.GENERALLOG);
                                    userEntity.TCUserId = UserId;
                                }
                                employeesEntities.Users.AddOrUpdate(userEntity);
                                employeesEntities.SaveChanges();
                            }
                        }
                        else if (isImport == true && userEntity.ApplcaiotnsList != null && userEntity.ApplcaiotnsList.Where(x => x.ID == 2).Count() > 0)
                        {
                            int employeeID = userEntity.ID;
                            var userApp = new UsersApplications();
                            var userPermission = new UserPermissions();

                            userApp.Application_ID = 2;
                            userApp.User_ID = employeeID;

                            employeesEntities.UsersApplications.Add(userApp);
                            employeesEntities.SaveChanges();

                            if(userEntity.MitigateModulePermissionsList != null)
                            {
                                foreach (var item in userEntity.MitigateModulePermissionsList)
                                {
                                    userPermission = new UserPermissions();
                                    userPermission.User_ID = employeeID;
                                    userPermission.Permission_ID = item.ID;

                                    mitigateEntities.UserPermissions.Add(userPermission);
                                    mitigateEntities.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            employeesEntities.Users.AddOrUpdate(userEntity);
                            employeesEntities.SaveChanges();
                        }
                        if(!isImport)
                        {
                            userEntity.MitigateModulePermissionsList = new List<PermissionEntity>();
                            userEntity.MitigateModulePermissionsList.Add(new PermissionEntity
                            {
                                ID = 1,
                                Name = "Dashboard"
                            });
                        }
                        
                        
                    }

                    SendCredentialsOnCreatingUser(userEntity, simplePassword);
                    feedback = "Employee inserted successfully.id" + userEntity.ID;
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }



        private bool MovProfileImageIntoUserDirectory(int userID, string currentLogoPath, string methood)
        {
            try
            {
                currentLogoPath = currentLogoPath.Split('/').Last();
                string currentfilePath = HttpContext.Current.Server.MapPath("~\\upload\\" + currentLogoPath);
                string newFilePath = HttpContext.Current.Server.MapPath("~\\upload\\" + userID);

                if (!File.Exists(currentfilePath))
                {
                    File.Create(currentfilePath);
                }

                if (!Directory.Exists(newFilePath))
                {
                    Directory.CreateDirectory(newFilePath);
                }

                newFilePath = newFilePath + "\\" + currentLogoPath;

                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }
                File.Move(currentfilePath, newFilePath);

                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "UPDATE [dbo].[Users] SET [ProfileImage] = @profileImage WHERE ID = @userID";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@profileImage", "upload/" + userID + "/" + currentLogoPath);
                sqlCommand.Parameters.AddWithValue("@userID", userID);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public string UpdateUser(Users userEntity, bool IsPermissions)
        {
            try
            {
                var currentUser = GeneralUtilities.GetCurrentUser();
                string feedback = null;
                if (IfUsernameAlreadyExist(userEntity.Username, userEntity.ID))
                {
                    feedback = "This Username already exists.";
                }
                else if (IfEmailAlreadyExist(userEntity.UserTypeID, userEntity.Email, userEntity.ID))
                {
                    feedback = "This Email already exists.";
                }
                else
                {                    
                
                    Users user = employeesEntities.Users.Find(userEntity.ID);
                    if (IsPermissions)
                    {
                        
                        if (userEntity.RiskManagerLevelPermissions.Count > 0)
                        {
                            user.LevelPermissionID = userEntity.RiskManagerLevelPermissions.Min(x => x.ID);
                        }
                        if (userEntity.DAC6UserLevelPermissionsList != null && userEntity.DAC6UserLevelPermissionsList.Count > 0)
                        {
                            user.DAC6LevelPermissionID = userEntity.DAC6UserLevelPermissionsList.Min(x => x.ID);
                        }
                    }

                    user.First_Name = userEntity.First_Name;
                    user.Last_Name = userEntity.Last_Name;
                    string previousEmail = user.Email;
                    user.Email = userEntity.Email;
                    user.Contact_Number = userEntity.Contact_Number;
                    user.Job_Title = userEntity.Job_Title;
                    user.Username = userEntity.Username;
                    user.Office = userEntity.Office;
                    user.OfficeID = userEntity.OfficeID;
                    user.Region = userEntity.Region;
                    user.Department = userEntity.Department;
                    user.PracticeGroup = userEntity.PracticeGroup;
                    user.Country = userEntity.Country;
                    user.State = userEntity.State;
                    user.City = userEntity.City;
                    user.GPS_Postal = userEntity.GPS_Postal;
                    user.Mail_Postal = userEntity.Mail_Postal;
                    user.Address_Line_1 = userEntity.Address_Line_1;
                    user.Address_Line_2 = userEntity.Address_Line_2;
                    user.Telephone = userEntity.Telephone;
                    user.Direct_Number = userEntity.Direct_Number;
                    user.Mobile_Number = userEntity.Mobile_Number;
                    user.Fax_Number = userEntity.Fax_Number;
                    //user.ProfileImage = "images/user.png";
                    user.UserTypeID = userEntity.UserTypeID;
                    user.Allow_Card = userEntity.Allow_Card;
                    user.IsDeleted = userEntity.IsDeleted;
                    user.IsActive = userEntity.IsActive;
                    user.WrongPaswordAttempts = user.IsActive == true ? 0 : 4;
                    user.IsSuperAdmin = userEntity.IsSuperAdmin;
                    int prevUserLevel = user.TCLevelPermissionId;
                    user.TCLevelPermissionId = userEntity.TCLevelPermissionId;

                    if (userEntity.UploadImage == true)
                    {
                        string profileImage = HttpContext.Current.Session["UserPImage"].ToString();

                        if (!string.IsNullOrWhiteSpace(profileImage))
                        {
                            string accountID = "0";
                            if (ConfigurationManager.AppSettings["onPrem"] == "0" && !string.IsNullOrWhiteSpace(HttpContext.Current.Session["UserAccountID"].ToString()))
                            {
                                 accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                              //  accountID = HttpContext.Current.Session["DBId"]; should be used in next version
                            }

                            profileImage = profileImage.Replace("data:image/png;base64,", "");
                            byte[] bytes = Convert.FromBase64String(profileImage);

                            string ImagePath = "upload/" + accountID + "/" + userEntity.ID.ToString() + "/";
                            var folderName = HttpContext.Current.Server.MapPath("~/" + ImagePath);
                            if (!Directory.Exists(folderName))
                            {
                                Directory.CreateDirectory(folderName);
                            }

                            DirectoryInfo dirInfo = new DirectoryInfo(folderName);
                            foreach (FileInfo file in dirInfo.GetFiles())
                            {
                                file.Delete();
                            }

                            Guid imageName = Guid.NewGuid();
                            File.WriteAllBytes(Path.Combine(folderName, imageName.ToString() + ".png"), bytes);

                            user.ProfileImage = ImagePath + imageName.ToString() + ".png";

                        }
                    }
                    if (userEntity.RemoveImage == true)
                    {
                        user.ProfileImage = "images/user.png";
                    }

                    employeesEntities.Users.AddOrUpdate(user);
                    employeesEntities.SaveChanges();

                    
                    if (!userEntity.IsSuperAdmin && IsPermissions)
                    {
                        Log4Net.WriteLog("Updating user : " + userEntity.First_Name + " on TC with URL : " + TC_APIUrl + "api/UserService/CreateUser", LogType.GENERALLOG);
                        DeleteUserPermissions(userEntity.ID);
                        if (userEntity.ApplcaiotnsList.Where(x => x.ID == 5).Count() > 0)
                        {
                            Log4Net.WriteLog("TC service", LogType.GENERALLOG);

                            if(user.TCUserId == 0)
                                Log4Net.WriteLog("Training course user id not inserted at INSERTION time for user with email : " + userEntity.Email, LogType.GENERALLOG);

                            string IsDeleted = "false";
                            Users userForTrainingCourses = new Users();
                            userForTrainingCourses.UserAdd = new UserAddModel();
                            userForTrainingCourses.UserAdd.Email = userEntity.Email + "$$$" + currentUser.CompanyId + "$$$" + prevUserLevel + "$$$" + userEntity.TCLevelPermissionId + "$$$" + previousEmail + "$$$" + user.TCUserId + "$$$" + IsDeleted;
                            userForTrainingCourses.UserAdd.FirstName = userEntity.First_Name;
                            userForTrainingCourses.UserAdd.LastName = userEntity.Last_Name;
                            userForTrainingCourses.UserAdd.Password = "Test";
                            userForTrainingCourses.UserAdd.ConfirmPassword = "Test";
                            Log4Net.WriteLog("Calling TC service", LogType.GENERALLOG);
                            var json = new JavaScriptSerializer().Serialize(userForTrainingCourses);
                            var client = new RestClient(TC_APIUrl + "api/UserService/CreateUser");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddParameter("application/json", json, ParameterType.RequestBody);
                            IRestResponse APIresponse = client.Execute(request);
                            int StatusCode = (int)APIresponse.StatusCode;
                            int UserId = 0;
                            Log4Net.WriteLog("Response retrun from Training course : " + APIresponse.Content + " with status code: " + APIresponse.StatusCode, LogType.GENERALLOG);
                            if (StatusCode == 200)
                            {
                                //UserId = APIresponse.Content.Trim() == "" ? 0 : int.Parse(APIresponse.Content);
                                UserId = deserial.Deserialize<int>(APIresponse);
                                if (UserId > 0)
                                {
                                    Log4Net.WriteLog("User Id return from Training course : " + UserId, LogType.GENERALLOG);
                                    user.TCUserId = UserId;
                                }
                                employeesEntities.Users.AddOrUpdate(user);
                                employeesEntities.SaveChanges();
                            }
                        }
                        employeesEntities.Users.AddOrUpdate(user);
                        employeesEntities.SaveChanges();
                        InsertApplications(userEntity.ID, userEntity.ApplcaiotnsList);
                        InsertUserModulesPermissions(userEntity.ID, userEntity.FileRepositoryModulePermissionsList);
                        InsertFileTypePermissions(userEntity.ID, userEntity.FileTypePermissionsList);
                        InsertFileRepositoryPermissions(userEntity.ID, userEntity.FileRepositoryPermissionsList);
                        InsertRiskManagerModulesPermissions(userEntity.ID, userEntity.MitigateModulePermissionsList);
                        InsertUserBoards(userEntity.ID, userEntity.MitigateBoardsList);
                        InsertUserLevelPermissions(userEntity.ID, userEntity.RiskManagerLevelPermissions);
                        InsertBusinessCardPermissions(userEntity.ID, userEntity.BusinessCardsModulesPermissionsList);
                        InsertDAC6Permissions(userEntity.ID, userEntity.DAC6ModulesPermissionsList);
                        InsertDAC6LevelPermissions(userEntity.ID, userEntity.DAC6UserLevelPermissionsList);
                    }
                    
                    feedback = "Employee updated successfully.";
                }
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public string UpdateUserLogin(Users userEntity)
        {
            try
            {
                Users user = employeesEntities.Users.Find(userEntity.ID);
                user.First_Name = userEntity.First_Name;
                user.Last_Name = userEntity.Last_Name;
                user.Direct_Number = userEntity.Direct_Number;
                user.Mobile_Number = userEntity.Mobile_Number;
                user.Fax_Number = userEntity.Fax_Number;

                if (userEntity.UploadImage == true)
                {
                    string profileImage = HttpContext.Current.Session["UserPImage"].ToString();

                    if (!string.IsNullOrWhiteSpace(profileImage))
                    {
                        string accountID = "0";
                        if (ConfigurationManager.AppSettings["onPrem"] == "0" && !string.IsNullOrWhiteSpace(HttpContext.Current.Session["UserAccountID"].ToString()))
                        {
                            accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                        }

                        profileImage = profileImage.Replace("data:image/png;base64,", "");
                        byte[] bytes = Convert.FromBase64String(profileImage);


                        string ImagePath = "upload/" + accountID + "/" + userEntity.ID.ToString() + "/";
                        var folderName = HttpContext.Current.Server.MapPath("~/" + ImagePath);

                        if (!Directory.Exists(folderName))
                        {
                            Directory.CreateDirectory(folderName);
                        }

                        DirectoryInfo dirInfo = new DirectoryInfo(folderName);
                        foreach (FileInfo file in dirInfo.GetFiles())
                        {
                            file.Delete();
                        }

                        Guid imageName = Guid.NewGuid();
                        File.WriteAllBytes(Path.Combine(folderName, imageName.ToString() + ".png"), Convert.FromBase64String(profileImage));

                        user.ProfileImage = ImagePath + imageName.ToString() + ".png";

                    }
                }
                if (userEntity.RemoveImage == true)
                {
                    user.ProfileImage = "images/user.png";
                }


                employeesEntities.Users.AddOrUpdate(user);
                employeesEntities.SaveChanges();

                Users currentUser = new Users();
                
                currentUser = GeneralUtilities.GetCurrentUser();
                currentUser.ProfileImage = user.ProfileImage;
                HttpContext.Current.Session["UserEntity"] = currentUser;

                string feedback = "Employee updated successfully.";
                return feedback;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public bool IfUsernameAlreadyExist(string username, int userID)
        {
            try
            {
                Log4Net.WriteLog("username: " + username + " || userID: " + userID, LogType.GENERALLOG);
                bool userExist = false;
                if (userID != 0)
                {
                    if (employeesEntities.Users.Where(x => x.Username == username && x.ID != userID && x.IsDeleted == false).Count() > 0)
                        userExist = true;
                }
                else
                {
                    if (employeesEntities.Users.Where(x => x.Username == username && x.IsDeleted == false).Count() > 0)
                        userExist = true;
                }
                Log4Net.WriteLog("userExist: " + userExist, LogType.GENERALLOG);
                return userExist;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public string GetUserType(string username)
        {
            try
            {
                string userType = null;
                DataTable dataTable = new DataTable();
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "select ut.[User Type] from users u inner join UserTypes ut on u.UserTypeID = ut.ID  where Username = @username and [IsDeleted] = @isDeleted";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@username", username);
                sqlCommand.Parameters.AddWithValue("@isDeleted", 0);
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    userType = row["User Type"].ToString();
                }
                return userType;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        private bool IfEmailAlreadyExist(int userTypeID, string email, int userID)
        {
            try
            {
                bool emailExist = false;
                if (email.EndsWith("ghost-digital.com"))
                {
                    return false;
                }
                if (userID != 0)
                {
                    if (employeesEntities.Users.Where(x => x.Email == email && x.UserTypeID == userTypeID && x.ID != userID).Count() > 0)
                    {
                        emailExist = true;
                    }
                }
                else
                {
                    if (employeesEntities.Users.Where(x => x.Email == email && x.UserTypeID == userTypeID && x.IsDeleted == false).Count() > 0)
                    {
                        emailExist = true;
                    }
                }
                if (employeesEntities.Users.Where(x => x.Email == email && x.ID == 1).Count() > 0)
                {
                    emailExist = true;
                }
                return emailExist;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool DeleteUserByID(int userID)
        {
            try
            {
                var currentUser = GeneralUtilities.GetCurrentUser();
                string emailformat = "";
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    emailformat = ConfigurationManager.AppSettings["OtherApplicationName"].ToString().ToLower().Replace(" ","") + ".com";
                }
                Users user = employeesEntities.Users.Where(x => x.ID == userID).FirstOrDefault();
                string previousEmail = user.Email;
                user.Email = userID + "@" + emailformat;
                user.Username = userID.ToString();
                user.IsDeleted = true;
                employeesEntities.SaveChanges();


                string IsDeleted = "true";
               
                    Log4Net.WriteLog("Deleting user : " + user.First_Name + " with TC user id : " + user.TCUserId, LogType.GENERALLOG);
                //if (user.ApplcaiotnsList.Where(x => x.ID == 5).Count() > 0)
                try
                {
                    Log4Net.WriteLog("TC service", LogType.GENERALLOG);
                    Users userForTrainingCourses = new Users();
                    userForTrainingCourses.UserAdd = new UserAddModel();
                    userForTrainingCourses.UserAdd.Email = user.Email + "$$$" + currentUser.CompanyId + "$$$" + user.TCLevelPermissionId + "$$$" + user.TCLevelPermissionId + "$$$" + previousEmail + "$$$" + user.TCUserId + "$$$" + IsDeleted;
                    userForTrainingCourses.UserAdd.FirstName = user.First_Name;
                    userForTrainingCourses.UserAdd.LastName = user.Last_Name;
                    userForTrainingCourses.UserAdd.Password = "Test";
                    userForTrainingCourses.UserAdd.ConfirmPassword = "Test";
                    Log4Net.WriteLog("Calling TC service", LogType.GENERALLOG);
                    var json = new JavaScriptSerializer().Serialize(userForTrainingCourses);
                    var client = new RestClient(TC_APIUrl + "api/UserService/CreateUser");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                    IRestResponse APIresponse = client.Execute(request);
                    int StatusCode = (int)APIresponse.StatusCode;
                    if (StatusCode == 200)
                    {
                        Log4Net.WriteLog("User deleted in TC as well.", LogType.GENERALLOG);
                    }
                }
                catch(Exception ex)
                {
                    Log4Net.WriteException(ex);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public Users CheckUserAuthentication(string userName, string password)
        {
            try
            {
                Users user = employeesEntities.Users.Where(x => x.Username == userName && x.IsDeleted == false).FirstOrDefault();
                bool isPasswordVerified = false;
                if (user.Password != null)
                {
                    isPasswordVerified = Cryptography.Cryptography.VerifyHash(password, user.Password);
                    if (isPasswordVerified && user.IsActive)
                    {
                        Security securityManagement = new Security();
                        securityManagement.UpdateWrongPasswordAttempts(user.ID, user.WrongPaswordAttempts, true);
                        string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                        if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                        {
                            Accounts account = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).FirstOrDefault();
                            user.SupportEmails = account.SupportEmails;
                            user.CompanyId = account.TrainingCoursesCompanyId;
                        }
                        else
                        {
                            user.SupportEmails = ConfigurationManager.AppSettings["SupportEmails"];
                        }
                        user.UserAuthenticated = true;
                    }
                    else
                    {
                        Security securityManagement = new Security();
                        securityManagement.UpdateWrongPasswordAttempts(user.ID, user.WrongPaswordAttempts, false);
                        user.WrongPaswordAttempts = user.WrongPaswordAttempts + 1;
                        user.UserAuthenticated = false;
                    }
                }
                else
                {
                    user = null;
                }
                return user;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        public Users UserAuthenticationForCloud(string Username)
        {
            try
            {
                Users userDetail = employeesEntities.Users.Where(x => x.Username == Username).SingleOrDefault(); 
                string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                {
                    Accounts account = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).FirstOrDefault();
                    userDetail.SupportEmails = account.SupportEmails;
                    userDetail.CompanyId = account.TrainingCoursesCompanyId;
                }
                else
                {
                    userDetail.SupportEmails = ConfigurationManager.AppSettings["SupportEmails"];
                }
                return userDetail;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public bool IsAccountDeleted()
        {
            bool bReturn = false;
            try
            {
                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                    if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                    {
                        //bReturn = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && !a.Active).Select(a => a.IsDeleted).FirstOrDefault();
                        Models.SinglePointCloud.Accounts objAccount = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).FirstOrDefault();
                        if (accountID.ToLower() == "gdddemo")
                        {
                            objAccount = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID)).FirstOrDefault();
                        }
                        
                        if (objAccount == null)
                        {
                            bReturn = true;
                        }
                        /*else if(!objAccount.Active)
                        {
                            bReturn = objAccount.IsDeleted;
                        }*/

                        Log4Net.WriteLog("Account deleted from cloud: " + bReturn.ToString(), LogType.GENERALLOG);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }

            return bReturn;
        }
        public bool IsAccountEnabled()
        {
            bool bReturn = true;
            try
            {
                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    string accountID = HttpContext.Current.Session["UserAccountID"].ToString();
                    if (accountID != null && !string.IsNullOrWhiteSpace(accountID))
                    {
                        bReturn = singlePointCloudEntities.Accounts.Where(a => a.UserID.Equals(accountID) && a.IsDeleted == false).Select(a => a.Active).FirstOrDefault();
                        Log4Net.WriteLog("Account status from cloud: " + bReturn.ToString(), LogType.GENERALLOG);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }

            return bReturn;
        }

      


        public string LoginUser(Users userEntity)
        {
            string feedback = null;
            try
            {
                
                if (userEntity != null)
                {
                    if (userEntity.UserAuthenticated)
                    {
                        
                        //if (IsAccountDeleted())
                        //{
                        //    feedback = "Account not found.";
                        //}
                        if (!userEntity.IsActive || !IsAccountEnabled())
                        {
                            feedback = "Your account is temporarily disabled.";
                        }
                        else if (userEntity.UserTypeID == 1)
                        {
                            if (userEntity.TwoFactorEnabled && _settingsDAL.IsEnabledTwoFA())
                            {
                                feedback = "1";
                            }
                            else if (!userEntity.TwoFactorEnabled && _settingsDAL.IsEnabledTwoFA())
                            {
                                feedback = "Verification not sent on your email";
                                if (SendEmailVerificationCode(userEntity))
                                {
                                    feedback = "5";
                                }
                            }
                            else
                            {
                                if (String.IsNullOrWhiteSpace(userEntity.LastPasswordChange.ToString()))
                                {
                                    HttpContext.Current.Session["UserEntity"] = userEntity;
                                    feedback = "2";
                                }
                                else
                                {
                                    HttpContext.Current.Session["UserEntity"] = userEntity;
                                    feedback = "3";
                                }
                            }

                        }
                        else if (userEntity.UserTypeID == 3)
                        {
                            if (userEntity.TwoFactorEnabled && _settingsDAL.IsEnabledTwoFA())
                            {
                                feedback = "1";
                            }
                            else if (!userEntity.TwoFactorEnabled && _settingsDAL.IsEnabledTwoFA())
                            {
                                feedback = "Verification not sent on your email";
                                if (SendEmailVerificationCode(userEntity))
                                {
                                    feedback = "5";
                                }
                            }
                            else
                            {
                                if (String.IsNullOrWhiteSpace(userEntity.LastPasswordChange.ToString()))
                                {
                                    HttpContext.Current.Session["UserEntity"] = userEntity;
                                    feedback = "2";
                                }
                                else
                                {
                                    HttpContext.Current.Session["UserEntity"] = userEntity;
                                    feedback = "3";
                                }
                            }

                        }
                        else if (userEntity.UserTypeID == 2)
                        {
                            if (String.IsNullOrWhiteSpace(userEntity.LastPasswordChange.ToString()))
                            {
                                HttpContext.Current.Session["UserEntity"] = userEntity;
                                feedback = "2";
                            }
                            else if (userEntity.TwoFactorEnabled && _settingsDAL.IsEnabledTwoFA())
                            {
                                feedback = "1";
                            }
                            else if (!userEntity.TwoFactorEnabled && _settingsDAL.IsEnabledTwoFA())
                            {
                                SendEmailVerificationCode(userEntity);
                                feedback = "5";
                            }
                            else
                            {
                                if (String.IsNullOrWhiteSpace(userEntity.LastPasswordChange.ToString()))
                                {
                                    HttpContext.Current.Session["UserEntity"] = userEntity;
                                    feedback = "2";
                                }
                                else
                                {
                                    HttpContext.Current.Session["UserEntity"] = userEntity;
                                    feedback = "4";
                                }
                            }
                        }

                    }
                    else
                    {
                        if (userEntity.WrongPaswordAttempts < 3)
                        {
                            int remaningAttempts = 3 - userEntity.WrongPaswordAttempts;
                            feedback = "Invalid Username or Password. " + remaningAttempts + " attempt(s) remaining";
                        }
                        else
                        {
                            feedback = "Your account has been temporarily disabled";
                        }
                    }
                }
                else
                {
                    feedback = "Invalid Username or Password.";
                }

                if (feedback == null)
                {
                    feedback = "Invalid Username or Password. Please try again.";
                }
            }
            catch (Exception ex)
            {
                
                Log4Net.WriteException(ex);
            }
            Log4Net.WriteLog("feedback: " + feedback, LogType.GENERALLOG);
            return feedback;
        }

        public bool IsApplicationAssignedToUser(int userId, int applicationId)
        {
            bool bReturn = false;
            try
            {
                int id = employeesEntities.UsersApplications.Where(x => x.User_ID == userId && x.Application_ID == applicationId).Select(x => x.ID).FirstOrDefault();
                if (id > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);

            }
            return bReturn;
        }

        private bool InsertApplications(int userID, List<Applications> applications)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = null;
                for (int i = 0; i < applications.Count; i++)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [UsersApplications] WHERE [User ID] = @userID;";
                    }
                    query += "INSERT INTO[dbo].[UsersApplications]([User ID],[Application ID]) VALUES(@userID, @applicationID)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@applicationID", applications[i].ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertUserModulesPermissions(int userID, List<PermissionEntity> modulesPermissions)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = null;
                int i = 0;
                foreach (var module in modulesPermissions)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [UsersModules] WHERE [User ID] = @userID;";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[UsersModules] ([User ID],[Permission ID]) VALUES (@userID,@permissionID)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@permissionID", module.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertFileTypePermissions(int userID, List<PermissionEntity> filePermissions)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = null;
                int i = 0;
                foreach (var fileType in filePermissions)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [UserFilesPermissions] WHERE [User ID] = @userID;";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[UserFilesPermissions]([User ID],[File Category ID]) VALUES (@userID,@fileCategoryID)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@fileCategoryID", fileType.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertFileRepositoryPermissions(int userID, List<PermissionEntity> fileRepositoryPermissions)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = null;
                int i = 0;
                foreach (var fileType in fileRepositoryPermissions)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [UsersRepositoryPermissions] WHERE [User ID] = @userID;";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[UsersRepositoryPermissions]([User ID],[Repository ID])VALUES(@userID,@permissionID)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@permissionID", fileType.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertRiskManagerModulesPermissions(int userID, List<PermissionEntity> riskManagerModulesPermissions)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = null;
                for (int i = 0; i < riskManagerModulesPermissions.Count; i++)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [UserPermissions] WHERE [User ID] = @userID;";
                    }
                    query += "INSERT INTO [UserPermissions] ( [User ID] , [Permission ID] ) VALUES ( @userID , @permissionID )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@permissionID", riskManagerModulesPermissions[i].ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertUserBoards(int userID, List<PermissionEntity> userBoards)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = null;
                int i = 0;
                foreach (var board in userBoards)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [dbo].[StaticUserBoards] WHERE [User ID] = @userID; ";
                        i++;
                    }
                    query += "INSERT INTO [StaticUserBoards] ( [User ID] , [Board ID] ) VALUES ( @userID , @boardID )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@boardID", board.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertUserLevelPermissions(int userID, List<UsersLevelPermissions> usersLevelPermissions)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = null;
                int i = 0;
                foreach (var userLP in usersLevelPermissions)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [dbo].[UsersLevelPermissions] WHERE [UserID] = @userID;";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[UsersLevelPermissions] ( [UserID] , [LevelPermissionID] ) VALUES ( @userID , @levelPermissionID )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@levelPermissionID", userLP.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertBusinessCardPermissions(int userID, List<PermissionEntity> businessCardModulePermissionsList)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("BusinessCard"));
                string query = null;
                int i = 0;
                foreach (var module in businessCardModulePermissionsList)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [dbo].[UsersModules] WHERE [User ID] = @userID; ";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[UsersModules] ( [User ID] , [Module ID] ) VALUES ( @userID , @moduleID )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@moduleID", module.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertDAC6LevelPermissions(int userID, List<Database.Models.DAC6.UserLevelPermission> userPermission)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DAC6"));
                string query = null;
                int i = 0;
                foreach (var permission in userPermission)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [dbo].[UserLevelPermission] WHERE [UserID] = @userID; ";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[UserLevelPermission] ( [UserID] , [LevelPermissionID] ) VALUES ( @userID , @levelPermissionID )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@levelPermissionID", permission.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        private bool InsertDAC6Permissions(int userID, List<PermissionEntity> dac6ModulePermissionsList)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DAC6"));
                string query = null;
                int i = 0;
                foreach (var module in dac6ModulePermissionsList)
                {
                    query = "";
                    if (i == 0)
                    {
                        query += "DELETE FROM [dbo].[DAC6UsersModules] WHERE [User ID] = @userID; ";
                        i++;
                    }
                    query += "INSERT INTO [dbo].[DAC6UsersModules] ( [User ID] , [Module ID] ) VALUES ( @userID , @moduleID )";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@userID", userID);
                    sqlCommand.Parameters.AddWithValue("@moduleID", module.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public UserEntity GetUserInfoByID(int ID)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                DataTable dataTable = new DataTable();
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "SELECT u.[ID],ut.[User Type], ISNULL(u.OfficeID,0) AS OfficeID, [First Name],[Last Name],[Email],[Contact Number],[PracticeGroup],[Job Title],[Username],[Password],[ProfileImage],l.[Name] as [Office], r.Name as [Region],[Department],[Direct Number],[Mobile Number],l.[Telephone],[Fax Number],l.[Address Line 1],l.[Address Line 2],Cities.Name as [City],l.[State],l.[GPS Postal],l.[Mail Postal],Countries.Name as [Country],[UserTypeID],[Allow Card],[IsDeleted],[IsActive],[TCLevelPermissionId] FROM [Users] u inner join UserTypes ut on u.UserTypeID = ut.ID  left join Location l on l.ID = u.OfficeID left join Regions r on r.ID = l.RegionID left join Cities on Cities.ID = l.CityID left join Countries on Countries.ID = l.CountryID where IsDeleted = 0 and u.ID =@userID";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@userID", ID);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    userEntity.UserID = Convert.ToInt32(row["ID"]);
                    userEntity.UserType = row["User Type"].ToString();
                    userEntity.OfficeID = Convert.ToInt32(row["OfficeID"]);
                    userEntity.FirstName = row["First Name"].ToString();
                    userEntity.LastName = row["Last Name"].ToString();
                    userEntity.Email = row["Email"].ToString();
                    userEntity.ContacNumber = row["Contact Number"].ToString();
                    userEntity.JobTitle = row["Job Title"].ToString();
                    userEntity.UserName = row["Username"].ToString();
                    userEntity.Password = row["Password"].ToString();
                    userEntity.Office = row["Office"].ToString();
                    userEntity.Region = row["Region"].ToString();
                    userEntity.Department = row["Department"].ToString();
                    userEntity.PracticeGroup = row["PracticeGroup"].ToString();
                    userEntity.DirectNumber = row["Direct Number"].ToString();
                    userEntity.MobileNumber = row["Mobile Number"].ToString();
                    userEntity.Telephone = row["Telephone"].ToString();
                    userEntity.FaxNumber = row["Fax Number"].ToString();
                    userEntity.AddressLine1 = row["Address Line 1"].ToString();
                    userEntity.AddressLine2 = row["Address Line 2"].ToString();
                    userEntity.City = row["City"].ToString();
                    userEntity.State = row["State"].ToString();
                    userEntity.GPSPostal = row["GPS Postal"].ToString();
                    userEntity.MailPostal = row["Mail Postal"].ToString();
                    userEntity.Country = row["Country"].ToString();
                    userEntity.UserTypeID = Convert.ToInt32(row["UserTypeID"]);
                    userEntity.ProfileImage = row["ProfileImage"].ToString();
                    userEntity.AllowCard = String.IsNullOrWhiteSpace(row["Allow Card"].ToString()) ? false : Convert.ToBoolean(row["Allow Card"]);
                    userEntity.IsActive = Convert.ToBoolean(row["IsActive"]);
                    userEntity.TCLevelPermissionId = Convert.ToInt32(row["TCLevelPermissionId"]);
                }
                if (userEntity.UserType != "Super Admin")
                {
                    userEntity.ApplcaiotnsList = GetUserApplicationsListByID(ID);

                    List<ApplicationEntity> ApplicationList = GetApplicationsList();
                    if (ApplicationList != null)
                    {
                        foreach (var application in ApplicationList)
                        {
                            if (application.ID == 1)
                            {
                                userEntity.FileRepositoryModulePermissionsList = GetUserPermissionsListByID(ID);
                                userEntity.FileTypePermissionsList = GetUserFilePermissionsListByID(ID);
                                userEntity.FileRepositoryPermissionsList = GetUserRepositoryPermissionsListByID(ID);
                            }
                            else if (application.ID == 2)
                            {
                                userEntity.MitigateModulePermissionsList = GetDentonsMitigateUserModulesPermissionsListByID(ID);
                                userEntity.MitigateBoardsList = GetDentonsMitigateUserBoardsByID(ID);
                                userEntity.RiskManagerUserLevelPermissionsList = GetRiskManagerUserLevelsPermissionsByID(ID);
                            }
                            else if (application.ID == 3)
                            {
                                userEntity.BusinessCardsModulesPermissionsList = GetBusinessCardModulesPermissionsByID(ID);
                            }
                            else if (application.ID == 4)
                            {
                                userEntity.DAC6ModulesPermissionsList = GetDAC6ModulesPermissionsByID(ID);
                                userEntity.DAC6UserLevelPermissionsList = GetDAC6UserLevelsPermissionsByID(ID);
                            }
                        }
                    }                    
                }

                string completePath = HttpContext.Current.Server.MapPath(userEntity.ProfileImage);
                if (!File.Exists(completePath))
                    completePath = HttpContext.Current.Server.MapPath("images/user.png");

                if (!userEntity.ProfileImage.Equals("images/user.png") && File.Exists(completePath) && new FileInfo(completePath).Length > 0)
                {
                    byte[] imageArray = File.ReadAllBytes(completePath);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    userEntity.ProfileImage = "data:image/png;base64," + base64ImageRepresentation;
                }

                return userEntity;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public string ChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            try
            {
                string feedBack = "";
                Users userEntity = new Users();
                userEntity = GeneralUtilities.GetCurrentUser();
                bool isPasswordVerified = Cryptography.Cryptography.VerifyHash(oldPassword, userEntity.Password);
                if (isPasswordVerified)
                {
                    byte[] passwordSalt = Cryptography.Cryptography.GenerateSalt();
                    string passwordHash = Cryptography.Cryptography.ComputeHash(newPassword, passwordSalt);
                    sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                    string query = "UPDATE [dbo].[Users] SET [Password] = @password WHERE ID = @userID";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@password", passwordHash);
                    sqlCommand.Parameters.AddWithValue("@userID", userEntity.ID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    HttpContext.Current.Session["UserEntity"] = (Users)CheckUserAuthentication(userEntity.Username, newPassword);
                    feedBack = "Password changed sucessfully";
                }
                else
                {
                    feedBack = "Please provide correct Old Password";
                }
                return feedBack;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        public string ChangeEncryptedPassword(string oldPassword, string newPassword)
        {
            try
            {
                string feedBack = "";
                
                DentonsEmployeesSettings settings = employeesEntities.DentonsEmployeesSettings.SingleOrDefault();
                string hashCode = "";
                bool isPasswordVerified = true;
                if (!String.IsNullOrWhiteSpace(settings.HashCode))
                {
                    if (oldPassword != LicInformation.DecryptString(settings.HashCode))
                    {
                        isPasswordVerified = false;
                    }
                }
                if (isPasswordVerified)
                {
                    if (UpdateDAC6HashCode(oldPassword, newPassword))
                    {
                        hashCode = LicInformation.EncryptString(newPassword);
                        settings.HashCode = hashCode;
                        employeesEntities.DentonsEmployeesSettings.AddOrUpdate(settings);
                        employeesEntities.SaveChanges();
                        feedBack = "1"; //Password changed sucessfully
                    }
                    else
                    {
                        feedBack = "101";
                    }
                }
                else
                {
                    feedBack = "2"; //Please provide correct Old Password
                }
                return feedBack;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        private bool UpdateDAC6HashCode(string oldPassword, string newPassword)
        {
            bool bReturn = false;
            try
            {
                using (sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DAC6")))
                {
                    using (SqlCommand cmd = new SqlCommand("ChangeHashCode", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OldKey", oldPassword);
                        cmd.Parameters.AddWithValue("@NewKey", newPassword);
                        sqlConnection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                bReturn = true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            
            return bReturn;
        }

        private void DeleteUserPermissions(int userID)
        {
            try
            {
                employeesEntities.UsersApplications.RemoveRange(employeesEntities.UsersApplications.Where(x => x.User_ID == userID));
                employeesEntities.SaveChanges();
            }
            catch(Exception ex)
            {
                Log4Net.WriteException(ex);
            }

            List<ApplicationEntity> ApplicationList = GetApplicationsList();
            if (ApplicationList != null)
            {
                foreach (var application in ApplicationList)
                {
                    try
                    {
                        if (application.ID == 1)
                        {
                            fileManagementEntities.UserFilesPermissions.RemoveRange(fileManagementEntities.UserFilesPermissions.Where(x => x.User_ID == userID));
                            fileManagementEntities.UsersModules.RemoveRange(fileManagementEntities.UsersModules.Where(x => x.User_ID == userID));
                            fileManagementEntities.UsersRepositoryPermissions.RemoveRange(fileManagementEntities.UsersRepositoryPermissions.Where(x => x.User_ID == userID));
                            fileManagementEntities.SaveChanges();
                        }
                        else if (application.ID == 2)
                        {
                            mitigateEntities.UserPermissions.RemoveRange(mitigateEntities.UserPermissions.Where(x => x.User_ID == userID));
                            mitigateEntities.StaticUserBoards.RemoveRange(mitigateEntities.StaticUserBoards.Where(x => x.User_ID == userID));
                            mitigateEntities.UsersLevelPermissions.RemoveRange(mitigateEntities.UsersLevelPermissions.Where(x => x.UserID == userID));
                            mitigateEntities.SaveChanges();
                        }
                        else if (application.ID == 3)
                        {
                            businessCardEntities.UsersModules.RemoveRange(businessCardEntities.UsersModules.Where(x => x.User_ID == userID));
                            businessCardEntities.SaveChanges();
                        }
                        else if (application.ID == 4)
                        {
                            dac6Entities.UserLevelPermission.RemoveRange(dac6Entities.UserLevelPermission.Where(x => x.UserId == userID));
                            dac6Entities.DAC6UsersModules.RemoveRange(dac6Entities.DAC6UsersModules.Where(x => x.User_ID == userID));
                            dac6Entities.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.WriteException(ex);
                    }
                }
            }
        }

        ////////////// Get Users DropDowns Lists //////////
        public List<PermissionEntity> GetPermissionsList()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = "select ID,[Permission Name] from Modules;";
                List<PermissionEntity> fileRepositoryModulePermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    fileRepositoryModulePermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Permission Name"].ToString()
                    });
                }
                return fileRepositoryModulePermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetFilePermissionsList()
        {
            try
            {
                List<PermissionEntity> filePermissionsList = new List<PermissionEntity>();
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = "select [ID], [File Category Name] from FilesCategory  where IsArchive = @isArchive";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@isArchive", 0);
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    filePermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["File Category Name"].ToString()
                    });
                }
                return filePermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetRepositoryPermissionsList()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = "select [ID], [Repository Name] from RepositoryPermissions";
                List<PermissionEntity> repositoryPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    repositoryPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Repository Name"].ToString()
                    });
                }
                return repositoryPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetRiskManagerModulePermissionsList()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = "SELECT [ID] , Description as [Name] FROM [Permissions] where Visible = @visible";
                List<PermissionEntity> dentonsMitigateModulePermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@visible", 1);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    dentonsMitigateModulePermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return dentonsMitigateModulePermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetAllRiskManagerUserBoards()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = "SELECT [ID] , [BoardName] FROM [StaticBoards]";
                List<PermissionEntity> dentonsMitigateBoardList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    dentonsMitigateBoardList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["BoardName"].ToString()
                    });
                }
                return dentonsMitigateBoardList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetAllRiskManagerUserLevelPermissions()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = "SELECT [ID] , [Name] FROM [LevelPermissions] where Visible = @visible order by OrderId";
                List<PermissionEntity> dentonsMitigateBoardList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@visible", 1);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    dentonsMitigateBoardList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return dentonsMitigateBoardList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetAllBusinessCardsPermissionsList()
        {
            try
            {
                Log4Net.WriteLog("Initialize Sql Connection", LogType.GENERALLOG);
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("BusinessCard"));
                string query = "SELECT [ID] , [Name] FROM [Modules]";
                List<PermissionEntity> businessCardPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                Log4Net.WriteLog("Start Fill Datatable ", LogType.GENERALLOG);
                sqlDataAdapter.Fill(dataTable);
                Log4Net.WriteLog("End Fill Datatable ", LogType.GENERALLOG);
                sqlConnection.Close();
                Log4Net.WriteLog("Data Table Rows" + dataTable.Rows.Count.ToString(), LogType.GENERALLOG);
                foreach (DataRow row in dataTable.Rows)
                {
                    Log4Net.WriteLog("", LogType.GENERALLOG);
                    Log4Net.WriteLog(row["Name"].ToString(), LogType.GENERALLOG);
                    businessCardPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return businessCardPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }


        public List<PermissionEntity> GetAllADAC6UserLevelPermissions()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DAC6"));
                string query = "SELECT [ID] , [Permission] FROM [LevelPermission] where Visible = @visible order by OrderId";
                List<PermissionEntity> dentonsMitigateBoardList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@visible", 1);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    dentonsMitigateBoardList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Permission"].ToString()
                    });
                }
                return dentonsMitigateBoardList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }
        public List<PermissionEntity> GetAllDAC6PermissionsList()
        {
            try
            {
                Log4Net.WriteLog("Initialize Sql Connection", LogType.GENERALLOG);
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DAC6"));
                string query = "SELECT [ID] , [Name] FROM [DAC6Modules]";
                List<PermissionEntity> dac6PermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                Log4Net.WriteLog("Start Fill Datatable ", LogType.GENERALLOG);
                sqlDataAdapter.Fill(dataTable);
                Log4Net.WriteLog("End Fill Datatable ", LogType.GENERALLOG);
                sqlConnection.Close();
                Log4Net.WriteLog("Data Table Rows" + dataTable.Rows.Count.ToString(), LogType.GENERALLOG);
                foreach (DataRow row in dataTable.Rows)
                {
                    Log4Net.WriteLog("", LogType.GENERALLOG);
                    Log4Net.WriteLog(row["Name"].ToString(), LogType.GENERALLOG);
                    dac6PermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return dac6PermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<ApplicationEntity> GetApplicationsList()
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                string query = "select ID, [Application Name] , [Modal Name] from Applications where IsDeleted = @isDeleted";
                List<ApplicationEntity> applicationsList = new List<ApplicationEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@isDeleted", 0);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    applicationsList.Add(new ApplicationEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Application Name"].ToString(),
                        ModalName = row["Modal Name"].ToString()
                    });
                }
                return applicationsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetUserPermissionsListByID(int ID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = "SELECT [User ID],[Permission ID],Modules.[Permission Name] FROM [dbo].[UsersModules] inner join Modules on UsersModules.[Permission ID]=Modules.ID where [User ID] = " + ID;
                List<PermissionEntity> permissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    permissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["Permission ID"]),
                        Name = row["Permission Name"].ToString()
                    });
                }
                return permissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetUserFilePermissionsListByID(int ID)
        {
            try
            {
                List<PermissionEntity> filePermissionsList = new List<PermissionEntity>();
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = "SELECT [User ID],[File Category ID],FilesCategory.[File Category Name] FROM [dbo].[UserFilesPermissions] inner join FilesCategory on UserFilesPermissions.[File Category ID] = FilesCategory.ID where  FilesCategory.IsArchive = @isArchive and [User ID] = @userID ";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@isArchive", 0);
                sqlCommand.Parameters.AddWithValue("@userID", ID);
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    filePermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["File Category ID"]),
                        Name = row["File Category Name"].ToString()
                    });
                }
                return filePermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        //Get User ID on Username
        public int GetUserIdByUsername(string username="")
        {
            try
            {

                int ID = -1;

                if(username != "")
                {
                    ID = employeesEntities.Users.Where(a => a.Username.ToLower() == username.ToLower()).FirstOrDefault().ID;

                }

                return ID;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        public int GetLevelPermissionIDOnPermissionName(string name = "")
        {
            try
            {

                int ID = -1;

                if (name != "")
                {
                    ID = mitigateEntities.LevelPermissions.Where(a => a.Visible == true && a.Name.ToLower() == name.ToLower()).FirstOrDefault().ID;

                }

                return ID;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        public int GetMitigateUserAccessLevelIdByAccessLevelName(string level = "")
        {
            try
            {

                int ID = -1;

                if (level != "")
                {
                    ID = mitigateEntities.LevelPermissions.Where(a => a.Name.ToLower() == level.ToLower()).FirstOrDefault().ID;
                }

                return ID;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        public int GetUserIdByEmail(string email = "")
        {
            try
            {

                int ID = -1;

                if (email != "")
                {
                    ID = employeesEntities.Users.Where(a => a.Email.ToLower() == email.ToLower()).FirstOrDefault().ID;
                }

                return ID;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return -1;
            }
        }

        public List<PermissionEntity> GetUserRepositoryPermissionsListByID(int ID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("FileManagement"));
                string query = "SELECT [User ID],[Repository ID],RepositoryPermissions.[Repository Name] FROM [dbo].[UsersRepositoryPermissions] inner join RepositoryPermissions on UsersRepositoryPermissions.[Repository ID] = RepositoryPermissions.ID where [User ID] = " + ID;
                List<PermissionEntity> repositoryPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    repositoryPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["Repository ID"]),
                        Name = row["Repository Name"].ToString()
                    });
                }
                return repositoryPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetDentonsMitigateUserModulesPermissionsListByID(int ID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = "Select p.ID , p.Name from [Permissions] p inner join [UserPermissions] up on p.ID = up.[Permission ID] where up.[User ID] = @userID and Visible = @visible";
                List<PermissionEntity> repositoryPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@userID", ID);
                sqlCommand.Parameters.AddWithValue("@visible", 1);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    repositoryPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return repositoryPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetDentonsMitigateUserBoardsByID(int ID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("Mitigate"));
                string query = "Select b.ID , b.[BoardName] from [StaticBoards] b inner join [StaticUserBoards] ub on b.ID = ub.[Board ID] where ub.[User ID] = @userID";
                List<PermissionEntity> repositoryPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@userID", ID);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    repositoryPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["BoardName"].ToString()
                    });
                }
                return repositoryPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<UsersLevelPermissions> GetRiskManagerUserLevelsPermissionsByID(int ID)
        {
            try
            {
                var recordList = employeesEntities.Users.Where(x => x.ID == ID).ToList();
                List<UsersLevelPermissions> usersLevelPermissions = new List<UsersLevelPermissions>();
                foreach (var item in recordList)
                {
                    UsersLevelPermissions singleUserLevelPermission = new UsersLevelPermissions();
                    singleUserLevelPermission.LevelPermissionID = item.LevelPermissionID;
                    singleUserLevelPermission.UserID = item.ID;
                    usersLevelPermissions.Add(singleUserLevelPermission);
                }
                
                return usersLevelPermissions;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetBusinessCardModulesPermissionsByID(int ID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("BusinessCard"));
                string query = "Select m.ID , m.Name from [Modules] m inner join [UsersModules] um on m.ID = um.[Module ID] where um.[User ID] = @userID";
                List<PermissionEntity> businessCardModulesPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@userID", ID);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    businessCardModulesPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return businessCardModulesPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<Database.Models.DAC6.UserLevelPermission> GetDAC6UserLevelsPermissionsByID(int ID)
        {
            try
            {
                var recordList = employeesEntities.Users.Where(x => x.ID == ID).ToList();
                List<Database.Models.DAC6.UserLevelPermission> usersLevelPermissions = new List<Database.Models.DAC6.UserLevelPermission>();
                foreach (var item in recordList)
                {
                    Database.Models.DAC6.UserLevelPermission singleUserLevelPermission = new Database.Models.DAC6.UserLevelPermission();
                    if (item.DAC6LevelPermissionID == null)
                    {
                        item.DAC6LevelPermissionID = 0;
                    }
                    singleUserLevelPermission.LevelPermissionId = Convert.ToInt32(item.DAC6LevelPermissionID);
                    singleUserLevelPermission.UserId = item.ID;
                    usersLevelPermissions.Add(singleUserLevelPermission);
                }

                return usersLevelPermissions;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<PermissionEntity> GetDAC6ModulesPermissionsByID(int ID)
        {
            try
            {
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DAC6"));
                string query = "Select m.ID , m.Name from [DAC6Modules] m inner join [DAC6UsersModules] um on m.ID = um.[Module ID] where um.[User ID] = @userID";
                List<PermissionEntity> businessCardModulesPermissionsList = new List<PermissionEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@userID", ID);
                sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    businessCardModulesPermissionsList.Add(new PermissionEntity
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Name = row["Name"].ToString()
                    });
                }
                return businessCardModulesPermissionsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<ApplicationEntity> GetUserApplicationsListByID(int ID)
        {
            try
            {
                string SecureLicense = "1";
                try
                {
                    SecureLicense = ConfigurationManager.AppSettings["SecureLicense"];
                }
                catch { SecureLicense = "1"; }
                string LicenseMsg = "";
                int LicCount = 0;
                sqlConnection = new SqlConnection(GeneralUtilities.GetConnectionString("DentonsEmployees"));
                List<ApplicationEntity> applicationsList = new List<ApplicationEntity>();
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("EXEC [dbo].[GetAllUserApplications] @userID =" + ID, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
                foreach (DataRow row in dataTable.Rows)
                {
                    LicenseMsg = "";
                    LicCount = 0;
                    if (Convert.ToInt32(row["Application ID"]) == 2)
                    {
                        LicInformation.ServerStatus objStatus = IsValidLicense(out LicCount, LicInformation.ServerLicense.MitigateServer);
                        if (objStatus == LicInformation.ServerStatus.Expire)
                        {
                            LicenseMsg = "License Expired";
                        }
                        else if(objStatus == LicInformation.ServerStatus.Trial)
                        {
                            LicenseMsg = "Trial License";
                        }
                    }

                    if(SecureLicense == "0")
                    {
                        LicenseMsg = "";
                    }

                    applicationsList.Add(new ApplicationEntity
                    {
                        ID = Convert.ToInt32(row["Application ID"]),
                        UserID = row["User ID"].ToString(),
                        Name = row["Application Name"].ToString(),
                        Icon = row["Icon"].ToString(),
                        LicenseCount = LicCount,
                        LicenseText = LicenseMsg
                    });
                }
                return applicationsList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public List<Applications> GetAllApplications()
        {
            try
            {
                var applications = employeesEntities.Applications.Where(a => a.IsDeleted == false).ToList();
                return applications;
            }
            catch (Exception ex)
            {
                LogApp.Log4Net.WriteException(ex);
                return null;
            }
        }

        public LicInformation.ServerStatus IsValidLicense(out int TotalLicense, LicInformation.ServerLicense licenseType)
        {
            TotalLicense = 0;
            LogApp.Log4Net.WriteLog("Checking License.", LogApp.LogType.GENERALLOG);
            LicInformation.ServerStatus objStatus = LicInformation.ServerStatus.Expire;
            try
            {
                string ConnectionString = GeneralUtilities.GetConnectionString("DentonsEmployees");
                string ClientDB = GeneralUtilities.GetDatabaseName("DentonsEmployees");
                LicInformation information = new LicInformation(ConnectionString, ClientDB);

                objStatus = information.serverLicenseStatus(licenseType);
                if (objStatus == LicInformation.ServerStatus.Expire)
                {
                    LogApp.Log4Net.WriteLog("Server License is Expired!, Stopping Server.", LogApp.LogType.ERRORLOG);                    
                }
                else if (objStatus == LicInformation.ServerStatus.Trial)
                {
                    TotalLicense = 15;
                }
                else
                {
                    //TotalLicense = information.clientLicenseStatus(LicInformation.ClientLicense.MitigateClient);                    
                    TotalLicense = 999999;
                }
            }
            catch (Exception exp)
            { LogApp.Log4Net.WriteLog("IsValidLicense: " + exp.Message, LogApp.LogType.GENERALLOG); }
            return objStatus;
        }


        public bool SendEmailVerificationCode(Users userEntity)
        {
            try
            {
                
                DentonsEmployeesSettings settings = _settingsDAL.GetGeneralSettings();

                Random random = new Random();
                string randomNumber = random.Next(1000, 9999).ToString();
                Users user = employeesEntities.Users.Where(x => x.Username == userEntity.Username).FirstOrDefault();
                user.TwoFAEmailCode = randomNumber;
                user.TwoFAEmailCodeDateTime = DateTime.Now;
                employeesEntities.SaveChanges();

                //MailMessage mail = new MailMessage(settings.MailUsername, userEntity.Email);
                //SmtpClient client = new SmtpClient();
                //client.Port = settings.SMTPPort;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Host = settings.MailServer;
                //mail.Subject = "Dentons Employee - SMTP Test Message";
                //mail.Body = "This is an email message sent by Dentons Employees application while testing the SMTP settings.";
                //client.Send(mail);

                string applicationURL = "";// "https://dentons.singlepoint.live/";

                try
                {
                    applicationURL = employeesEntities.DentonsEmployeesSettings.Select(x => x.DentonsEmployeesURL).FirstOrDefault();
                }
                catch { 
                    //applicationURL = "https://dentons.singlepoint.live/"; 
                }

                string AppName = "Single Point";
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    AppName = ConfigurationManager.AppSettings["OtherApplicationName"].ToString();
                }

                    applicationURL = applicationURL + "/images/heading-image.png";

                MailMessage mail = new MailMessage();
                mail.To.Add(userEntity.Email);
                mail.From = new MailAddress(settings.MailUsername);
                mail.Subject = AppName + " verification code";

                string Body = "<div style='margin-bottom: 30px'><img src='"+ applicationURL + "' ></div></p>Below is the authentication code required for you to login to "+ AppName + ".</p> <h1><b>" + randomNumber + "</b></h1><p>If you have any issues obtaining access, please contact your system administrator.</p>";
                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = settings.MailServer; //Or Your SMTP Server Address
                smtp.Port = settings.SMTPPort; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (settings.MailUsername, settings.MailPassword);
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool SendCredentialsOnCreatingUser(Users userEntity, string password)
        {
            try
            {

                DentonsEmployeesSettings settings = _settingsDAL.GetGeneralSettings();

                string applicationURL = "";
                

                try
                {
                    applicationURL = employeesEntities.DentonsEmployeesSettings.Select(x => x.DentonsEmployeesURL).FirstOrDefault();
                }
                catch
                { }

                if (settings.MailUsername == "" || settings.MailServer == "")
                    return false;

                string AppName = "Single Point";
                if (Convert.ToInt32(ConfigurationManager.AppSettings["OtherApplication"]) == 1)
                {
                    AppName = ConfigurationManager.AppSettings["OtherApplicationName"].ToString();
                }

                string imageURL = applicationURL + "/images/heading-image.png";

                MailMessage mail = new MailMessage();
                mail.To.Add(userEntity.Email);
                mail.From = new MailAddress(settings.MailUsername);
                mail.Subject = "You are now part of " + AppName;

                string Body = "<div style='margin-bottom: 30px'><img src='" + imageURL + "' ></div></p>" +
                        "<p>Dear " + userEntity.First_Name + " " + userEntity.Last_Name + "</p>" +
                        "<p>A User has been created for you on the " + AppName + ", your details are listed below.</p>" +
                        "<p><b>The URL is:</b> " + applicationURL + "</p>" +
                        "<p><b>Your Username is:</b> " + userEntity.Username + "</p>" +
                        "<p><b>Your Password is:</b> " + password + "</p>";

                if (ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    string AccountID = HttpContext.Current.Session["UserAccountID"].ToString();
                    Body += "<p><b>Your Account ID is:</b> " + AccountID +"</p>";
                }
               

                Body += "<p>By logging in, you will see which products are available to you and what your access rights are</p><p><b>Administrator</b></p>";
                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = settings.MailServer; //Or Your SMTP Server Address
                smtp.Port = settings.SMTPPort; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (settings.MailUsername, settings.MailPassword);
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool VerifyEmailCode(Users userEntity, string code)
        {
            try
            {
                bool ifVerified = false;
                //DateTime? dateTime = DentonsEmployeesEntities.Users.Where(x => x.Username == userEntity.Username && x.TwoFAEmailCode == code).Select(x => x.TwoFAEmailCodeDateTime).FirstOrDefault();
                //if (String.IsNullOrWhiteSpace(dateTime.ToString()))
                //{
                //    if (dateTime.Value.Hour < 3)
                //    {

                //    }
                //}
                if (employeesEntities.Users.Where(x => x.Username == userEntity.Username && x.TwoFAEmailCode == code).Count() > 0)
                {
                    ifVerified = true;
                }
                return ifVerified;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }

        public bool Reset2FA(Users userEntity)
        {
            try
            {
                Users user = employeesEntities.Users.Where(x => x.ID == userEntity.ID).FirstOrDefault();
                if (user != null)
                {
                    user.IsGoogleAuthenticatorEnabled = false;
                    user.GoogleAuthenticatorSecretKey = null;
                    user.TwoFactorEnabled = false;
                    employeesEntities.SaveChanges();
                }
                if (SendEmailVerificationCode(userEntity))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }
        }



    }
}
