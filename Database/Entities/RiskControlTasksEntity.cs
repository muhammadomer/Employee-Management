using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class RiskControlTasksEntity
    {
        public string RiskControlState { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public int RiskControlID { get; set; }
        public int RiskID { get; set; }
        public string TaskOwner { get; set; }
        public string TaskStartDate { get; set; }
        public string TaskEndDate { get; set; }
        public DateTime TaskEndDateForDaysCalculation { get; set; }
        public string Overdue { get; set; }
    }
}
