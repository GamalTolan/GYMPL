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
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Session> GetAllSessions(int memberId)
        {
            _context.Members.Find(memberId);
            return _context.Sessions.ToList();
        }
    }
}
