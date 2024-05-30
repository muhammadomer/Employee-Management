using EmployeeManagement.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace EmployeeManagement
{
    /// <summary>
    /// Summary description for ImportEmployeeFile
    /// </summary>
    public class ImportEmployeeFile : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            // Checking no of files injected in Request object  
            if (HttpContext.Current.Request.Files.Count > 0)
            {

                try
                {
                    var splitArr = HttpContext.Current.Request.Files[0].FileName.Split('.');
                    if (splitArr[splitArr.Length - 1].ToLower() != "xlsx")
                    {
                        // WriteEvent("Please upload a .xlsx file.", true);
                        context.Response.Write(JsonConvert.SerializeObject(new UploadFilesResponse() { Mode = 2, status = "Please Upload a .xlsx file.", errorList = "", error = "Please Upload a .xlsx file." }));
                    }

                    int fgID = 0;
                    if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                    {
                        try
                        {
                            fgID = Convert.ToInt32(HttpContext.Current.Request.Form[0]);
                        }
                        catch
                        {
                            fgID = 0;
                        }

                    }

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = new HttpFileCollectionWrapper(HttpContext.Current.Request.Files);
                    string fname = null;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];

                        // Checking for Internet Explorer  
                        if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE" || HttpContext.Current.Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.


                        string Path = HostingEnvironment.MapPath("~/upload/");
                        bool exists = Directory.Exists(Path);

                        if (!exists)
                            Directory.CreateDirectory(Path);

                        fname = Path + fname;

                        if (System.IO.File.Exists(fname))
                        {
                            System.IO.File.Delete(fname);
                        }

                        file.SaveAs(fname);
                    }
                    context.Response.Write(JsonConvert.SerializeObject(new UploadFilesResponse() { Mode = 5, fileName = fname, fileGroupID = fgID }));
                }
                catch (Exception ex)
                {
                    //WriteEvent("Error occurred.Error details: " + ex.Message, true);
                    context.Response.Write(JsonConvert.SerializeObject(new UploadFilesResponse() { Mode = 2, status = "", errorList = "", error = "Error occurred. Error details: " + ex.Message }));
                }
            }
            else
            {
                //WriteEvent("Please upload a .xlsx file", true);
                context.Response.Write(JsonConvert.SerializeObject(new UploadFilesResponse() { Mode = 2, status = "Please upload a .xlsx file.", errorList = "", error = "No files selected." }));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}