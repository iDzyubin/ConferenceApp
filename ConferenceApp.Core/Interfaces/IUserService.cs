namespace ConferenceApp.Core.Interfaces
{
    public interface IUserService
    {
        bool TryToSignIn( string email, string password );
    }
}