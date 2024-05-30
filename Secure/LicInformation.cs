using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using LogApp;
using System.Threading;
using System.Globalization;
using Microsoft.Win32;

namespace Secure
{
    public class LicInformation
    {
        public string ConnectionString = string.Empty;
        public string DBName = string.Empty;

        public LicInformation()
        {
        }

        public LicInformation(string connectionstring, string dbname)
        {
            this.ConnectionString = connectionstring;
            this.DBName = dbname;
        }

        //Name of the Application
        public const string ApplicationName = "Single Point Application";

        public const string ApplicationCode = "SPA0087";

        //public void GenerateTrialLicense(int TrialDays)
        //{
        //    try
        //    {
        //        DateTime requestDate = DateTime.Now;
        //        string LicenseStatus = "Apply";
        //        string ProcessorID = MachineInformtion.GetMACAddress()[0].ToString();
        //        string ModuleName = LicInformation.GetEnumDescription(LicInformation.ServerLicense.vBOARDServer).Name;
        //        string strLicenseType = LicInformation.LicenseType.Server.ToString();
        //        string ModuleCode = DBName;// LicInformation.GetEnumDescription(LicInformation.ServerLicense.TDMRecording).Code;
        //        string strQuantity = "N/A";
        //        string strLicensePeriod = LicInformation.ServerStatus.Trial.ToString();
        //        string ApplicationName = LicInformation.ApplicationName;
        //        string strAssemblyVersion = "1.0.0.0";
        //        string ApplicationCode = LicInformation.ApplicationCode;
        //        string NA1 = "N/A";
        //        string NA2 = TrialDays.ToString();         //Incase of trial, Number of days will be here
        //        string OSSerial = MachineInformtion.GetOSSerial();
        //        string strPreviousLicense = "N/A";

        //        strLicensePeriod = strLicensePeriod + " (" + NA2 + ")";

        //        string strLicenseDetail = ApplicationName + "," + ModuleName + "," + strLicenseType + ",N/A," + strLicensePeriod;

        //        string strAppliedKey = DateTime.Now.ToString() + "|" + LicenseStatus + "|" + ApplicationName + "|" + ApplicationCode + "|" + strAssemblyVersion + "|" + strLicenseType + "|" + ModuleName + "|" + ModuleCode + "|" + strPreviousLicense + "|" + OSSerial + "|" + ProcessorID + "|N/A|N/A|" + strQuantity + "|" + strLicensePeriod;
        //        strAppliedKey = LicInformation.EncryptData(strAppliedKey);

        //        string[] strLicenseInformation = LicInformation.DecryptData(strAppliedKey);
        //        DateTime applyDate = Convert.ToDateTime(strLicenseInformation[0]);
        //        strAppliedKey = UpdateTrialKey(strAppliedKey, true);

        //        //  GeneralLog.WriteLog("Saving values in XML");
        //        saveValuesInXML(ModuleName, strAppliedKey);
        //        // GeneralLog.WriteLog("Saving values in other source");
        //        // LicInformation.saveValuesInRegistry(ModuleName, strAppliedKey);
        //    }
        //    catch (Exception edx)
        //    {

        //    }
        //}

        public void GenerateTrialLicenseModule(int TrialDays, LicInformation.ServerLicense module)
        {
            try
            {
                string[] strLicenseKey = getKeyFromXML(module.ToString());
                if (strLicenseKey == null || strLicenseKey.Length == 0)
                {
                    DateTime requestDate = DateTime.Now;
                    string LicenseStatus = "Apply";
                    string ProcessorID = MachineInformtion.GetMACAddress()[0].ToString();
                    string ModuleName = LicInformation.GetEnumDescription(module).Name;
                    string strLicenseType = LicInformation.LicenseType.Server.ToString();
                    string ModuleCode = DBName;// LicInformation.GetEnumDescription(LicInformation.ServerLicense.TDMRecording).Code;
                    string strQuantity = "N/A";
                    string strLicensePeriod = LicInformation.ServerStatus.Trial.ToString();
                    string ApplicationName = LicInformation.ApplicationName;
                    string strAssemblyVersion = "1.0.0.0";
                    string ApplicationCode = LicInformation.ApplicationCode;
                    string NA1 = "N/A";
                    string NA2 = TrialDays.ToString();         //Incase of trial, Number of days will be here
                    string OSSerial = MachineInformtion.GetOSSerial();
                    string strPreviousLicense = "N/A";

                    strLicensePeriod = strLicensePeriod + " (" + NA2 + ")";

                    string strLicenseDetail = ApplicationName + "," + ModuleName + "," + strLicenseType + ",N/A," + strLicensePeriod;

                    string strAppliedKey = DateTime.Now.ToString() + "|" + LicenseStatus + "|" + ApplicationName + "|" + ApplicationCode + "|" + strAssemblyVersion + "|" + strLicenseType + "|" + ModuleName + "|" + ModuleCode + "|" + strPreviousLicense + "|" + OSSerial + "|" + ProcessorID + "|N/A|N/A|" + strQuantity + "|" + strLicensePeriod;
                    strAppliedKey = LicInformation.EncryptData(strAppliedKey);

                    string[] strLicenseInformation = LicInformation.DecryptData(strAppliedKey);
                    DateTime applyDate = Convert.ToDateTime(strLicenseInformation[0]);
                    strAppliedKey = UpdateTrialKey(strAppliedKey, true);

                    //  GeneralLog.WriteLog("Saving values in XML");
                    saveValuesInXML(ModuleName, strAppliedKey);
                    // GeneralLog.WriteLog("Saving values in other source");
                    // LicInformation.saveValuesInRegistry(ModuleName, strAppliedKey);
                }
            }
            catch (Exception edx)
            {

            }
        }

        private int TotalRisksWithServer = 999999;
        public enum ServerLicense
        {
            //[Description("Mitigate Server License,MITA001,MIT")]
            [Description("Mitigate Server License,MITA001")]
            MitigateServer,

            //[Description("DAC6 Server License,DAC6001,DAC")]
            //DAC6Server

        };

        //Name of the Client Licenses
        public enum ClientLicense
        {
            //[Description("Mitigate additional Risk bundle,MITA002,MIT")]
            //MitigateClient

            //[Description("DAC6 additional Risk bundle,DAC6002,DAC")]
            //DAC6Client

        };        

        #region XML Read & Write Functions
        //For XML Settings

        /// <summary>
        /// Get values from xml
        /// </summary>
        /// <param name="ModuleName">
        /// Module Name
        /// </param>      
        public void saveValuesInXML(string ModuleName, string LicenseKey)
        {
            try
            {
                Log4Net.WriteLog("XMLInformation for module [ " + ModuleName + " ]", LogType.GENERALLOG);
                LicKeyPath obj = new LicKeyPath(ConnectionString, DBName);
                if (LicenseKey != "")
                    obj.SetLicenseKeyInXML(ModuleName, LicenseKey);
                else
                    obj.SetLicenseKeyInXML(ModuleName);
                Log4Net.WriteLog("XMLInformation updated for [ " + ModuleName + " ]", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        /// <summary>
        /// Check license key is valid or not
        /// </summary>
        /// <param name="LicenseKey">
        /// License Key
        /// </param>      
        public bool isValidLicense(string LicenseKey)
        {
            try
            {
                bool bValidLicense = false;
                string[] LicenseData = LicenseKey.Split(',');
                Log4Net.WriteLog("License Key count is  " + LicenseData.Length, LogType.GENERALLOG);
                if (LicenseData.Length == 5)
                {
                    string ApplicationName = LicenseData[0];
                    string ModuleName = LicenseData[1];
                    string LicenseType = LicenseData[2];
                    Log4Net.WriteLog("License Key count is  " + LicenseData.Length + " [" + ApplicationName + "] [" + LicInformation.ApplicationName + "]", LogType.GENERALLOG);
                    if (ApplicationName == LicInformation.ApplicationName)
                    {
                        if (LicenseType == "Server")
                        {
                            foreach (LicInformation.ServerLicense serverLicense in LicInformation.EnumToList<LicInformation.ServerLicense>())
                            {
                                EnumDescriptionData objEnumData = LicInformation.GetEnumDescription(serverLicense);
                                if (objEnumData.Name == ModuleName)         //Key in value
                                {
                                    bValidLicense = true; break;
                                }
                            }
                        }
                        else if (LicenseType == "Client")
                        {
                            foreach (LicInformation.ClientLicense clientLicense in LicInformation.EnumToList<LicInformation.ClientLicense>())
                            {
                                EnumDescriptionData objEnumData = LicInformation.GetEnumDescription(clientLicense);
                                if (objEnumData.Name == ModuleName)         //Key in value
                                {
                                    bValidLicense = true; break;
                                }
                            }
                        }
                        else
                        {
                            Log4Net.WriteLog("License Key is invalid. ", LogType.GENERALLOG);
                            return false;
                        }

                        //string strApplyKey = DateTime.Now.ToString() + "|Apply|" + LicInformation.ApplicationName + "|" + LicInformation.ApplicationCode + "|" + strAssemblyVersion + "|" + LicenseType + "|" + ModuleName + "|" + ModuleCode + "|" + strPreviousLicense + "|" + Secure.MachineInformtion.GetOSSerial() + "|" + ProcessorID + "|N/A|N/A|" + strQuantity + "|" + strLicensePeriod;


                        string DecryptKey = LicenseKey.Split('[')[1].Split(']')[0];
                        string[] strLicenseInformation = LicInformation.DecryptData(DecryptKey);



                        if (bValidLicense && isValidLicense(strLicenseInformation, ModuleName))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        Log4Net.WriteLog("License Key count is invalid.", LogType.GENERALLOG);
                        return false;
                    }

                }
                else
                {
                    Log4Net.WriteLog("License Key count is invalid. ", LogType.GENERALLOG);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return false;
        }


        private bool isValidLicense(string[] strLicenseInformation, string ModuleName)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

                Log4Net.WriteLog("New date time culture set. " + Thread.CurrentThread.CurrentCulture.DateTimeFormat, LogType.GENERALLOG);

                try
                {
                    DateTime applyDate = Convert.ToDateTime(strLicenseInformation[0]);
                }
                catch (Exception ex)
                {
                    Log4Net.WriteLog("Failed Date Time parsed. " + strLicenseInformation[0], LogType.ERRORLOG);
                    Log4Net.WriteException(ex);
                }
                Log4Net.WriteLog("Date Time parsed", LogType.GENERALLOG);

                string LicenseStatus = strLicenseInformation[1];
                string strApplicationName = strLicenseInformation[2];
                string strApplicationCode = strLicenseInformation[3];
                string strAssemblyVersion = strLicenseInformation[4];
                string strLicenseType = strLicenseInformation[5];
                string strModuleName = strLicenseInformation[6];
                string strModuleCode = strLicenseInformation[7];
                string strPreviousLicense = strLicenseInformation[8];
                string strOSSerial = strLicenseInformation[9];
                string strMACID = strLicenseInformation[10];
                string NA1 = strLicenseInformation[11];
                string NA2 = strLicenseInformation[12];
                string strQuantity = strLicenseInformation[13];
                string strLicensePeriod = strLicenseInformation[14];
              //  LogApp.Log4Net.WriteLog("110="+LicenseStatus, LogType.GENERALLOG);
                
                if (LicenseStatus != "Apply")
                {
                    return false;
                }
                else if (strApplicationName != ApplicationName)
                {
                   // LogApp.Log4Net.WriteLog("111=" + strApplicationName + " = " + ApplicationName, LogType.GENERALLOG);
                    return false;
                }
                else if (strModuleName != ModuleName)
                {
                  //  LogApp.Log4Net.WriteLog("112=" + strModuleName + " = " + ModuleName, LogType.GENERALLOG);
                    return false;
                }
                else if (strModuleCode.Trim().ToLower() != DBName.Trim().ToLower())
                {
                  //  LogApp.Log4Net.WriteLog("112=" + strModuleCode + " = " + DBName, LogType.GENERALLOG);
                    return false;
                }
                else if (strOSSerial != MachineInformtion.GetOSSerial())
                {
                    return false;
                }
                else if (!MachineInformtion.MatchMACAddress(strMACID))
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                return false;
            }

        }


        /// <summary>
        /// Get values from xml
        /// </summary>
        /// <param name="moduleName">
        /// License Module
        /// </param>      
        public ServerStatus serverLicenseStatus(ServerLicense moduleName)
        {
            ServerStatus objStatus = ServerStatus.Expire;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

                string ModuleName = GetEnumDescription(moduleName).Name;
                string[] strLicenseInformation = getKeyFromXML(ModuleName);
             //   LogApp.Log4Net.WriteLog("1", LogType.GENERALLOG);
                if (strLicenseInformation != null)
                {
                  //  LogApp.Log4Net.WriteLog("2", LogType.GENERALLOG);
                    DateTime dtime = DateTime.Now;
                    string licenseStatus = strLicenseInformation[14];
                //    LogApp.Log4Net.WriteLog("3=" + licenseStatus, LogType.GENERALLOG);
                    if (licenseStatus.ToLower() == ServerStatus.Full.ToString().ToLower())
                    {
                        objStatus = ServerStatus.Full;
                  //      LogApp.Log4Net.WriteLog("4", LogType.GENERALLOG);
                    }
                    else if (licenseStatus.ToLower().Contains(ServerStatus.Trial.ToString().ToLower()))
                    {
                        objStatus = ServerStatus.Trial;
                        LicKeyPath obj = new LicKeyPath(ConnectionString, DBName);
                        string Key = obj.GetLicenseKeyFromXML(ModuleName);
                        Key = UpdateTrialKey(Key, false);
                        saveValuesInXML(ModuleName, Key);
                        //   LicInformation.saveValuesInRegistry(ModuleName, Key);

                        try
                        {
                            strLicenseInformation = getKeyFromXML(ModuleName);
                            DateTime dtSaved = Convert.ToDateTime(strLicenseInformation[11]);
                            DateTime dtExpire = Convert.ToDateTime(strLicenseInformation[12]);

                            if (dtSaved >= dtExpire || dtSaved > DateTime.Now)
                                objStatus = ServerStatus.Expire;
                        }
                        catch (Exception ex)
                        {
                            Log4Net.WriteException(ex);
                        }
                    }
                }

                Log4Net.WriteLog("Information updated for [ " + ModuleName + " ]", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return objStatus;
        }
      
        public void removeServerLicense(ServerLicense moduleName)
        {
            ServerStatus objStatus = ServerStatus.Expire;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

                string ModuleName = GetEnumDescription(moduleName).Name;
                string[] strLicenseInformation = getKeyFromXML(ModuleName);

                if (strLicenseInformation != null)
                {
                  //  saveValuesInRegistry(ModuleName, "");
                    saveValuesInXML(ModuleName, "");
                }

                Log4Net.WriteLog("Information updated for [ " + ModuleName + " ]", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        /// <summary>
        /// Get values from xml
        /// </summary>
        /// <param name="moduleName">
        /// License Module
        /// </param>      
        public ServerStatus serverLicenseStatus(ServerLicense moduleName, bool Update)
        {
            ServerStatus objStatus = ServerStatus.Expire;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

                string ModuleName = GetEnumDescription(moduleName).Name;
                string[] strLicenseInformation = getKeyFromXML(ModuleName);

                if (strLicenseInformation != null)
                {
                    DateTime dtime = DateTime.Now;
                    string licenseStatus = strLicenseInformation[14];
                    if (licenseStatus.ToLower() == ServerStatus.Full.ToString().ToLower())
                        objStatus = ServerStatus.Full;
                    else if (licenseStatus.ToLower().Contains(ServerStatus.Trial.ToString().ToLower()))
                    {
                        objStatus = ServerStatus.Trial;
                        LicKeyPath obj = new LicKeyPath(ConnectionString, DBName);
                        string Key = obj.GetLicenseKeyFromXML(ModuleName);
                        Key = UpdateTrialKey(Key, false);
                        saveValuesInXML(ModuleName, Key);
                       // LicInformation.saveValuesInRegistry(ModuleName, Key);

                        try
                        {
                            strLicenseInformation = getKeyFromXML(ModuleName);
                            DateTime dtSaved = Convert.ToDateTime(strLicenseInformation[11]);
                            DateTime dtExpire = Convert.ToDateTime(strLicenseInformation[12]);

                            if (dtSaved >= dtExpire || dtSaved > DateTime.Now)
                                objStatus = ServerStatus.Expire;
                        }
                        catch (Exception ex)
                        {
                            Log4Net.WriteException(ex);
                        }
                    }
                }

                Log4Net.WriteLog("Information updated for [ " + ModuleName + " ]", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return objStatus;
        }

        public string UpdateTrialKey(string strKey, bool UpdateExpiry)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

                string[] strLicenseInformation = LicInformation.DecryptData(strKey);

                if (UpdateExpiry)
                {
                    strLicenseInformation[11] = DateTime.Now.ToString();
                    int iDays = Convert.ToInt32(strLicenseInformation[14].Split('(')[1].Split(')')[0]);
                    strLicenseInformation[12] = DateTime.Now.AddDays(iDays).ToString();
                }
                else
                {
                    try
                    {
                        DateTime dtPrevious = Convert.ToDateTime(strLicenseInformation[11]);
                        if (dtPrevious < DateTime.Now)
                            strLicenseInformation[11] = DateTime.Now.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                strKey = "";
                foreach (string str in strLicenseInformation)
                {
                    strKey += str + "|";
                }
                strKey = strKey.Trim('|');
                strKey = LicInformation.EncryptData(strKey);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return strKey;
        }
        /// <summary>
        /// Get values from xml
        /// </summary>
        /// <param name="moduleName">
        /// License Module
        /// </param>      
        public int clientLicenseStatus(ClientLicense moduleName)
        {
            int iReturn = TotalRisksWithServer;
            try
            {
                EnumDescriptionData objClientEnumData = GetEnumDescription(moduleName);
                string[] strLicenseInformation = getKeyFromXML(objClientEnumData.Name);

                if (strLicenseInformation != null)
                {
                    foreach (LicInformation.ClientLicense clntLicense in LicInformation.EnumToList<LicInformation.ClientLicense>())
                    {
                        objClientEnumData = LicInformation.GetEnumDescription(clntLicense);
                        if (objClientEnumData.Code == objClientEnumData.Code)
                        {
                            //ServerStatus serverStatus = serverLicenseStatus(clntLicense);
                            //if (moduleName == ClientLicense.MitigateClient)
                            //   iReturn += Convert.ToInt32(strLicenseInformation[13]);
                            //else if (serverStatus == ServerStatus.Trial)
                            //    iReturn = 999;
                            break;
                        }
                    }
                }

                Log4Net.WriteLog("Information updated for [ " + moduleName.ToString() + " ]", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return iReturn;
        }      

        /*
        /// <summary>
        /// Get values from xml
        /// </summary>
        /// <param name="enumValue">
        /// License Module
        /// </param>      
        public static string getValuesFromXML(Enum enumValue)
        {
            string strReturn = "";
            try
            {
                string ModuleName = GetEnumDescription(enumValue);
                LicKeyPath obj = new LicKeyPath();
                strReturn = obj.GetLicenseKeyFromXML(ModuleName);
                Log4Net.WriteLog("XMLInformation updated for [ " + ModuleName + " ]", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return strReturn;
        }
        */

        /// <summary>
        /// Get values from xml
        /// </summary>
        /// <param name="ModuleName">
        /// Module Name
        /// </param>      
        public string[] getKeyFromXML(string ModuleName)
        {
            try
            {
                LicKeyPath obj = new LicKeyPath(ConnectionString, DBName);
                string Key = obj.GetLicenseKeyFromXML(ModuleName);
                Log4Net.WriteLog("XMLInformation updated for [ " + ModuleName + " ]", LogType.GENERALLOG);
               // LogApp.Log4Net.WriteLog("10", LogType.GENERALLOG);
                if (Key != "")
                {
                  //  LogApp.Log4Net.WriteLog("11", LogType.GENERALLOG);
                    string[] strKey = DecryptData(Key);
                //    LogApp.Log4Net.WriteLog("12=" + strKey.Length, LogType.GENERALLOG);
                    if (!isValidLicense(strKey, ModuleName))
                    {
                    //    LogApp.Log4Net.WriteLog("13", LogType.GENERALLOG);
                        return null;
                    }
                    if (strKey.Length <= 2)
                    {
                      //  LogApp.Log4Net.WriteLog("14", LogType.GENERALLOG);
                        return null;
                    }
                    else
                    {
                      //  LogApp.Log4Net.WriteLog("15", LogType.GENERALLOG);
                        return strKey;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return null;
        }
        #endregion

        #region Encrypt/Decrypt Data
        public static string EncryptData(string strData)
        {
            string strReturn = "";
            try
            {
                LicCryptography objCrypt = new LicCryptography();
                strReturn = objCrypt.EncryptNewOrder(strData);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return strReturn;
        }

        public static string[] DecryptData(string strData)
        {
            try
            {
                LicCryptography objCrypt = new LicCryptography();
                return objCrypt.DecryptOrder(strData);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return null;
        }
        #endregion Encrypt/Decrypt Data


        #region Encrypt/Decrypt String
        static string chipperPassword = "D@taK3yF0rD8";
        static string strToken = "[({$})]";
        public static string EncryptString(string strData)
        {
            string strReturn = "";
            try
            {
                Guid guid = Guid.NewGuid();
                strData = guid.ToString() + strToken + strData  + strToken + DateTime.Now.ToString();
                LicCryptography objCrypt = new LicCryptography();
                strReturn = objCrypt.Encrypt(strData, chipperPassword);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return strReturn;
        }

        public static string DecryptString(string strData)
        {
            string strReturn = "";
            try
            {
                string[] strArray = { strToken };
                LicCryptography objCrypt = new LicCryptography();
                strData = objCrypt.Decrypt(strData, chipperPassword);
                string[] splitString = strData.Split(strArray, StringSplitOptions.RemoveEmptyEntries);
                if (splitString.Length == 3 && splitString[0].Length == 36)
                {
                    DateTime objDate = DateTime.Parse(splitString[2]);
                    strReturn = splitString[1];
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return strReturn;
        }
        #endregion Encrypt/Decrypt String

        //Name of the Server Licenses
        public enum ServerStatus
        {
            Full,
            Trial,
            Expire
        };

        //Type of Licenses
        public enum LicenseType
        {
            Server,
            Client
        };


        #region Enum functions
        public static EnumDescriptionData GetEnumDescription(Enum value)
        {
            EnumDescriptionData objReturn = new EnumDescriptionData();
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    objReturn.Name = attributes[0].Description.Split(',')[0];
                    objReturn.Code = attributes[0].Description.Split(',')[1];
                }
                else
                {
                    objReturn.Name = value.ToString();
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return objReturn;
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            try
            {
                Type enumType = typeof(T);

                // Can't use generic type constraints on value types,
                // so have to do check like this
                if (enumType.BaseType != typeof(Enum))
                    throw new ArgumentException("T must be of type System.Enum");

                Array enumValArray = Enum.GetValues(enumType);
                List<T> enumValList = new List<T>(enumValArray.Length);

                foreach (int val in enumValArray)
                {
                    enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
                }

                return enumValList;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return null;
            //DropDownList stateDropDown = new DropDownList();
            //foreach (States state in EnumToList<States>())
            //{
            //    stateDropDown.Items.Add(GetEnumDescription(state));
            //}
        }
        #endregion Enum functions

    }

    public class EnumDescriptionData
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public EnumDescriptionData() {
            Name = "";
            Code = "";
        }

        public EnumDescriptionData(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
