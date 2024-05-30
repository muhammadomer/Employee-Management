using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class RisksWithReviewStatusAndStatesEntity
    {
        public string ReviewStatusName { get; set; }
        public int RiskID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ClassOfRisk { get; set; }
        public string OwnerName { get; set; }
        public string RiskEndDate { get; set; }
        public string RiskState { get; set; }
        public int TotalControls { get; set; }
        public int Unconfirmed { get; set; }
        public int Annotated { get; set; }
        public int Reannotated { get; set; }
        public int Revised { get; set; }
        public int Confirmed { get; set; }

    }
}
