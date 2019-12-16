using System;
using System.Threading.Tasks;
using ConferenceApp.Web.Services.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = ConferenceApp.Web.Services.Authorization.IAuthorizationService;
using User = ConferenceApp.Web.Models.User;

namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthorizationService _authorizationService;


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public AccountController
        (
            IAccountService accountService,
            IAuthorizationService authorizationService
        )
        {
            _accountService = accountService;
            _authorizationService = authorizationService;
        }


        // TODO.
        /// <summary>
        /// Регистрация.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp( [FromBody] User user )
        {
            try
            {
                _accountService.SignUp( user.Username, user.Password );
                return NoContent();
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        // TODO.
        /// <summary>
        /// Вход.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignIn( [FromBody] User user )
        {
            try
            {
                var token = _accountService.SignIn( user.Username, user.Password );
                return Ok( token );
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        /// <summary>
        /// Обновление токена.
        /// </summary>
        [HttpPost( "/token/{token}/refresh" )]
        [AllowAnonymous]
        public IActionResult RefreshAccessToken( string token )
        {
            try
            {
                var refreshToken = _accountService.RefreshAccessToken( token );
                return Ok( refreshToken );
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        /// <summary>
        /// Отмена обновления токена.
        /// </summary>
        [HttpPost( "/token/{token}/revoke" )]
        public IActionResult RevokeRefreshToken( string token )
        {
            try
            {
                _accountService.RevokeRefreshToken( token );
                return NoContent();
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        /// <summary>
        /// Деактивация токена.
        /// </summary>
        [HttpPost( "/token/cancel" )]
        public async Task<IActionResult> CancelAccessToken()
        {
            try
            {
                await _authorizationService.DeactivateCurrentAsync();
                return NoContent();
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }
    }
}