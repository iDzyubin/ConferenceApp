using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Guid> InsertAsync( T item );

        Task UpdateAsync( T item );

        Task DeleteAsync( Guid id );

        Task<T> GetAsync( Guid id );

        List<T> Get( Func<T, bool> filter );

        Task<List<T>> GetAllAsync();

        Task<bool> IsExistAsync( Guid id );
    }
}