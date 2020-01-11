using System;
using System.Security.Policy;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Validators;
using ConferenceApp.Web.ViewModels;

namespace ConferenceApp.Web.Services.Account
{
    public interface IAccountService
    {
        Task<Guid> SignUpAsync( SignUpViewModel model );

        Task<TokenViewModel> SignInAsync( SignInViewModel model );

        JsonWebToken RefreshAccessToken( string token );

        void RevokeRefreshToken( string token );

        Task ConfirmAccountAsync( string code );
    }
}