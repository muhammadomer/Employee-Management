using Database.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace EmployeeManagement
{
    /// <summary>
    /// Summary description for ImageUploadHandler
    /// </summary>
    public class ImageUploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string pathToReturn = string.Empty;
            try
            {
                var currentUser = GeneralUtilities.GetCurrentUser();
                LogApp.Log4Net.WriteLog("Start Uploading Image: ", LogApp.LogType.GENERALLOG);
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];

                        LogApp.Log4Net.WriteLog("Check image extension: ", LogApp.LogType.GENERALLOG);
                        if (file.FileName.ToLower().EndsWith(".jpg") || file.FileName.ToLower().EndsWith(".png") || file.FileName.ToLower().EndsWith(".jpeg"))
                        {
                            Image image = Image.FromStream(file.InputStream, true, true);

                            LogApp.Log4Net.WriteLog("Set Image height and width: ", LogApp.LogType.GENERALLOG);

                            var newWidth = image.Width;
                            var newHeight = image.Height;

                            if (newWidth > 300)
                            {
                                for (int div = 100; div > 0; div--)
                                {
                                    newWidth = image.Width * div / 100;
                                    if (newWidth <= 300)
                                    {
                                        newHeight = image.Height * div / 100;
                                        break;
                                    }
                                }
                            }

                            var newWidth1 = newWidth;
                            var newHeight1 = newHeight;

                            if (newHeight1 > 100)
                            {
                                for (int div = 100; div > 0; div--)
                                {
                                    newHeight1 = newHeight * div / 100;
                                    if (newHeight1 <= 100)
                                    {
                                        newWidth1 = newWidth * div / 100;
                                        break;
                                    }
                                }
                            }

                            var thumbnailImg = new Bitmap(newWidth1, newHeight1, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

                            Graphics thumbGraph = Graphics.FromImage(thumbnailImg);
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            var imageRectangle = new Rectangle(0, 0, newWidth1, newHeight1);
                            thumbGraph.DrawImage(image, imageRectangle);

                            MemoryStream ms = new MemoryStream();

                            thumbnailImg.Save(ms, image.RawFormat);

                            byte[] imageBytes = ms.ToArray();

                            LogApp.Log4Net.WriteLog("Image Converted in base64: ", LogApp.LogType.GENERALLOG);
                            string base64String = Convert.ToBase64String(imageBytes);

                            thumbGraph.Dispose();
                            ms.Dispose();
                            var folderName = context.Server.MapPath("~/upload/Images/Logo");
                            string dbId = "";
                            if (!String.IsNullOrWhiteSpace(Convert.ToString(context.Session["UserAccountID"])))
                            {
                                dbId = Convert.ToString(context.Session["UserAccountID"]);
                            }
                            if (!String.IsNullOrWhiteSpace(dbId))
                            {
                                folderName = context.Server.MapPath("~/upload/Images/"+ dbId + "/"+currentUser.ID);
                            }
                            else
                            {
                                folderName = context.Server.MapPath("~/upload/Images/0/" + currentUser.ID);
                            }
                            if (!Directory.Exists(folderName))
                            {
                                Directory.CreateDirectory(folderName);
                            }

                            DirectoryInfo dirInfo = new DirectoryInfo(folderName);
                            foreach (FileInfo fileInfo in dirInfo.GetFiles())
                            {
                                fileInfo.Delete();
                            }

                            byte[] bytes = Convert.FromBase64String(base64String);
                            File.WriteAllBytes(Path.Combine(folderName, "logo.png"), bytes);

                            LogApp.Log4Net.WriteLog("Image uploaded in folder: " + folderName, LogApp.LogType.GENERALLOG);
                            pathToReturn = "true";
                        }
                        else
                        {
                            pathToReturn = "false";
                            LogApp.Log4Net.WriteLog("Image with invalid format", LogApp.LogType.ERRORLOG);
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(pathToReturn);
                            return;
                        }
                    }
                    LogApp.Log4Net.WriteLog("Company Logo Updated Successfully, Logo Path: " + pathToReturn, LogApp.LogType.ERRORLOG);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(pathToReturn);
                }
            }
            catch (Exception ex)
            {
                pathToReturn = "Exception Occured";
                LogApp.Log4Net.WriteLog("Exception Occured While Trying To Upload Company Logo", LogApp.LogType.ERRORLOG);
                LogApp.Log4Net.WriteException(ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write(pathToReturn);
                throw ex;
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