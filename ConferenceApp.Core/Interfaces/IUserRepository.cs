using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync( string email );

        Task<bool> IsExistAsync( string email );
    }
}