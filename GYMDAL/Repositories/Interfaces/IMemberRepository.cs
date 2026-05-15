using GYMDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Repositories.Interfaces
{
    public interface IMemberRepository :IGenericRepository<Member>
    {
        IEnumerable<Session> GetAllSessions(int memberId);
    }
}
