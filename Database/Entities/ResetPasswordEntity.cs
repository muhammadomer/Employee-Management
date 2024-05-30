using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class ResetPasswordEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool LinkExpired { get; set; }

        public string UserFeedback { get; set; }
    }
}
