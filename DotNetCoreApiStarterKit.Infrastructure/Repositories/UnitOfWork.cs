﻿using DotNetCoreApiStarterKit.Domain.Interfaces.Repositories;
using DotNetCoreApiStarterKit.Infrastructure.DbContext;

namespace DotNetCoreApiStarterKit.Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        private readonly Dictionary<Type, object> _repositories = new();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_dbContext);
                _repositories[type] = repo;
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
