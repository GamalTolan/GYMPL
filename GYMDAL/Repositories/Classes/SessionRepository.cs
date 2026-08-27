using GYMDAL.Data.Contexts;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository( GymDbContext context) : base(context)
        {
            _context = context;
        }
        public ICollection<Session> GetAllSessionsWithTrainerAndCategory()
        {
           return _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .ToList();
        }

        public Session GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _context.Sessions
                 .Include(s => s.Trainer)
                 .Include(s => s.Category)
                 .FirstOrDefault(s => s.Id == sessionId);
        }

        public int GetCountOfBookingSlots(int sessionId)
        {
            return _context.Bookings.Where(b => b.SessionId == sessionId).Count();
        }
    }
}
