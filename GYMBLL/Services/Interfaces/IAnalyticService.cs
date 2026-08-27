using GYMBLL.ViewModels.AnalyticViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Interfaces
{
    public interface IAnalyticService 
    {
        AnalyticViewModel GetAnalytics();
    }
}
