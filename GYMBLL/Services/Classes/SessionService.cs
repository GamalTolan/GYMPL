using AutoMapper;
using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.SessionViewModels;
using GYMDAL.Entities;
using GYMDAL.Entities.Enums;
using GYMDAL.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Classes
{
    public class SessionService(IUnitOfWork unitOfWork ,IMapper mapper) : ISessionService
    {
        public bool CreateSession(CreateSessionViewModel createSessionViewModel)
        {
            if(!IsCatigoryExists(createSessionViewModel.CategoryId))
                throw new ArgumentException($"Category with ID {createSessionViewModel.CategoryId} does not exist.");
            if(!IsTrainerExists(createSessionViewModel.TrainerId))
                throw new ArgumentException($"Trainer with ID {createSessionViewModel.TrainerId} does not exist.");
            if(!IsSessionTimeValid(createSessionViewModel.StartDate, createSessionViewModel.EndDate))
                throw new ArgumentException("Invalid session time.");

            var session = mapper.Map<CreateSessionViewModel, Session>(createSessionViewModel);
            unitOfWork.GetRepository<Session>().Add(session);
            return unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory()
                .OrderByDescending(x => x.StartDate);

            if (sessions == null || !sessions.Any())
              return  [] ;
            var mappedSessions = mapper.Map<IEnumerable<Session> , IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);
            }

            return mappedSessions;

        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
           var session = unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (session == null)
                return null;

            var mappedSession = mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);
            return mappedSession;
        }
        public bool UpdateSession(int sessionId, UpdateSessionViewModel updateSessionViewModel)
        {
            
            var session = unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (!IsSessionAvilableForUpdate(session))
                return false;
            if (!IsTrainerExists(updateSessionViewModel.TrainerId))
                return false;
            if (!IsSessionTimeValid(updateSessionViewModel.StartDate, updateSessionViewModel.EndDate))
                return false;

            session.Description = updateSessionViewModel.Description;
            session.TrainerId = updateSessionViewModel.TrainerId;
            session.StartDate =updateSessionViewModel.StartDate;
            session.EndDate = updateSessionViewModel.EndDate;
            session.UpdatedAt = DateTime.UtcNow;
            unitOfWork.GetRepository<Session>().Update(session);
            return unitOfWork.SaveChanges() > 0;
        }
        public bool RemoveSession(int sessionId)
        {
            var session = unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (!IsSessionAvilableForDelete(session))
                return false;

            unitOfWork.GetRepository<Session>().Delete(session);
            return unitOfWork.SaveChanges() > 0;
        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session == null)
               return null;

            return mapper.Map<UpdateSessionViewModel>(session);
        }
        public IEnumerable<CategorySelectViewModel> GetCategoriesDropdown()
        {
            var categories = unitOfWork.GetRepository<Category>().GetAll();
            if (categories == null || !categories.Any())
                return [];
            return mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersDropdown()
        {
           var trainers = unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers == null || !trainers.Any())
                return [];
            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
        public IEnumerable<TrainerSelectViewModel> GetTrainersByCategory(int categoryId)
        {
            if (!Enum.IsDefined(typeof(Specialities), categoryId))
                return [];
            var speciality = (Specialities)categoryId;
            var trainers = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Specialities == speciality);
            if (trainers is null)
                return [];
            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }




        #region HelperMethods

        private bool IsCatigoryExists(int categoryId)
        {
            return unitOfWork.GetRepository<Category>().GetById(categoryId) != null;
        }
        private bool IsTrainerExists(int trainerId)
        {
            return unitOfWork.GetRepository<Trainer>().GetById(trainerId) != null;
        }
        private bool IsSessionTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate > DateTime.UtcNow;
        }

        private bool IsSessionAvilableForUpdate(Session session)
        {
            if (session == null)
                return false;

            if (session.StartDate <= DateTime.UtcNow || session.EndDate < DateTime.UtcNow)
                return false;

            if (unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id) > 0)
                return false;

            return true;

        }

        private bool IsSessionAvilableForDelete(Session session)
        {
            if (session == null)
                return false;

            if (session.EndDate > DateTime.Now)
                return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            if (unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id) > 0)
                return false;

            return true;

        }



        #endregion
    }
}
