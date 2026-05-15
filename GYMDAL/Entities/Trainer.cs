using GYMDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialities Specialities { get; set; }

        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
