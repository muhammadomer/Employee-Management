using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Responses
{
    public class UploadFilesResponse
    {
        public int Mode { get; set; }
        public string status { get; set; }
        public string errorList { get; set; }
        public string error { get; set; }
        public string fileName { get; set; }
        public int fileGroupID { get; set; }
    }
}