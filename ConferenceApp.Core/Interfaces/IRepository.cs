using System;
using System.Collections.Generic;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Insert( T item );

        void Delete( Guid id );

        void Update( T item );

        T Get( Guid id );

        IEnumerable<T> Get( Func<T, bool> filter );

        IEnumerable<T> GetAll();
    }
}