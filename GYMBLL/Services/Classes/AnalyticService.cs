using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.AnalyticViewModels;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Classes
{
    public class AnalyticService : IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticViewModel GetAnalytics()
        {
            return new AnalyticViewModel
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(x=>x.Status == "Active").Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                OngoingSessions = _unitOfWork.GetRepository<Session>().GetAll(x=>x.StartDate <= DateOnly.FromDateTime(DateTime.Now) && x.EndDate >= DateOnly.FromDateTime(DateTime.Now)).Count(),
                CompletedSessions = _unitOfWork.GetRepository<Session>().GetAll(x=>x.EndDate < DateOnly.FromDateTime(DateTime.Now)).Count(),
                UpcomingSessions = _unitOfWork.GetRepository<Session>().GetAll(x => x.StartDate > DateOnly.FromDateTime(DateTime.Now)).Count()
            };
        }
    }
}
