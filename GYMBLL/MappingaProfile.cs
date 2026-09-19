using AutoMapper;
using GYMBLL.ViewModels.SessionViewModels;
using GYMDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL
{
    public class MappingaProfile :Profile
    {
        public MappingaProfile()
        {
            SessionMapping();
            
        }
        private void SessionMapping()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());
            CreateMap<CreateSessionViewModel, Session>();    
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
            CreateMap<Category, CategorySelectViewModel>().ForMember(d => d.Name, opt => opt.MapFrom(s => s.CategoryName));
            CreateMap<Trainer, TrainerSelectViewModel>();
            



        }
    }
}
