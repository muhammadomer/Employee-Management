using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using EmployeeManagement.Responses;
using LogApp;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace EmployeeManagement
{
    /// <summary>
    /// Summary description for ImportEmployeeContents
    /// </summary>
    public class ImportEmployeeContents : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            LogApp.Log4Net.WriteLog("In process event....", LogType.GENERALLOG);
            var fname = context.Request.QueryString["fname"];
            context.Response.Write(JsonConvert.SerializeObject(ImportExcelFile(fname)));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static void WriteEvent(string data, bool IsError)
        {

            LogApp.Log4Net.WriteLog("In Write Event ....", LogType.GENERALLOG);
            HttpContext.Current.Response.ContentType = "text/event-stream";
            LogApp.Log4Net.WriteLog("In Write Event .... after content type", LogType.GENERALLOG);

            data = $"data: {data}\n\n";

            LogApp.Log4Net.WriteLog("Resp Data::: " + data.ToString(), LogType.GENERALLOG);

            System.Threading.Thread.Sleep(100);
            HttpContext.Current.Response.Write(data);
            LogApp.Log4Net.WriteLog("In Write Event .... before response.flush", LogType.GENERALLOG);
            HttpContext.Current.Response.Flush();

            if (IsError)
            {
                System.Threading.Thread.Sleep(1000);
                HttpContext.Current.Response.Write("Close!Closing Event stream.");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }

        }

        public static UploadFilesResponse ImportExcelFile(string fileName)
        {
            try
            {
                Log4Net.WriteLog("step 1", LogType.GENERALLOG);
                FileInfo fileInfo = new FileInfo(fileName);

                Log4Net.WriteLog("step 2: " + fileName, LogType.GENERALLOG);
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage package = new ExcelPackage(fileInfo);

                Log4Net.WriteLog("step 3", LogType.GENERALLOG);
                LocationDAL locationDAL = new LocationDAL();
                string errorMessage = string.Empty;
                List<string> actualOfficeColumnsList = GetFilesCoulmnsList();

                Log4Net.WriteLog("step 4", LogType.GENERALLOG);
                List<OfficeEntity> officesList = new List<OfficeEntity>();
                List<UserEntity> employeesList = new List<UserEntity>();
                List<string> successList = new List<string>();
                List<string> errorList = new List<string>();

                Log4Net.WriteLog("step 5", LogType.GENERALLOG);
                var worksheets = package.Workbook.Worksheets.ToList();

                Log4Net.WriteLog("step 6: " + worksheets.Count.ToString(), LogType.GENERALLOG);
                ExcelWorksheet EmployeeSheet = package.Workbook.Worksheets[0];
                ExcelWorksheet OfficeSheet = package.Workbook.Worksheets[1];

                Log4Net.WriteLog("step 7", LogType.GENERALLOG);
                if (OfficeSheet.Dimension == null && EmployeeSheet.Dimension == null)
                {
                    errorList.Add("Your file is empty which you are trying to upload");
                    WriteEvent("Your file is empty which you are trying to upload", true);
                    return new UploadFilesResponse() { Mode = 1, status = "Your file is empty which you are trying to upload" };
                }
                var OfficeStart = OfficeSheet.Dimension.Start;
                var EmployeeStart = EmployeeSheet.Dimension.Start;
                var OfficeEnd = OfficeSheet.Dimension.End;
                var EmployeeEnd = EmployeeSheet.Dimension.End;
                int OfficeColumn = OfficeStart.Column;
                int EmployeeColumn = EmployeeStart.Column;
                int OfficeColumns = 10;
                int EmployeeColumns = 13;

                List<OfficeEntity> listOfOfficesForEmailParameter;
                OfficeEntity officeEntityForEmail = new OfficeEntity();

                System.Collections.Hashtable htOwnerEmail = new System.Collections.Hashtable();

                OfficeEntity office = null;
                string CellType = "";
                int sendEventCount = 0;
                Log4Net.WriteLog("step 8", LogType.GENERALLOG);
                if (OfficeEnd.Row <= 100)
                {
                    sendEventCount = 1;
                }
                else
                {
                    sendEventCount = Convert.ToInt32(Math.Floor((OfficeEnd.Row + EmployeeEnd.Row) / 100.0));
                }
                Log4Net.WriteLog("Total no. of rows to import: " + (OfficeEnd.Row + EmployeeEnd.Row), LogType.GENERALLOG);
                //int loggedInUserID = Convert.ToInt32(Session["UserID"]);
                var loggedInUserDetail = new Database.Utility.LoggedinUserDetail();
                
                //Offices Sheet
                for (int row = 1; row <= OfficeEnd.Row; row++)
                {
                    try
                    {
                        errorMessage = string.Empty;
                        if (row == 1)
                        {
                            List<string> uploadedFileColumnsList = new List<string>();
                            for (int i = 0; i < OfficeColumns; i++)
                            {
                                string columnName = OfficeSheet.Cells[row, OfficeColumn + i].Text.ToString().Trim();
                                if (!string.IsNullOrWhiteSpace(columnName))
                                {
                                    uploadedFileColumnsList.Add(columnName);
                                }
                            }

                        }
                        else
                        {
                            Log4Net.WriteLog("processing sheet: Offices ::: row: " + row, LogType.GENERALLOG);

                            int columnNumber = 1;

                            string OfficeName = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            bool officeExists = false;
                            if (OfficeName != "")
                            {

                                int ID = locationDAL.GetLocationIDOnName(OfficeName);
                                if (ID != -1)
                                {
                                    officeExists = true;
                                    errorMessage = addErrorMessage("Office already exists", errorMessage);
                                }
                                else
                                {
                                    office = new OfficeEntity();
                                    office.Longitude = "";
                                    office.Latitude = "";
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Office name not specified", errorMessage);
                            }

                            if(!officeExists)
                            {
                                //Office Name
                                office.Name = OfficeName;
                                office.Office = OfficeName;
                                if (office.Name.Length > 100)
                                {
                                    office.Name = "";
                                    errorMessage = addErrorMessage("Office name is too large", errorMessage);

                                }
                                columnNumber++;


                                //Region
                                office.RegionName = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.RegionName != "")
                                {
                                    office.RegionID = locationDAL.GetRegionIDOnName(office.RegionName);
                                    if (office.RegionID <= 0)
                                    {
                                        errorMessage = addErrorMessage("Region name does not exist", errorMessage);
                                        office.RegionID = 0;
                                    }
                                }
                                else
                                {
                                    errorMessage = addErrorMessage("Region name not specified", errorMessage);
                                }
                                columnNumber++;

                                //Country
                                office.CountryName = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.CountryName != "")
                                {
                                    office.CountryID = locationDAL.GetCountryIdOnName(office.CountryName);
                                    if (office.CountryID <= 0)
                                    {
                                        errorMessage = addErrorMessage("Country name does not exist", errorMessage);
                                        office.RegionID = 0;
                                    }
                                }
                                else
                                {
                                    errorMessage = addErrorMessage("Country name not specified", errorMessage);
                                }
                                columnNumber++;

                                //City
                                office.CityName = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.CityName != "")
                                {
                                    office.CityID = locationDAL.GetCityIdOnName(office.CityName);
                                    if (office.CityID <= 0)
                                    {
                                        errorMessage = addErrorMessage("City name does not exist", errorMessage);
                                        office.CityID = 0;
                                    }
                                }
                                else
                                {
                                    errorMessage = addErrorMessage("City name not specified", errorMessage);
                                }
                                columnNumber++;


                                //state/city/province
                                office.State = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                columnNumber++;

                                //GPS Postal
                                office.GPSPostal = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.GPSPostal != "")
                                {
                                    if (office.GPSPostal.Length > 100)
                                    {
                                        errorMessage = addErrorMessage("GPS Postal Code is too large", errorMessage);
                                    }
                                }
                                columnNumber++;

                                //Mail Postal
                                office.MailPostal = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.MailPostal != "")
                                {
                                    if (office.MailPostal.Length > 100)
                                    {
                                        errorMessage = addErrorMessage("Mail Postal Code is too large", errorMessage);
                                    }
                                }
                                else
                                {
                                    errorMessage = addErrorMessage("Mail Postal Code not specified", errorMessage);
                                }
                                columnNumber++;


                                //Address Line 1
                                office.AddressLine1 = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.AddressLine1 == "")
                                {
                                    errorMessage = addErrorMessage("Address Line 1 not specified", errorMessage);
                                }
                                columnNumber++;

                                //Address Line 2
                                office.AddressLine2 = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                columnNumber++;

                                //Telephone
                                office.Telephone = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                                if (office.Telephone == "")
                                {
                                    errorMessage = addErrorMessage("Telephone not specified", errorMessage);
                                }
                                columnNumber++;
                            }


                            if (string.IsNullOrWhiteSpace(errorMessage))
                            {
                                string resp = "";
                                if (officeExists)
                                {
                                    resp = locationDAL.EditLocation(office);
                                }
                                else
                                {
                                    resp = locationDAL.AddLocation(office);
                                }

                                if (resp == "Office inserted successfully." || resp == "Office updated successfully.")
                                {
                                    successList.Add("Offices - Row " + (row).ToString() + " | Office Name : " + office.Name);
                                }
                                else
                                {
                                    errorList.Add("Offices - Row " + (row).ToString() + " | Office Name : " + office.Name + " | Failed to import office");
                                }

                            }
                            else
                            {
                                errorList.Add("Offices - Row " + (row).ToString() + " | Office Name : " + OfficeName + " | " + errorMessage);

                            }
                            if ((row % sendEventCount) == 0)
                                WriteEvent("total:" + ((OfficeEnd.Row + EmployeeEnd.Row) - 1 - 1).ToString() + "+success:" + successList.Count.ToString() + "+error:" + errorList.Count.ToString(), false);

                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.WriteException(ex);
                        errorList.Add("Invalid Entry Format");
                    }
                }

                //Employee Sheet
                for (int row = 1; row <= EmployeeEnd.Row; row++)
                {
                    try
                    {
                        errorMessage = string.Empty;
                        if (row == 1)
                        {
                            List<string> uploadedFileColumnsList = new List<string>();
                            for (int i = 0; i < EmployeeColumns; i++)
                            {
                                string columnName = EmployeeSheet.Cells[row, EmployeeColumn + i].Text.ToString().Trim();
                                if (!string.IsNullOrWhiteSpace(columnName))
                                {
                                    uploadedFileColumnsList.Add(columnName);
                                }
                            }

                        }
                        else
                        {
                            Log4Net.WriteLog("processing sheet: Employees :::  row: " + row, LogType.GENERALLOG);

                            int columnNumber = 1;

                            var employee = new Users();

                            //************* User **********//
                            employee.UserTypeID = 2;
                            employee.IsActive = true;
                            employee.IsSuperAdmin = false;
                            employee.IsDeleted = false;
                            //*******************************//

                            bool usernameExists = false;
                            bool emailExists = false;

                            //First Name
                            employee.First_Name = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.First_Name != "")
                            {
                                if (employee.First_Name.Length > 100)
                                {
                                    errorMessage = addErrorMessage("First Name is too large", errorMessage);
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("First Name not specified", errorMessage);
                            }
                            columnNumber++;

                            //Last Name
                            employee.Last_Name = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.Last_Name != "")
                            {
                                if (employee.Last_Name.Length > 100)
                                {
                                    errorMessage = addErrorMessage("Last Name is too large", errorMessage);
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Last Name not specified", errorMessage);
                            }
                            columnNumber++;

                            //Job Title
                            employee.Job_Title = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.Job_Title != "")
                            {
                                if (employee.Job_Title.Length > 100)
                                {
                                    errorMessage = addErrorMessage("Job Title is too large", errorMessage);
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Job Title not specified", errorMessage);
                            }
                            columnNumber++;

                            //Email
                            employee.Email = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.Email != "")
                            {
                                if (employee.Email.Length > 100)
                                {
                                    errorMessage = addErrorMessage("Email is too large", errorMessage);
                                }
                                else
                                {
                                    if(new UserDAL().GetUserIdByEmail(employee.Email) > 0)
                                    {
                                        emailExists = true;
                                        errorMessage = addErrorMessage("Email already exists", errorMessage);
                                    }
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Email not specified", errorMessage);
                            }
                            columnNumber++;

                            //Username
                            employee.Username = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.Username != "")
                            {
                                if (employee.Username.Length > 100)
                                {
                                    errorMessage = addErrorMessage("Username is too large", errorMessage);
                                }
                                else
                                {
                                    if (new UserDAL().GetUserIdByUsername(employee.Username) > 0)
                                    {
                                        emailExists = true;
                                        errorMessage = addErrorMessage("Username already exists", errorMessage);
                                    }
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Username not specified", errorMessage);
                            }
                            columnNumber++;

                            //Password
                            employee.Password = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if(employee.Password == "")
                            {
                                errorMessage = addErrorMessage("Password is empty", errorMessage);
                            }
                            columnNumber++;

                            //Office Name
                            employee.Office = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.Office != "")
                            {
                                if (employee.Office.Length > 100)
                                {
                                    errorMessage = addErrorMessage("Office name is too large", errorMessage);
                                }
                                else
                                {
                                    int officeID = locationDAL.GetLocationIDOnName(employee.Office);
                                    if (officeID <= 0)
                                    {
                                        errorMessage = addErrorMessage("Office does not exist", errorMessage);
                                    }
                                    else
                                    {
                                        employee.OfficeID = officeID;
                                    }
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Office not specified", errorMessage);
                            }
                            columnNumber++;

                            //Department Name
                            employee.Department = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (employee.Department != "")
                            {
                                if (employee.Department.Length > 100)
                                {
                                    employee.Department = "";
                                    errorMessage = addErrorMessage("Department name is too large", errorMessage);
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Department not specified", errorMessage);
                            }
                            columnNumber++;

                            //Practice group
                            employee.PracticeGroup = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if(employee.Department.ToLower() == "Practice Group".ToLower())
                            {
                                if (employee.PracticeGroup == "")
                                {
                                    errorMessage = addErrorMessage("Practice Group not specified", errorMessage);
                                }
                            }
                            columnNumber++;

                           // User access level
                            employee.LevelPermissionID = new UserDAL().GetLevelPermissionIDOnPermissionName(EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim());
                            if (employee.LevelPermissionID <= 0)
                            {
                                errorMessage = addErrorMessage("User Access level is not valid", errorMessage);
                            }
                            else
                            {
                                var modulesList = new UserDAL().GetRiskManagerModulePermissionsList();
                                var ApplicationList = new UserDAL().GetAllApplications().Where(a=>a.ID == 2).ToList();

                                employee.ApplcaiotnsList = ApplicationList;

                                if (employee.LevelPermissionID == 1 || employee.LevelPermissionID == 2)
                                {
                                    employee.MitigateModulePermissionsList = modulesList;
                                }
                                else if(employee.LevelPermissionID == 6)
                                {
                                    employee.MitigateModulePermissionsList = modulesList.Where(a => a.ID == 1 || a.ID == 5).ToList();
                                }
                                else
                                {
                                    employee.MitigateModulePermissionsList = modulesList.Where(a => a.ID == 1).ToList();
                                }
                            }
                            columnNumber++;

                            //Direct Number
                            employee.Direct_Number = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            columnNumber++;

                            //Mobile Number
                            employee.Mobile_Number = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            columnNumber++;

                            //Fax Number
                            employee.Fax_Number = EmployeeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            columnNumber++;

                            if (string.IsNullOrWhiteSpace(errorMessage))
                            {
                                string resp = "";

                                resp = new UserDAL().AddUser(employee, true);

                                if (resp.Contains("Employee inserted successfully."))
                                {
                                    successList.Add("Employees - Row " + (row).ToString() + " | Employee Name : " + employee.First_Name + " " + employee.Last_Name);
                                }
                                else
                                {
                                    errorList.Add("Employees - Row " + (row).ToString() + " | Employee Name : " +employee.First_Name + " " + employee.Last_Name + " | " + resp);
                                }

                            }
                            else
                            {
                                errorList.Add("Employees - Row " + (row).ToString() + " | Employee Name : " + employee.First_Name + " " + employee.Last_Name + " | " + errorMessage);
                            }
                            if ((row % sendEventCount) == 0)
                                WriteEvent("total:" + ((OfficeEnd.Row + EmployeeEnd.Row) - 1 - 1).ToString() + "+success:" + successList.Count.ToString() + "+error:" + errorList.Count.ToString(), false);

                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.WriteException(ex);
                        errorList.Add("Invalid Entry Format");
                    }
                }

                
                int importedEntries = successList.Count();
                int errors = errorList.Count();
                string statusImportedRisks = importedEntries + " Entries Generated,  " + errors + " errors";
                string LogFileName = DownloadLog(errorList, successList);
                WriteEvent("Last!" + LogFileName, true);

                return new UploadFilesResponse() { Mode = 1, status = statusImportedRisks };
                //return new UploadFilesResponse() { Mode = 1, status = "total:" + ((OfficeEnd.Row + EmployeeEnd.Row) - 1 - 1).ToString() + "+success:" + successList.Count.ToString() + "+error:" + errorList.Count.ToString() + "+logFile:" + LogFileName };
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return new UploadFilesResponse() { Mode = 2, status = ex.Message, fileName = "" };
            }
        }



        public static string DownloadLog(List<string> errorList, List<string> successList)
        {
            try
            {
                string Path = HostingEnvironment.MapPath("~/upload/");
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);

                string fileName = "ImportedLogs" + Guid.NewGuid() + ".txt";
                string fileNameWithPath = Path + fileName;
                // Delete the file if it exists.
                // Create the file.
                if (!System.IO.File.Exists(fileNameWithPath))
                {
                    //File.Create(fname);
                    TextWriter textWriter = new StreamWriter(fileNameWithPath);
                    if (errorList.Count > 0)
                    {
                        textWriter.WriteLine("----------------------------------------");
                        textWriter.WriteLine("Errors");
                        textWriter.WriteLine("----------------------------------------");

                        foreach (String s in errorList)
                            textWriter.WriteLine(s);
                    }

                    if (successList.Count > 0)
                    {
                        textWriter.WriteLine("");
                        textWriter.WriteLine("");
                        textWriter.WriteLine("----------------------------------------");
                        textWriter.WriteLine("Imported");
                        textWriter.WriteLine("----------------------------------------");

                        foreach (String s in successList)
                            textWriter.WriteLine(s);
                    }



                    textWriter.Close();
                }
                return fileName;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return null;
            }
        }

        public static List<string> GetFilesCoulmnsList()
        {
            List<string> actualFileColumnsList = new List<string>();
            actualFileColumnsList.Add("London Office");
            actualFileColumnsList.Add("Region");
            actualFileColumnsList.Add("Country");
            actualFileColumnsList.Add("City");
            actualFileColumnsList.Add("State/Country/Province");
            actualFileColumnsList.Add("GPS Postal/Zip Code");
            actualFileColumnsList.Add("Mail Postal/Zip Code");
            actualFileColumnsList.Add("Address Line 1");
            actualFileColumnsList.Add("Address Line 2");
            actualFileColumnsList.Add("Telephone");
            return actualFileColumnsList;
        }

        public static string CompareColumns(List<string> actualColumnsList, List<string> uploadedRiskRegisterColumnsList)
        {
            string uploadedRiskRegisterStatus = "Valid";
            if (actualColumnsList.Count == uploadedRiskRegisterColumnsList.Count)
            {
                for (int i = 0; i < actualColumnsList.Count; i++)
                {
                    if (actualColumnsList[i].ToLower() != uploadedRiskRegisterColumnsList[i].ToLower())
                    {
                        int columnNo = i + 1;
                        uploadedRiskRegisterStatus = "The import requires column '" + columnNo + "' to be named: '" + actualColumnsList[i] + " '. However, the column is currently named:  '" + uploadedRiskRegisterColumnsList[i] + " '. Please correct this issue and rerun the import process";
                        break;
                    }
                }
            }
            else
            {
                uploadedRiskRegisterStatus = "The import requires " + actualColumnsList.Count + " columns but this process found " + uploadedRiskRegisterColumnsList.Count + ". Please correct the number of columns and rerun the import process.";
            }
            return uploadedRiskRegisterStatus;
        }


        public static string addErrorMessage(string msg, string errorMessage)
        {
            if (errorMessage != "")
            {
                errorMessage = errorMessage + "," + msg;
            }
            else
            {
                errorMessage = msg;
            }
            return errorMessage;
        }

    }
}