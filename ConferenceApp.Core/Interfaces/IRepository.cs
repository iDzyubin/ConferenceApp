using System;
using System.Collections.Generic;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Insert( T item );

        void Delete( Guid id );

        T Get( Guid id );

        IEnumerable<T> GetAll();
    }
}