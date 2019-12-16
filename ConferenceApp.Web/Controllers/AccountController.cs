using System;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.Filters;
using ConferenceApp.Web.Services.Account;
using ConferenceApp.Web.ViewModels;
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
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IAuthorizationService _authorizationService;


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public AccountController
        (
            IMapper mapper,
            IAccountService accountService,
            IAuthorizationService authorizationService
        )
        {
            _mapper = mapper;
            _accountService = accountService;
            _authorizationService = authorizationService;
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        [HttpPost]
        [ModelValidation]
        [AllowAnonymous]
        public IActionResult SignUp( [FromBody] UserViewModel model )
        {
            try
            {
                var user = _mapper.Map<UserModel>(model);
                var userId = _accountService.SignUp( user );
                var result = new JsonResult(new
                {
                    id = userId,
                    message = $"User with id='{userId}' was successfully registered."
                });
                return Ok(result);
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


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