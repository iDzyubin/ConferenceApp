using System;
using System.Collections.Generic;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            var users = _userRepository.Get( user =>
                user.UserRole == UserRole.User &&
                user.UserStatus != UserStatus.Confirmed
            );
            var model = _mapper.Map<IEnumerable<UserViewModel>>( users );
            return Ok( model );
        }


        /// <summary>
        /// Получить информацию о пользователе по его id.
        /// </summary>
        [HttpGet( "{id}" )]
        public IActionResult Get( Guid id )
        {
            var user = _userRepository.Get( id );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return NotFound( $"User with id='{id}' not found" );
            }

            var model = _mapper.Map<UserViewModel>( user );
            return Ok( model );
        }


        /// <summary>
        /// Обновить информацию о пользователе.
        /// </summary>
        [HttpPut( "{id}" )]
        public IActionResult Update( Guid id, [FromBody] UserViewModel model )
        {
            if( id != model.Id )
            {
                return BadRequest( "Not valid id: route id and model id are not equal." );
            }

            var user = _userRepository.Get( id );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return NotFound( $"User with id='{id}' not found" );
            }

            var updatedUser = _mapper.Map<User>( model );
            _userRepository.Update( updatedUser );
            return Ok( $"User with id='{id}' is successfully updated." );
        }


        /// <summary>
        /// Получить роль пользователя по его id.
        /// </summary>
        [HttpGet( "{id}/role" )]
        public IActionResult GetRole( Guid id )
        {
            var user = _userRepository.Get( id );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
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
            var users = _userRepository.Get( user =>
                user.UserRole == UserRole.User &&
                user.UserStatus != UserStatus.Confirmed
            );
            var model = _mapper.Map<IEnumerable<UserShortInfoViewModel>>( users );
            return Ok( model );
        }
    }
}