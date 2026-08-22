using GYMDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;  
        ISessionRepository SessionRepository { get; set; }
        int SaveChanges();
    }
}
