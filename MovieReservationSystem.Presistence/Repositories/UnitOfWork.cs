using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Presistence.Data;
using System.Collections.Concurrent;

namespace MovieReservationSystem.Presistence.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<Type, object> _repositories = new();
        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<TEntity>(context);
                _repositories.TryAdd(type, repositoryInstance);
            }
            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
