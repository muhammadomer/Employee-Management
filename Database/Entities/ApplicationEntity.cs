using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class ApplicationEntity
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string ModalName { get; set; }
        public string ApplicationURL { get; set; }
        public string Icon { get; set; }
        public string LicenseText { get; set; }
        public int LicenseCount { get; set; }
    }
}
