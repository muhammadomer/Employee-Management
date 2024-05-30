using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class SettingEntity
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string CityName { get; set; }
        public string DepartmentName { get; set; }
        public int RegionID { get; set; }
        public int CountryID { get; set; }
    }
}
