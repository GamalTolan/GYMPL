using GYMDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Repositories.Interfaces
{
    public interface ISessionRepository :IGenericRepository<Session>
    {
        ICollection<Session> GetAllSessionsWithTrainerAndCategory();   
        Session GetSessionWithTrainerAndCategory(int sessionId);   
        int GetCountOfBookingSlots(int sessionId);
    }
}
