using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Web.Filters;
using ConferenceApp.Web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователями.
    /// </summary>
    [ApiController]
    [Route( "/api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UserController( IUserRepository userRepository, IMapper mapper )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Получить информацию о всех подтвержденных пользователях.
        /// </summary>
        [HttpGet( "confirmed" )]
        public async Task<IActionResult> AllConfirmed()
        {
            var users = await _userRepository.GetAllAsync();
            var model = _mapper.Map<IEnumerable<UserViewModel>>( users );
            return Ok( model );
        }


        /// <summary>
        /// Получить информацию о пользователе по его id.
        /// </summary>
        [HttpGet( "{userId}" )]
        public async Task<IActionResult> Get( Guid userId )
        {
            var user = await _userRepository.GetAsync( userId );
            var model = _mapper.Map<UserViewModel>( user );
            return Ok( model );
        }


        /// <summary>
        /// Проверка существует ли пользователь.
        /// </summary>
        [HttpGet("{email}/is-exists")]
        public async Task<IActionResult> IsExists( string email )
        {
            if( !await _userRepository.IsExistAsync( email ) )
            {
                return NotFound( $"User with email='{email}' not found" );
            }
            return Ok( $"User with email='{email}' is exists" );
        }


        /// <summary>
        /// Обновить информацию о пользователе.
        /// </summary>
        [HttpPut( "{userId}" )]
        [ModelValidation]
        [ServiceFilter(typeof(UserExists))]
        public async Task<IActionResult> Update( Guid userId, [FromBody] UpdateUserViewModel model )
        {
            var updatedUser = _mapper.Map<User>( model );
            await _userRepository.UpdateAsync( updatedUser );
            return Ok( $"User with id='{userId}' is successfully updated." );
        }
    }
}