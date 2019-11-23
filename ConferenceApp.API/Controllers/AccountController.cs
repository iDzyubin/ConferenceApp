using System.Threading.Tasks;
using ConferenceApp.API.Models;
using ConferenceApp.API.Services.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = ConferenceApp.API.Services.Authorization.IAuthorizationService;

namespace ConferenceApp.API.Controllers
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


        /// <summary>
        /// Регистрация.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp( [FromBody] User user )
        {
            _accountService.SignUp( user.Username, user.Password );
            return NoContent();
        }


        /// <summary>
        /// Вход.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignIn( [FromBody] User user )
        {
            var token = _accountService.SignIn( user.Username, user.Password );
            return Ok( token );
        }


        /// <summary>
        /// Обновление токена.
        /// </summary>
        [HttpPost( "/token/{token}/refresh" )]
        [AllowAnonymous]
        public IActionResult RefreshAccessToken( string token )
        {
            var refreshToken = _accountService.RefreshAccessToken( token );
            return Ok( refreshToken );
        }


        /// <summary>
        /// Отмена обновления токена.
        /// </summary>
        [HttpPost( "/token/{token}/revoke" )]
        public IActionResult RevokeRefreshToken( string token )
        {
            _accountService.RevokeRefreshToken( token );
            return NoContent();
        }


        /// <summary>
        /// Деактивация токена.
        /// </summary>
        [HttpPost( "/token/cancel" )]
        public async Task<IActionResult> CancelAccessToken()
        {
            await _authorizationService.DeactivateCurrentAsync();
            return NoContent();
        }
    }
}