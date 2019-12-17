using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>, IChangable<UserStatus>
    {
        User GetByEmail( string email );
        
        void Update( User user );
    }
}