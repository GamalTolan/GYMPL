using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate   { get; set; }
        public ICollection<Booking> SessionMembers { get; set; } = null!;

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!; 

    }
}
