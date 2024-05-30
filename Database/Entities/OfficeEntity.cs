using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class OfficeEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string Office { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string GPSPostal { get; set; }
        public string MailPostal { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string Telephone { get; set; }
    }
}
