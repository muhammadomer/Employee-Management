using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class RiskControlsWithTaskEntity
    {
        public int RiskControlID { get; set; }
        public int RiskID { get; set; }
        public string Name { get; set; }
        public string ControlOwner { get; set; }
        public string ReviewDate { get; set; }
        public string RiskControlState { get; set; }
        public int TotalTasks { get; set; }
        public int Overdue { get; set; }
        public int Started { get; set; }
        public int Finished { get; set; }
    }
}
