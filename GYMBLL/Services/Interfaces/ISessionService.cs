using GYMBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Interfaces
{
    public interface ISessionService
    {
        bool CreateSession(CreateSessionViewModel createSessionViewModel);
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int sessionId);
        bool UpdateSession(int sessionId, UpdateSessionViewModel updateSessionViewModel);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool RemoveSession(int sessionId);
        IEnumerable<CategorySelectViewModel> GetCategoriesDropdown();
        IEnumerable<TrainerSelectViewModel> GetTrainersDropdown();
        IEnumerable<TrainerSelectViewModel> GetTrainersByCategory(int categoryId);
    }
}
