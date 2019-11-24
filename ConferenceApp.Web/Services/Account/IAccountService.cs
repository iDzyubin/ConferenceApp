using ConferenceApp.Web.Models;

namespace ConferenceApp.Web.Services.Account
{
    public interface IAccountService
    {
        void SignUp( string email, string password );

        JsonWebToken SignIn( string email, string password );

        JsonWebToken RefreshAccessToken( string token );

        void RevokeRefreshToken( string token );
    }
}