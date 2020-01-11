using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>, IChangable<UserStatus>
    {
        Task<User> GetByEmailAsync( string email );

        Task Confirm( Guid userId );
    }
}