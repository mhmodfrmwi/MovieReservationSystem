using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.Interfaces
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IGenericRepository<TEntity>Repository<TEntity>() where TEntity : class;
        Task<int>CompleteAsync();
    }
}
