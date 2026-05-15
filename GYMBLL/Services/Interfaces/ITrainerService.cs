using GYMBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel);
        bool UpdateTrainer(int id, UpdateTrainerViewModel updateTrainerModel);
        UpdateTrainerViewModel GetTrainerToUpdate(int id);
        TrainerViewModel GetTrainerDetails(int id)  ;
        bool RemoveTrainer(int id);
    }
}
