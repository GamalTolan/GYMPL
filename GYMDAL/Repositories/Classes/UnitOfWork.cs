using GYMDAL.Data.Contexts;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork(GymDbContext context)
        {
            _context = context;
           
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
