using System;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.Models;

namespace ConferenceApp.Web.Services.Account
{
    public interface IAccountService
    {
        Guid SignUp( UserModel model );

        JsonWebToken SignIn( string email, string password );

        JsonWebToken RefreshAccessToken( string token );

        void RevokeRefreshToken( string token );
    }
}