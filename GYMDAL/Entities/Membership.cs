using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Entities
{
    public class Membership : BaseEntity
    {
        public DateTime EndDate { get; set; }
        
        public string Status
        {
            get
            {
                return EndDate >= DateTime.Now ? "Active" : "Expired";

            }
        }
        public int PlanId { get; set; }
        public Plan Plan { get; set; } 
        public int MemberId { get; set; }
        public Member Member { get; set; }



    }
}
