using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Web.Models;

namespace ConferenceApp.Web.Services.Account
{
    public interface IAccountService
    {
        Guid SignUp( User user );

        JsonWebToken SignIn( string email, string password );

        JsonWebToken RefreshAccessToken( string token );

        void RevokeRefreshToken( string token );
    }
}