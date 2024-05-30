using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Database.DAL;
using Database.Entities;
using Database.Models.EmployeeManagement;
using LogApp;

namespace EmployeeManagement
{
    public partial class AddEditOffice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

        [WebMethod]
        [ScriptMethod]
        public static string AddLocation(OfficeEntity location)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                LocationDAL locationDAL = new LocationDAL();
                return locationDAL.AddLocation(location);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static string EditLocation(OfficeEntity location)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                LocationDAL locationDAL = new LocationDAL();
                return locationDAL.EditLocation(location);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }

        [WebMethod]
        [ScriptMethod]
        public static OfficeEntity GetLocationByID(int locationID)
        {
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                LocationDAL locationDAL = new LocationDAL();
                return locationDAL.GetLocationInfoByID(locationID);
            }
            catch (Exception ex)
            {
                Log4Net.WriteException(ex);
                throw ex;
            }
        }
    
    }
}