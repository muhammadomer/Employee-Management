using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections;
using LogApp;

namespace Secure
{
    public static class MachineInformtion
    {
        #region Get Processor ID
        /// <summary>
        /// Get the Processor ID of the current machine which is ued to validate license
        /// </summary>        
        /// <returns>Get Processor ID of the machine</returns>
        public static ArrayList GetProcessorID1()
        {
            ArrayList alProcessorId = new ArrayList();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    if (wmi_HD["ProcessorId"] != null)
                    {
                        Log4Net.WriteLog("ID : " + wmi_HD["ProcessorId"].ToString(), LogType.GENERALLOG);
                        alProcessorId.Add(wmi_HD["ProcessorId"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return alProcessorId;
        }

        #region Match Prossor ID
        public static bool MatchProcessorID1(string ProcessorID)
        {
            try
            {
                ArrayList objList = GetProcessorID1();

                foreach (string strPid in objList)
                {
                    //strPid = strPid.Trim();
                    if (strPid == ProcessorID)
                        return true;
                }

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return false;
        }
        #endregion Match Prossor ID
        #endregion Get Processor ID

        #region Get OS Serial Number
        /// <summary>
        /// Get the OS Serial of the current machine which is ued to validate license
        /// </summary>        
        /// <returns>OS Serial of the machine</returns>
        public static string GetOSSerial()
        {
            string strReturn = "";
            try
            {
                System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_OperatingSystem");
                System.Management.ManagementObjectCollection moc = mc.GetInstances();
                foreach (System.Management.ManagementObject mo in moc)
                {
                    strReturn = mo["SerialNumber"].ToString();
                    mo.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            Log4Net.WriteLog("Returning value is [ " + strReturn + " ]", LogType.GENERALLOG);
            return strReturn;
        }
        #endregion Get OS Serial Number

        #region Get MAC Address
        public static ArrayList GetMACAddress()
        {
            ArrayList alMACAddress = new ArrayList();
            try
            {
                //ManagementObjectSearcher objMOS = new ManagementObjectSearcher("Win32_NetworkAdapterConfiguration");
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("Select * FROM Win32_NetworkAdapterConfiguration");

                ManagementObjectCollection objMOC = objMOS.Get();
                string MACAddress = String.Empty;

                foreach (ManagementObject objMO in objMOC)
                {
                    try
                    {
                        if (objMO != null)
                        {
                            MACAddress = objMO["MacAddress"].ToString();
                            bool IPEnabled = Convert.ToBoolean(objMO["IPEnabled"]);
                            if (IPEnabled != null && IPEnabled)
                            {
                                MACAddress = MACAddress.Replace(":", "");
                                alMACAddress.Add(MACAddress);

                                Log4Net.WriteLog("ID : " + MACAddress, LogType.GENERALLOG);
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {

            }
            return alMACAddress;
        }

        public static ArrayList GetMACAddressWeb()
        {
            ArrayList alMACAddress = new ArrayList();
            try
            {
                //ManagementObjectSearcher objMOS = new ManagementObjectSearcher("Win32_NetworkAdapterConfiguration");
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("Select * FROM Win32_NetworkAdapterConfiguration");

                ManagementObjectCollection objMOC = objMOS.Get();
                string MACAddress = String.Empty;

                foreach (ManagementObject objMO in objMOC)
                {
                    try
                    {
                        if (objMO != null)
                        {
                            MACAddress = objMO["MacAddress"].ToString();
                            bool IPEnabled = Convert.ToBoolean(objMO["IPEnabled"]);
                            if (IPEnabled != null && IPEnabled)
                            {
                                MACAddress = MACAddress.Replace(":", "");
                                alMACAddress.Add(MACAddress);

                                Log4Net.WriteLog("ID : " + MACAddress, LogType.GENERALLOG);
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {

            }
            return alMACAddress;
        }

        public static bool MatchMACAddress(string MAC)
        {
            try
            {
                ArrayList objList = GetMACAddress();

                foreach (string strMAC in objList)
                {
                    //strPid = strPid.Trim();
                    if (strMAC == MAC)
                        return true;
                }

            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            return false;
        }
    
        #endregion Match MAC Address
    }
}
