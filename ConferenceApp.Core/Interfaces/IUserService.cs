using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IUserService
    {
        (User user, bool result) TryToSignIn( string email, string password );
    }
}