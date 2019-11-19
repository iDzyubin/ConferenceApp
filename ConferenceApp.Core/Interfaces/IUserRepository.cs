using System;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Guid InsertWithId( User user );
    }
}