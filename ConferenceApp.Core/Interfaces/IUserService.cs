using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<(User user, bool result)> TryToSignInAsync( string email, string password );

        Task ConfirmAccountAsync( string code );
    }
}