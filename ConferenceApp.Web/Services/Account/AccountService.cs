using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Services.Jwt;
using ConferenceApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ConferenceApp.Web.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<SignInViewModel> _passwordHasher;
        private readonly PasswordHashService _passwordHashService;
        private readonly NotificationService _notificationService;


        public AccountService
        (
            IMapper mapper,
            UserService userService,
            IUserRepository userRepository,
            IJwtHandler jwtHandler,
            IPasswordHasher<SignInViewModel> passwordHasher,
            PasswordHashService passwordHashService,
            NotificationService notificationService
        )
        {
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            _passwordHashService = passwordHashService;
            _notificationService = notificationService;
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        public async Task<Guid> SignUpAsync( SignUpViewModel model )
        {
            if( await _userRepository.GetByEmailAsync(model.Email) != null )
            {
                throw new Exception($"Username '{model.Email}' is already in use.");
            }

            try
            {
                var user = _mapper.Map<User>( model );
                user.Password = _passwordHashService.GetHash(user.Password);
                user.ConfirmCode = Guid.NewGuid().ToString( "N" );
                
                var userId = await _userRepository.InsertAsync(user);
                
                var confirmUrl = $"/api/account/confirm/{user.ConfirmCode}";
                await _notificationService.SendAccountConfirmationAsync( user.Email, confirmUrl );
                
                return userId;
            }
            catch( Exception e )
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Подтверждение аккаунта.
        /// </summary>
        public async Task ConfirmAccountAsync( string code )
        {
            await _userService.ConfirmAccountAsync( code );
        }


        /// <summary>
        /// Вход.
        /// </summary>
        public TokenViewModel SignInAsync( SignInViewModel model )
        {
            var (user, isSuccess) = _userService.TryToSignInAsync(model.Email, model.Password);
            if( !isSuccess )
            {
                throw new Exception("Invalid credentials.");
            }

            var jwt = _jwtHandler.Create(model.Email);
            var refreshToken = _passwordHasher.HashPassword(model, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);
            jwt.RefreshToken = refreshToken;
            _refreshTokens.Add(new RefreshToken {Username = model.Email, Token = refreshToken});

            var tokenViewModel = new TokenViewModel
            {
                JsonWebToken = jwt,
                UserId       = user.Id,
                Role         = user.UserRole,
                FullName = ( user.LastName ) +
                           ( user.FirstName  == String.Empty ? String.Empty : $" {user.FirstName}" ) +
                           ( user.MiddleName == String.Empty ? String.Empty : $" {user.MiddleName}" )
            };
            return tokenViewModel;
        }


        /// <summary>
        /// Обновление токена.
        /// </summary>
        public JsonWebToken RefreshAccessToken( string token )
        {
            var refreshToken = GetRefreshToken(token);
            
            if( refreshToken == null ) throw new Exception("Refresh token was not found.");
            if( refreshToken.Revoked ) throw new Exception("Refresh token was revoked");

            var jwt = _jwtHandler.Create(refreshToken.Username);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }


        /// <summary>
        /// Отмена обновлений.
        /// </summary>
        public void RevokeRefreshToken( string token )
        {
            var refreshToken = GetRefreshToken(token);
            
            if( refreshToken == null ) throw new Exception("Refresh token was not found.");
            if( refreshToken.Revoked ) throw new Exception("Refresh token was already revoked.");

            refreshToken.Revoked = true;
        }


        private RefreshToken GetRefreshToken( string token )
            => _refreshTokens.SingleOrDefault(x => x.Token == token);
    }
}