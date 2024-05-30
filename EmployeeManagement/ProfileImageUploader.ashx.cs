using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace EmployeeManagement
{
    /// <summary>
    /// Summary description for ProfileImageUploader
    /// </summary>
    public class ProfileImageUploader : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable => throw new NotImplementedException();

        public void ProcessRequest(HttpContext context)
        {
            string pathToReturn = string.Empty;
            try
            {
                CsrfHandler.ValidateAntiForgery(HttpContext.Current);

                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];

                        if (file.FileName.ToLower().EndsWith(".jpg") || file.FileName.ToLower().EndsWith(".png") || file.FileName.ToLower().EndsWith(".jpeg"))
                        {

                            Image image = Image.FromStream(file.InputStream, true, true);

                            var newWidth = 200;
                            var newHeight = 200;
                            var thumbnailImg = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

                            Graphics thumbGraph = Graphics.FromImage(thumbnailImg);
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            
                            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                            thumbGraph.DrawImage(image, imageRectangle);

                            MemoryStream ms = new MemoryStream();

                            thumbnailImg.Save(ms, image.RawFormat);

                            byte[] imageBytes = ms.ToArray();

                            string base64String = Convert.ToBase64String(imageBytes);

                            thumbGraph.Dispose();
                            ms.Dispose();

                            base64String = "data:image/png;base64," + base64String;

                            HttpContext.Current.Session["UserPImage"] = base64String;

                            pathToReturn = base64String;





                            //Image oldImage = Image.FromStream(file.InputStream, true, true);

                            //string clientID = "";
                            //Users userEntity = (Users)context.Session["UserEntity"];
                            //if (userEntity.First_Name != null)
                            //{
                            //    clientID = userEntity.ID.ToString();
                            //}

                            //var folderName = context.Server.MapPath("~/upload");
                            //if (!Directory.Exists(folderName))
                            //{
                            //    Directory.CreateDirectory(folderName);
                            //}

                            //string fileName = folderName + "/" + file.FileName;
                            //string fileNameWithoutPath = string.Empty;

                            //if (File.Exists(fileName))
                            //{
                            //    string ext = Path.GetExtension(fileName);
                            //    fileName = Path.GetFileNameWithoutExtension(fileName);
                            //    fileNameWithoutPath = fileName + "-1" + ext;
                            //    fileName = folderName + "/" + fileNameWithoutPath;
                            //}
                            //else
                            //{
                            //    fileNameWithoutPath = file.FileName;
                            //}

                            //oldImage.Save(fileName, ImageFormat.Png);
                            //pathToReturn = "upload/" + fileNameWithoutPath;
                        }
                        else
                        {
                            LogApp.Log4Net.WriteLog("Image with invalid format", LogApp.LogType.ERRORLOG);
                            context.Response.ContentType = "text/plain";
                            context.Response.Write("E1");
                            return;
                        }
                        
                    }
                    LogApp.Log4Net.WriteLog("Company profileimage Updated Successfully, profileimage Path: " + pathToReturn, LogApp.LogType.ERRORLOG);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(pathToReturn);
                }
            }
            catch (Exception ex)
            {
                LogApp.Log4Net.WriteLog("Exception Occured While Trying To Upload Company Logo", LogApp.LogType.ERRORLOG);
                LogApp.Log4Net.WriteException(ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write("Exception Occured");
                throw ex;
            }
        }
    }
}