using System;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>, IChangable<UserStatus>
    {
        User GetByEmail( string email );
    }
}