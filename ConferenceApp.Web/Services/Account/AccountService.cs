using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Web.Models;
using ConferenceApp.Web.Services.Jwt;
using Microsoft.AspNetCore.Identity;

namespace ConferenceApp.Web.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<Admin> _passwordHasher;


        public AccountService
        (
            IJwtHandler jwtHandler,
            IAdminRepository adminRepository,
            IPasswordHasher<Admin> passwordHasher,
            IMapper mapper
        )
        {
            _jwtHandler      = jwtHandler;
            _adminRepository = adminRepository;
            _passwordHasher  = passwordHasher;
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <exception cref="Exception"></exception>
        public void SignUp( string email, string password )
        {
            if( string.IsNullOrWhiteSpace( email ) )
            {
                throw new Exception( $"Username can not be empty." );
            }

            if( string.IsNullOrWhiteSpace( password ) )
            {
                throw new Exception( $"Password can not be empty." );
            }

            if( _adminRepository.GetByEmail( email.ToLower() ) != null )
            {
                throw new Exception( $"Username '{email}' is already in use." );
            }

            _adminRepository.Insert( new Admin { Login = email, Password = password } );
        }


        /// <summary>
        /// Вход.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonWebToken SignIn( string email, string password )
        {
            var user = _adminRepository.GetByEmail( email );
            if( user == null )
            {
                throw new Exception( "Invalid credentials." );
            }

            var jwt = _jwtHandler.Create( user.Login );
            var refreshToken = _passwordHasher.HashPassword( user, Guid.NewGuid().ToString() )
                .Replace( "+", string.Empty )
                .Replace( "=", string.Empty )
                .Replace( "/", string.Empty );
            jwt.RefreshToken = refreshToken;
            _refreshTokens.Add( new RefreshToken { Username = email, Token = refreshToken } );

            return jwt;
        }


        /// <summary>
        /// Обновление токена.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonWebToken RefreshAccessToken( string token )
        {
            var refreshToken = GetRefreshToken( token );
            if( refreshToken == null )
            {
                throw new Exception( "Refresh token was not found." );
            }

            if( refreshToken.Revoked )
            {
                throw new Exception( "Refresh token was revoked" );
            }

            var jwt = _jwtHandler.Create( refreshToken.Username );
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }


        /// <summary>
        /// Отмена обнолвнией.
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="Exception"></exception>
        public void RevokeRefreshToken( string token )
        {
            var refreshToken = GetRefreshToken( token );
            if( refreshToken == null )
            {
                throw new Exception( "Refresh token was not found." );
            }

            if( refreshToken.Revoked )
            {
                throw new Exception( "Refresh token was already revoked." );
            }

            refreshToken.Revoked = true;
        }

        
        private RefreshToken GetRefreshToken( string token )
            => _refreshTokens.SingleOrDefault( x => x.Token == token );
    }
}