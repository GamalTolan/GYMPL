using GYMDAL.Data.Contexts;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYMDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly GymDbContext _context;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
        }
        public void Add(TEntity entity)
        {
            _context.Add(entity);
        }

        public void Delete(TEntity entity)
        {
           _context.Set<TEntity>().Remove(entity);
            
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if(condition is null)
            {
                return _context.Set<TEntity>().AsNoTracking().ToList();
            }
            else
            {
                return _context.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            }
        }

        public TEntity GetById(int id)
        {
           return _context.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
            

        }
    }
}
