using System.Threading.Tasks;
using ConferenceApp.Web.Filters;
using ConferenceApp.Web.Services.Account;
using ConferenceApp.Web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    [AllowAnonymous]
    [ExceptionFilter]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public AccountController( IAccountService accountService )
        {
            _accountService = accountService;
        }


        /// <summary>
        /// Регистрация.
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public async Task<IActionResult> SignUp( [FromBody] SignUpViewModel model )
        {
            var userId = await _accountService.SignUpAsync( model );
            var result = new
            {
                id = userId, 
                message = $"User with id='{userId}' was successfully registered.",
                notify = "На указанный e-mail было отправлено письмо. " +
                         "Для завершения регистрации перейдите по ссылке в письме."
            };
            return Ok( result );
        }

        
        /// <summary>
        /// Подтвердить аккаунт.
        /// </summary>
        [HttpGet("{code}")]
        public async Task<IActionResult> Confirm( string code )
        {
            await _accountService.ConfirmAccountAsync( code );
            return Ok( "Регистрация завершена. Теперь Вы можете войти под своим аккаунтом" );
        }


        /// <summary>
        /// Вход.
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public IActionResult SignIn( [FromBody] SignInViewModel model )
        {
            var token = _accountService.SignInAsync( model );
            return Ok( token );
        }


        /// <summary>
        /// Обновление токена.
        /// </summary>
        [HttpPost( "/token/{token}/refresh" )]
        [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
        public IActionResult RefreshAccessToken( string token )
        {
            var refreshToken = _accountService.RefreshAccessToken( token );
            return Ok( refreshToken );
        }


        /// <summary>
        /// Деактивация рефреш токена.
        /// </summary>
        [HttpPost( "/token/{token}/revoke" )]
        [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
        public IActionResult RevokeRefreshToken( string token )
        {
            _accountService.RevokeRefreshToken( token );
            return NoContent();
        }
    }
}