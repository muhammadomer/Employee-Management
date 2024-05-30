using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;
using EmployeeManagement.Responses;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Web.Hosting;

namespace EmployeeManagement
{
    public partial class Offices : System.Web.UI.Page
    {

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
        public static List<OfficeEntity> GetAllLocations()
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                LocationDAL locationDAL = new LocationDAL();
                return locationDAL.GetAllLocations();
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static bool DeleteLocationByID(int locationID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                LocationDAL locationDAL = new LocationDAL();
                return locationDAL.DeleteOfficeByID(locationID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static List<string> GetDistinctColumnValues(string ColName)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                LocationDAL locationDAL = new LocationDAL();
                return locationDAL.GetAllCountriesName(ColName);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        #region FileUpload

        private bool stopProcess = false;

        [HttpPost]
        [WebMethod]
        [ScriptMethod]
        public static UploadFilesResponse Process(string fname)
        {
            LogApp.Log4Net.WriteLog("In process event....", LogType.GENERALLOG);
            //HttpContext.Current.Response.ContentType = "text/event-stream";

            return ImportExcelFile(fname);
        }


        [WebMethod]
        [ScriptMethod]
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
                            Log4Net.WriteLog("processing row: " + row, LogType.GENERALLOG);

                            int columnNumber = 1;

                            string OfficeName = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            bool officeExists = false;
                            if (OfficeName != "")
                            {
                                int ID = locationDAL.GetLocationIDOnName(OfficeName);
                                if (ID != -1)
                                {
                                    officeExists = true;
                                    office = locationDAL.GetLocationInfoByID(ID);
                                }
                                else
                                {
                                    office = new OfficeEntity();
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("Office name not specified", errorMessage);
                            }

                            //Office Name
                            office.Name = OfficeName;
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
                                if(office.RegionID <= 0)
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
                            if (office.State != "")
                            {
                                if (office.State.Length > 100)
                                {
                                    errorMessage = addErrorMessage("State/Contry/Province name is too large", errorMessage);
                                }
                            }
                            else
                            {
                                errorMessage = addErrorMessage("State/Contry/Province name not specified", errorMessage);
                            }
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
                            else
                            {
                                errorMessage = addErrorMessage("GPS Postal Code not specified", errorMessage);
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
                            if (office.AddressLine2 == "")
                            {
                                errorMessage = addErrorMessage("Address Line 2 not specified", errorMessage);
                            }
                            columnNumber++;

                            //Telephone
                            office.Telephone = OfficeSheet.Cells[row, columnNumber].Text.ToString().Trim();
                            if (office.Telephone == "")
                            {
                                errorMessage = addErrorMessage("Telephone not specified", errorMessage);
                            }
                            columnNumber++;


                            if (string.IsNullOrWhiteSpace(errorMessage))
                            {
                                string resp = "";
                                if(officeExists)
                                {
                                    resp = locationDAL.EditLocation(office);
                                }
                                else
                                {
                                    resp = locationDAL.AddLocation(office);
                                }
                                
                                if (resp == "Office inserted successfully." || resp == "Office updated successfully.")
                                {
                                    successList.Add("Row " + (row).ToString() + " | Office Name : " + office.Name);
                                }
                                else
                                {
                                    errorList.Add("Row " + (row).ToString() + " | Office Name : " + office.Name + " | Failed to import office");
                                }

                            }
                            else
                            {
                                errorList.Add("Row " + (row).ToString() + " | Office Name : " + office.Name + " | " + errorMessage);

                            }
                            //if ((row % sendEventCount) == 0)
                            //    WriteEvent("total:" + ((OfficeEnd.Row + EmployeeEnd.Row) - 1 - 1).ToString() + "+success:" + successList.Count.ToString() + "+error:" + errorList.Count.ToString(), false);

                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.WriteException(ex);
                        errorList.Add("Invalid Entry Format");
                    }
                }

                //WriteEvent("total:" + ((OfficeEnd.Row + EmployeeEnd.Row) - 1 - 1).ToString() + "+success:" + successList.Count.ToString() + "+error:" + errorList.Count.ToString(), false);

                int importedEntries = successList.Count();
                int errors = errorList.Count();
                string statusImportedRisks = importedEntries + " Entries Generated,  " + errors + " errors";
                string LogFileName = DownloadLog(errorList, successList);
                //WriteEvent("Last!" + LogFileName, true);

                //return new UploadFilesResponse() { Mode = 1, status = statusImportedRisks };
                return new UploadFilesResponse() { Mode = 1, status = "total:" + ((OfficeEnd.Row + EmployeeEnd.Row) - 1 - 1).ToString() + "+success:" + successList.Count.ToString() + "+error:" + errorList.Count.ToString() };
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


        #endregion


    }
}