using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserService
    {
        (UserModel user, bool isSuccess) TryToSignIn( string email, string password );
    }
}