using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MemberViewModels;
using GYMBLL.ViewModels.TrainerViewModels;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            if (IsEmailExists(createTrainerViewModel.Email) || IsPhoneExists(createTrainerViewModel.PhoneNumber))
                return false;

            var trainerEntity = new Trainer
            {
                Name = createTrainerViewModel.Name,
                CreatedAt = createTrainerViewModel.HireDate,
                Email = createTrainerViewModel.Email,
                PhoneNumber = createTrainerViewModel.PhoneNumber,
                DateOfBirth = createTrainerViewModel.DateOfBirth,
                Gender = createTrainerViewModel.Gender,
                Address = new Address
                {
                    BuildingNumber = createTrainerViewModel.BuildingNumber,
                    Street = createTrainerViewModel.Street,
                    City = createTrainerViewModel.City
                },
                Specialities = createTrainerViewModel.Specialization


            };
            _unitOfWork.GetRepository<Trainer>().Add(trainerEntity);
            _unitOfWork.SaveChanges();
            return true;

        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll() ?? [];
            var trainerViewModels = trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Specialities = t.Specialities.ToString(),
            }).ToList();
            return trainerViewModels;
        }
        public TrainerViewModel GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null)
            {
                return null;
            }
            var trainerDetails = new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                Address = AddressFormat(trainer.Address),
                Gender = trainer.Gender.ToString(),
                Specialities = trainer.Specialities.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToString("yyyy-MM-dd"),
            };
            return trainerDetails;
        }
        public bool RemoveTrainer(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null || HasActiveSessions(id)) 
                return false;
            var validSession = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == id&& s.EndDate > DateOnly.FromDateTime(DateTime.UtcNow));
            if (validSession is not null && validSession.Any())
            {
                return false;
            }
            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            _unitOfWork.SaveChanges();
            return true;
        }
        public UpdateTrainerViewModel GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null)
            {
                return null;
            }
            var updateTrainerModel = new UpdateTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                Specialization = trainer.Specialities

            };
            return updateTrainerModel;
        }

        public bool UpdateTrainer(int id, UpdateTrainerViewModel updateTrainerModel)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null)
            {
                return false;
            }
           var existingTrainerWithEmail = _unitOfWork.GetRepository<Trainer>()
                .GetAll(t => t.Email == updateTrainerModel.Email && t.Id != id);
           var existingTrainerWithPhone = _unitOfWork.GetRepository<Trainer>()
                .GetAll(t => t.PhoneNumber == updateTrainerModel.PhoneNumber && t.Id != id);
            if (existingTrainerWithEmail.Any() || existingTrainerWithPhone.Any())
                return false;

            trainer.Name = updateTrainerModel.Name;
            trainer.Email = updateTrainerModel.Email;
            trainer.PhoneNumber = updateTrainerModel.PhoneNumber;
            trainer.Address.BuildingNumber = updateTrainerModel.BuildingNumber;
            trainer.Address.Street = updateTrainerModel.Street;
            trainer.Address.City = updateTrainerModel.City;
            trainer.Specialities = updateTrainerModel.Specialization;
            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            _unitOfWork.SaveChanges();
            return true;
        }

        #region HelperMethods

        private bool IsEmailExists(string email)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.Email == email).Any();
            return existing;
        }

        private bool IsPhoneExists(string phone)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.PhoneNumber == phone).Any();
            return existing;
        }
        private string AddressFormat(Address address)
        {
            if (address == null)
                return "N/A";
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }

        private bool HasActiveSessions(int Id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(
               s => s.TrainerId == Id && s.StartDate > DateOnly.FromDateTime(DateTime.Now)).Any();
            return activeSessions;
        }


        #endregion

    }
}
