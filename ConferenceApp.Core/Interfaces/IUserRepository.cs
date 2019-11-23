using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Guid InsertWithId( User user );

        IEnumerable<User> Get( Func<User, bool> filter );
    }
}