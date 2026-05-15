using GYMDAL.Data.Contexts;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Repositories.Classes
{
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        private readonly GymDbContext _context;
        public TrainerRepository(GymDbContext context) : base(context) 
        {
            _context = context;
        }

       
    }
}
