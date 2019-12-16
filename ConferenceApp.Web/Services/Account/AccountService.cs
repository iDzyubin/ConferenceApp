using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Services.Jwt;
using Microsoft.AspNetCore.Identity;

namespace ConferenceApp.Web.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<UserModel> _passwordHasher;


        public AccountService
        (
            IUserService userService,
            IUserRepository userRepository,
            IJwtHandler jwtHandler,
            IPasswordHasher<UserModel> passwordHasher
        )
        {
            _userService = userService;
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        public Guid SignUp( UserModel model )
        {
            if( _userRepository.GetByEmail(model.Email) != null )
            {
                throw new Exception($"Username '{model.Email}' is already in use.");
            }

            try
            {
                var userId = _userRepository.Insert(model);
                return userId;
            }
            catch( Exception e )
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Вход.
        /// </summary>
        public JsonWebToken SignIn( string email, string password )
        {
            var (user, isSuccess) = _userService.TryToSignIn(email, password);
            if( !isSuccess )
            {
                throw new Exception("Invalid credentials.");
            }

            var jwt = _jwtHandler.Create(user.Email);
            var refreshToken = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);
            jwt.RefreshToken = refreshToken;
            _refreshTokens.Add(new RefreshToken {Username = email, Token = refreshToken});

            return jwt;
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