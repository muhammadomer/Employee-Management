using LogApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Database.Utility
{
    public class LoggedinUserDetail
    {
        public LoggedinUserDetail()
        {
            try
            {
                Log4Net.WriteLog("LoggedInUserDetail: ", LogType.GENERALLOG);
                var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                Log4Net.WriteLog("identity: " + identity.Name, LogType.GENERALLOG);
                try
                {
                    _userId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    _fullName = identity.FindFirst("FullName").Value;
                    _profileImage = identity.FindFirst("ProfileImage").Value;
                    _applicationURL = identity.FindFirst("ApplicationURL").Value;
                    _LevelPermissionID = Convert.ToInt32(identity.FindFirst("LevelPermissionID").Value);
                    _supportEmails = identity.FindFirst("SupportEmails").Value;
                    _userEmail = identity.FindFirst("UserEmail").Value;
                }
                catch (Exception ex)
                {
                    Log4Net.WriteException(ex);
                }


            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
            }
        }

        private int _LevelPermissionID;

        public int LevelPermissionID
        {
            get { return _LevelPermissionID; }
            private set { _LevelPermissionID = value; }
        }

        private int _userId;

        public int UserId
        {
            get { return _userId; }
            private set { _userId = value; }
        }
        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            private set { _fullName = value; }
        }
        private string _profileImage;

        public string ProfileImage
        {
            get { return _profileImage; }
            private set { _profileImage = value; }
        }
        private string _applicationURL;

        public string ApplicationURL
        {
            get { return _applicationURL; }
            private set { _applicationURL = value; }
        }
        private string _supportEmails;
        public string SupportEmails
        {
            get { return _supportEmails; }
            private set { _supportEmails = value; }
        }
        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            private set { _userEmail = value; }
        }
    }
}

