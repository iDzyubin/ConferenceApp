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


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public UserController( IUserRepository userRepository, IMapper mapper )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Получить информацию о всех пользователях(кроме модераторов).
        /// </summary>
        [HttpGet]
        public IActionResult All()
        {
            var users = _userRepository.Get( user =>
                user.UserRole == UserRole.User
            );
            var model = _mapper.Map<IEnumerable<UserViewModel>>( users );
            return Ok( model );
        }


        /// <summary>
        /// Получить информацию о всех подтвержденных пользователях(кроме модераторов).
        /// </summary>
        [HttpGet( "confirmed" )]
        public IActionResult AllConfirmed()
        {
            var users = GetConfirmedUsers();
            var model = _mapper.Map<IEnumerable<UserViewModel>>( users );
            return Ok( model );
        }


        /// <summary>
        /// Получить информацию о пользователе по его id.
        /// </summary>
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( Guid id )
        {
            var user = await _userRepository.GetAsync( id );
            if( user == null )
            {
                return NotFound( $"User with id='{id}' not found" );
            }

            var model = _mapper.Map<UserViewModel>( user );
            return Ok( model );
        }


        /// <summary>
        /// Проверка существует ли пользователь.
        /// </summary>
        /// <param name="email">Email пользователя</param>
        [HttpGet("{email}/is-exists")]
        public async Task<IActionResult> IsExists( string email )
        {
            var user = await _userRepository.GetByEmailAsync( email );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return NotFound( $"User with email='{email}' not found" );
            }
            return Ok( $"User with email='{email}' is exists" );
        }


        /// <summary>
        /// Обновить информацию о пользователе.
        /// </summary>
        [HttpPut( "{id}" )]
        [ModelValidation]
        public async Task<IActionResult> Update( Guid id, [FromBody] UpdateUserViewModel model )
        {
            if( id != model.Id )
            {
                return BadRequest( "Not valid id: route id and model id are not equal." );
            }

            if( ! await _userRepository.IsExistAsync( id ) )
            {
                return NotFound( $"User with id='{id}' not found" );
            }

            var updatedUser = _mapper.Map<User>( model );
            await _userRepository.UpdateAsync( updatedUser );
            return Ok( $"User with id='{id}' is successfully updated." );
        }


        /// <summary>
        /// Получить роль пользователя по его id.
        /// </summary>
        [HttpGet( "{id}/role" )]
        public async Task<IActionResult> GetRole( Guid id )
        {
            var user = await _userRepository.GetAsync( id );
            if( user == null )
            {
                return NotFound( $"User with id='{id}' not found" );
            }

            return new JsonResult( new { id, role = user.UserRole } );
        }


        /// <summary>
        /// Получить краткую информацию о пользователях (для списка соавторов).
        /// </summary>
        [HttpGet( "user-list" )]
        public IActionResult GetShortInfo()
        {
            var users = GetConfirmedUsers();
            var model = _mapper.Map<IEnumerable<UserShortInfoViewModel>>( users );
            return Ok( model );
        }
        
        
        [NonAction]
        private IEnumerable<User> GetConfirmedUsers() 
            => _userRepository.Get( user =>
                user.UserRole == UserRole.User &&
                user.UserStatus == UserStatus.Confirmed );
    }
}