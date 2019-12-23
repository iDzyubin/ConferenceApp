using System;
using System.Collections.Generic;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Guid Insert( T item );

        void Update( T item );

        void Delete( Guid id );

        T Get( Guid id );

        IEnumerable<T> Get( Func<T, bool> filter );

        IEnumerable<T> GetAll();

        bool IsExist( Guid id );
    }
}