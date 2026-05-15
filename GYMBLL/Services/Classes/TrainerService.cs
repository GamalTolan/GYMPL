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
            if (IsEmailUnique(createTrainerViewModel.Email))
            {
                return false;
            }
            if (IsPhoneNumberUnique(createTrainerViewModel.PhoneNumber))
            {
                return false;
            }

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
                Specialities = trainer.Specialities.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToString("yyyy-MM-dd"),
            };
            return trainerDetails;
        }
        public bool RemoveTrainer(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer == null) 
                return false;
            var validSession = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == id&& s.EndDate > DateOnly.FromDateTime(DateTime.UtcNow));
            if (validSession != null && validSession.Any())
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
                Name = trainer.Name,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
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
            if (IsEmailUnique(updateTrainerModel.Email))
                return false;
            if (IsPhoneNumberUnique(updateTrainerModel.PhoneNumber))
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

        private bool IsEmailUnique(string email)
        {
            var existingTrainer = _unitOfWork.GetRepository<Trainer>().GetAll(t=>t.Email==email);

            return existingTrainer is not null && existingTrainer.Any(); ;
        }
        

        private bool IsPhoneNumberUnique(string phoneNumber)
        {
            var existingTrainer = _unitOfWork.GetRepository<Trainer>().GetAll(t => t.PhoneNumber == phoneNumber);

            return   existingTrainer is not null && existingTrainer.Any(); ;
        }
        private string AddressFormat(Address address)
        {
            if (address == null)
                return "N/A";
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }

        


        #endregion

    }
}
