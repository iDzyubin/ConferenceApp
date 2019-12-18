using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Validators;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Services.Account
{
    public interface IAccountService
    {
        Guid SignUp( SignUpViewModel model );

        TokenViewModel SignIn( SignInViewModel model );

        JsonWebToken RefreshAccessToken( string token );

        void RevokeRefreshToken( string token );
    }
}