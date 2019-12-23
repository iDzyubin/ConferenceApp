using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Services.Jwt;
using ConferenceApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ConferenceApp.Web.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<SignInViewModel> _passwordHasher;


        public AccountService
        (
            IMapper mapper,
            IUserService userService,
            IUserRepository userRepository,
            IJwtHandler jwtHandler,
            IPasswordHasher<SignInViewModel> passwordHasher
        )
        {
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        public Guid SignUp( SignUpViewModel model )
        {
            if( _userRepository.GetByEmail(model.Email) != null )
            {
                throw new Exception($"Username '{model.Email}' is already in use.");
            }

            try
            {
                var user = _mapper.Map<User>( model );
                var userId = _userRepository.Insert(user);
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
        public void ConfirmAccount( string code )
        {
            _userService.ConfirmAccount( code );
        }


        /// <summary>
        /// Вход.
        /// </summary>
        public TokenViewModel SignIn( SignInViewModel model )
        {
            var (user, isSuccess) = _userService.TryToSignIn(model.Email, model.Password);
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
            if( refreshToken == null )
            {
                throw new Exception("Refresh token was not found.");
            }

            if( refreshToken.Revoked )
            {
                throw new Exception("Refresh token was revoked");
            }

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
            if( refreshToken == null )
            {
                throw new Exception("Refresh token was not found.");
            }

            if( refreshToken.Revoked )
            {
                throw new Exception("Refresh token was already revoked.");
            }

            refreshToken.Revoked = true;
        }


        private RefreshToken GetRefreshToken( string token )
            => _refreshTokens.SingleOrDefault(x => x.Token == token);
    }
}