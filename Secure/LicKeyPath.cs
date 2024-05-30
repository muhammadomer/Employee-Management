using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using LogApp;
using System.IO;
using System.Xml;
using System.Threading;
using System.Security.Permissions;

namespace Secure
{
    class LicKeyPath
    {
        private string _machine = "localhost";
        private string _vdir = "vRecord";
        private string fileName = "license.xml";

        public string ConnectionString = string.Empty;
        public string DBName = string.Empty;
        
        public LicKeyPath(string connectionstring, string dbname)
        {
            this.ConnectionString = connectionstring;
            this.DBName = dbname;
        }

        #region Get connection string according to the key provided
        private string Get_License_Path()
        {
            string strReturn = "";
            try
            {
                strReturn = OpenORCreateXML();
            }
            catch (Exception exception)
            {
                Log4Net.WriteException(exception);
            }
            return strReturn;
        }
        #endregion Get connection string according to the key provided

        #region Get License Key From XML
        public string GetLicenseKeyFromXML(string ModuleName)
        {
            string strReturn = "";
            try
            {
                Log4Net.WriteLog("Key Module Name : " + ModuleName, LogType.GENERALLOG);
                ModuleName = ModuleName.Replace(" ", "");

                string xml = Get_License_Path();
              //   LogApp.Log4Net.WriteLog("XML:" + xml, LogType.GENERALLOG);
                XmlDocument root = new XmlDocument();
                root.LoadXml(xml);

                string strSingleNode = "//Licenses/" + ModuleName + "/Key";
                //LogApp.Log4Net.WriteLog("Single Node:" + strSingleNode, LogType.GENERALLOG);
                //foreach (XmlNode node in root.ChildNodes)
                //{
                //    LogApp.Log4Net.WriteLog("name:" + node.LocalName + " name2:" + root.Name, LogType.GENERALLOG);
                //}
                XmlNode KeyValue = root.SelectSingleNode(strSingleNode);
                if (KeyValue != null)
                    strReturn = KeyValue.InnerText;
                else
                    Log4Net.WriteLog("Key value not found", LogType.GENERALLOG);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            Log4Net.WriteLog("Returning value is " + strReturn, LogType.GENERALLOG);
            return strReturn;
        }
        #endregion Get License Key From XML

        #region Set License Key From XML       
    
        public void SetLicenseKeyInXML(string ModuleName, string strLicenseKey)
        {
            try
            {
                 string[] strLicenseInformation = LicInformation.DecryptData(strLicenseKey);

                string LicenseType = strLicenseInformation[5];
                string Quantity = strLicenseInformation[13];
                string TimePeriod = strLicenseInformation[14];

                string xml = Get_License_Path();
                XmlDocument doc = new XmlDocument();
                XmlElement Innerelem;

                doc.LoadXml(xml);
                XmlNode root;              

                root = doc.SelectSingleNode("//Licenses/" + ModuleName.Replace(" ", "") + "");


                if (root == null)
                {
                    root = doc.SelectSingleNode("//Licenses");

                    doc.CreateElement(ModuleName.Replace(" ", ""));

                    //Create a new node.
                    XmlElement elem = doc.CreateElement(ModuleName.Replace(" ", ""));

                    Innerelem = doc.CreateElement("Name");
                    Innerelem.InnerText = ModuleName;
                    elem.AppendChild(Innerelem);

                    Innerelem = doc.CreateElement("LicenseType");
                    Innerelem.InnerText = LicenseType;
                    elem.AppendChild(Innerelem);

                    if (Quantity.ToLower().Equals("n/a") == false)
                    {
                        Innerelem = doc.CreateElement("Quantity");
                        Innerelem.InnerText = Quantity;
                        elem.AppendChild(Innerelem);
                    }

                    if (TimePeriod.ToLower().Equals("n/a") == false)
                    {
                        Innerelem = doc.CreateElement("TimePeriod");
                        Innerelem.InnerText = TimePeriod;
                        elem.AppendChild(Innerelem);
                    }

                    Innerelem = doc.CreateElement("Key");
                    Innerelem.InnerText = strLicenseKey;
                    elem.AppendChild(Innerelem);

                    //Add the node to the document.
                    root.AppendChild(elem);
                    Log4Net.WriteLog(ModuleName.Replace(" ", "") + " has been created sucsessfully.", LogType.GENERALLOG);

                }
                else
                {


                    root.ChildNodes[0].InnerText = ModuleName;
                    root.ChildNodes[1].InnerText = LicenseType;


                    if (Quantity.ToLower().Equals("n/a") == false)
                    {
                        root.ChildNodes[2].InnerText = Quantity;

                    }

                    if (TimePeriod.ToLower().Equals("n/a") == false)
                    {
                        root.ChildNodes[2].InnerText = TimePeriod;

                    }

                    root.ChildNodes[3].InnerText = strLicenseKey;
                    Log4Net.WriteLog(ModuleName.Replace(" ", "") + " has been created sucsessfully.", LogType.GENERALLOG);

                }

                Log4Net.WriteLog("Accuring Lock.", LogType.GENERALLOG);
                string xmltemp=doc.InnerXml ;
                xmltemp = xmltemp.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                {


                    //File.SetAttributes(xmlPath, FileAttributes.Normal);
                    //FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, xmlPath);

                    //using (FileStream fs = new FileStream(xmlPath, FileMode.Create))
                    //{
                    //    using (XmlWriter w = XmlWriter.Create(fs))
                    //    {                          
                    //        w.WriteRaw(xml);// .WriteStartElement("book");
                    //        w.Flush();
                    //    }
                    //}     
                    clsGeneralDBQeueries.SaveXML(ConnectionString, xmltemp);
                }
                Log4Net.WriteLog("XML has been saved sucsessfully.", LogType.GENERALLOG);

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        public void SetLicenseKeyInXML(string ModuleName)
        {
            try
            {
               

                string xml = Get_License_Path();             
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xml);
                XmlNode root = doc.SelectSingleNode("//Licenses");
                XmlNode childNode = root.SelectSingleNode("//" +  ModuleName.Replace(" ", "") + "");

                root.RemoveChild(childNode);
                //root.RemoveAll();
                Log4Net.WriteLog(ModuleName.Replace(" ", "") + " has been removed sucsessfully.", LogType.GENERALLOG);
                    

                Log4Net.WriteLog("Accuring Lock.", LogType.GENERALLOG);
                string xmltemp = doc.InnerXml;
                xmltemp = xmltemp.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                {
                    //File.SetAttributes(xmlPath, FileAttributes.Normal);
                    //FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, xmlPath);

                    //using (FileStream fs = new FileStream(xmlPath, FileMode.Create))
                    //{
                    //    using (XmlWriter w = XmlWriter.Create(fs))
                    //    {
                    //        w.WriteRaw(xml);
                    //        w.Flush();
                    //    }
                    //}
                    clsGeneralDBQeueries.SaveXML(ConnectionString, xmltemp);

                }
                Log4Net.WriteLog("XML has been saved sucsessfully.", LogType.GENERALLOG);

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }


        #endregion Set License Key From XML


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Open or Create XML
        /// <summary>
        /// Create OR open an XML file.
        /// </summary>
        /// <returns>XML file full path</returns>
        private string OpenORCreateXML()
        {
            string xml = "";
            bool CreateNewFile = false;
            string xmlData = "<VLicense><OSSerial><Serial>" + MachineInformtion.GetOSSerial() + "</Serial></OSSerial><Licenses></Licenses></VLicense>";

            try
            {
               // xmlPath = Get_XML_Path();
                //Log4Net.WriteLog("XML path : [ " + xmlPath + " ]", LogType.GENERALLOG);
                XmlDocument doc = new XmlDocument();
                XmlNode root;
                // xml = clsGeneralDBQeueries.GetXML("select * from ServerInfo", ConnectionString);
                xml = clsGeneralDBQeueries.GetXML("select * from DentonsEmployeesSettings", ConnectionString);
                if (!String.IsNullOrEmpty(xml))
                {
                    Log4Net.WriteLog("XML found. Loading it", LogType.GENERALLOG);
                    doc.LoadXml(xml);

                    root = doc.SelectSingleNode("//VLicense");
                    if (root != null)
                    {
                        root = doc.SelectSingleNode("//OSSerial");
                        if (root != null)
                        {
                            root = doc.SelectSingleNode("//Serial");
                            if (root != null)
                            {
                                root = doc.SelectSingleNode("//Licenses");
                                if (root == null)
                                {                                   
                                    CreateNewFile = true;
                                }
                            }
                            else
                            {
                                CreateNewFile = true;
                            }
                        }
                        else
                        {
                            CreateNewFile = true;
                        }
                    }
                    else
                    {
                        CreateNewFile = true;
                    }
                }
                else
                {
                    CreateNewFile = true;
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.ToLower().Contains("the process cannot access the file"))
                    CreateNewFile = true;
                Log4Net.WriteException(ex);
            }


            try
            {
                if (CreateNewFile)
                {
                    //if (File.Exists(xmlPath))
                    //    File.Delete(xmlPath);

                    Log4Net.WriteLog("XML file not found. Creating it", LogType.GENERALLOG);
                    // FileStream fileStream = new FileStream(xmlPath, FileMode.Create);
                    //  StreamWriter streamWriter = new StreamWriter(fileStream);
                    // streamWriter.Write(xmlData);
                    xml = xmlData;
                    clsGeneralDBQeueries.SaveXML(ConnectionString, xmlData);
                   // streamWriter.Flush();
                   // streamWriter.Close();
                    //streamWriter = null;
                    
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }

           // Log4Net.WriteLog("Returning value is [ " + xmlPath + " ]", LogType.GENERALLOG);
            return xml;
        }

        #endregion Open or Create XML

        #region Get XML file path
        //private string Get_XML_Path()
        //{
        //    string strReturn = string.Empty;
        //    try
        //    {
        //        string[] strArray = this._vdir.Split(@"\".ToCharArray());
        //        Log4Net.WriteLog("Array [ 0 ]  = " + strArray[0], LogType.GENERALLOG);
        //        string strApplicationPath = (string)this.LocateWebsite(strArray[0]).Properties["Path"].Value;
        //        Log4Net.WriteLog("Path of Virtual directory for [ " + this._vdir + " ] is [ " + strApplicationPath + " ]", LogType.GENERALLOG);
        //        if (strArray.Length > 1)
        //        {
        //            strReturn = Path.Combine(strApplicationPath, strArray[0] + @"\" + fileName);
        //        }
        //        else if (strReturn.EndsWith(@"\"))
        //        {
        //            strReturn = strApplicationPath + fileName;
        //        }
        //        else
        //        {
        //            strReturn = strApplicationPath + @"\" + fileName;
        //        }
        //        Log4Net.WriteLog("Full xml path is [ " + strReturn + " ].", LogType.GENERALLOG);
        //        return strReturn;
        //    }
        //    catch (Exception exception)
        //    {
        //        Log4Net.WriteException(exception);
        //    }
        //    Log4Net.WriteLog("Failed to get xml path.", LogType.ERRORLOG);
        //    return strReturn;
        //}
        #endregion Get XML file path

        #region Local website directory
        private DirectoryEntry LocateWebsite(string name)
        {
            DirectoryEntry deReturn = null;
            try
            {
                string iisPath = "IIS://" + this._machine + "/W3SVC/1/Root/" + name;
                deReturn = new DirectoryEntry(iisPath);
                deReturn.Username = "";
                deReturn.Password = "";
                Log4Net.WriteLog("Returning Directory entery with Virtual path [ " + iisPath + " ]", LogType.GENERALLOG);
                return deReturn;
            }
            catch (Exception exception)
            {
                Log4Net.WriteException(exception);
            }
            Log4Net.WriteLog("Failed to return Directory entery.", LogType.ERRORLOG);
            return deReturn;
        }
        #endregion Local website directory
    }

    public class CrossProcessLockFactory
    {
        private static int DefaultTimoutInMinutes = 1;
        public static IDisposable CreateCrossProcessLock(int TotalAttempts)
        {
            return new ProcessLock(TimeSpan.FromMinutes(DefaultTimoutInMinutes), TotalAttempts);
        }

        public static IDisposable CreateCrossProcessLock(TimeSpan timespan, int TotalAttempts)
        {
            return new ProcessLock(timespan,TotalAttempts);
        }
    }

    public class ProcessLock : IDisposable
    {
        // the name of the global mutex;
        private const string MutexName = "vRECORD";

        private Mutex _globalMutex;

        private bool _owned = false;


        public ProcessLock(TimeSpan timeToWait,int TotalAttempts)
        {
            try
            {
                int count = 1;
                _globalMutex = new Mutex(true, MutexName, out _owned);
                if (_owned == false && count <= TotalAttempts)
                {
                    // did not get the mutex, wait for it.
                    _owned = _globalMutex.WaitOne(timeToWait);
                    count++;
                }
            }
            catch (Exception ex)
            {
                LogApp.Log4Net.WriteException(ex);
                throw;
            }
        }

        public void Dispose()
        {
            if (_owned)
            {
                _globalMutex.ReleaseMutex();
            }
            _globalMutex = null;
        }
    }
}
