using GYMDAL.Data.Contexts;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;

namespace GYMDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
       
        private readonly Dictionary<string, object> _repositories = [];

        public ISessionRepository SessionRepository { get ; set ; }
        public UnitOfWork(GymDbContext context , ISessionRepository sessionRepository)
        {
            _context = context;
            SessionRepository = sessionRepository;
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var entityName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(entityName, out object? value))
                return (IGenericRepository<TEntity>)value;

            var repository = new GenericRepository<TEntity>(_context);
            _repositories.Add(entityName, repository);
            return repository;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
