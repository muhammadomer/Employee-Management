using System;
using System.Web;
using System.Web.UI;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using LogApp;
using System.Configuration;

namespace EmployeeManagement
{
    public class CsrfHandler
    {

        public static void Validate(Page page, HiddenField forgeryToken)
        {
            if(page.AppRelativeVirtualPath.Contains("WarningPage.aspx"))
            {
                return;
            }


            string CompareDomain = "";
            try
            {
                if (ConfigurationManager.AppSettings["DomainName"] != null && ConfigurationManager.AppSettings["DomainName"] != "" && ConfigurationManager.AppSettings["onPrem"] == "0")
                {
                    CompareDomain = ConfigurationManager.AppSettings["DomainName"];
                }
                //Log4Net.WriteLog("CompareDomain: " + CompareDomain, LogType.GENERALLOG);
            }
            catch { }

            if(CompareDomain != "" && page.Request.UrlReferrer != null && (!page.Request.Url.Authority.Contains(CompareDomain) || !page.Request.UrlReferrer.Authority.Contains(CompareDomain)))
            {
                //Log4Net.WriteLog("CompareDomain: " + page.Request.UrlReferrer.Authority + " | " + page.Request.Url.Authority, LogType.GENERALLOG);
                page.Response.Redirect("WarningPage.aspx");
            }
            else if (CompareDomain == "" && page.Request.UrlReferrer != null && !page.Request.Url.Authority.ToString().Equals(page.Request.UrlReferrer.Authority.ToString()))
            {
                //Log4Net.WriteLog("1: " + page.Request.UrlReferrer.Authority + " | " + page.Request.Url.Authority, LogType.GENERALLOG);
                page.Response.Redirect("WarningPage.aspx");
            }
            else if (!page.IsPostBack || page.AppRelativeVirtualPath.Contains("Licensing.aspx"))
            {
                //Log4Net.WriteLog("Licensing", LogType.GENERALLOG);
                Guid antiforgeryToken = Guid.NewGuid();
                page.Session["AntiforgeryToken"] = antiforgeryToken;
                forgeryToken.Value = antiforgeryToken.ToString();
            }
            else
            {
                //Log4Net.WriteLog("Validate ELSE", LogType.GENERALLOG);
                Guid stored = (Guid)page.Session["AntiforgeryToken"];
                Guid sent = new Guid(forgeryToken.Value);
                if (sent != stored)
                {
                    // you can throw an exception, in my case I'm just logging the user out
                    page.Response.Redirect("WarningPage.aspx");
                }
            }
        }

        public static void ValidateAntiForgery(HttpContext context)
        {
            var antiForgeryCookie = context.Request.Cookies[AntiForgeryConfig.CookieName];

            var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;

            AntiForgery.Validate(cookieValue, context.Request.Headers["__RequestVerificationToken"]);

            if (!context.Request.Url.Authority.ToString().Equals(context.Request.UrlReferrer.Authority.ToString()))
            {
                cookieValue += cookieValue;
                AntiForgery.Validate(cookieValue, context.Request.Headers["__RequestVerificationToken"]);
            }
            else if (!context.Request.Url.Scheme.ToString().Equals(context.Request.UrlReferrer.Scheme.ToString()))
            {
                cookieValue += cookieValue;
                AntiForgery.Validate(cookieValue, context.Request.Headers["__RequestVerificationToken"]);
            }
        }
    }
}