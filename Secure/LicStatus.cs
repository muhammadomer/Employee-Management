using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogApp;

namespace Secure
{
    public class LicStatus
    {
        #region Check whether license is valid or not
        /// <summary>
        /// If return value is true then it means server license is valid
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool IsValidServerLicense(string Module)
        {
            bool bReturn = false;
            try
            {
                Log4Net.WriteLog("Getting license for the server [ " + Module + " ].", LogType.GENERALLOG);
                //bReturn = Globalsettings.IsValidServerLicense(Module);
                Log4Net.WriteLog("Returning value is [ " + bReturn.ToString() + " ].", LogType.GENERALLOG);
                return bReturn;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            Log4Net.WriteLog("Failed to return Server status.", LogType.ERRORLOG);
            return bReturn;
        }
        #endregion Check whether license is valid or not

        #region Check number of licenses
        /// <summary>
        /// If 'N' is returned then it means server is on trial. Infinte license of client exist.
        /// If '0' is returned then it means server license is full but have no client license
        /// Any other number will show number of existing licenses
        /// </summary>
        /// <param name="Module"></param>
        /// <returns></returns>
        public string TotalLicense(string Module)
        {
            string strReturn = "";
            try
            {
                Log4Net.WriteLog("Getting license for the client [ " + Module + " ] without key", LogType.GENERALLOG);
                //strReturn = Globalsettings.TotalLicense(Module);
                Log4Net.WriteLog("Returning value is [ " + strReturn + " ].", LogType.GENERALLOG);
                return strReturn;
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
            Log4Net.WriteLog("Failed to return Client status.", LogType.ERRORLOG);
            return strReturn;
        }
        #endregion Check number of licenses

    }
}

