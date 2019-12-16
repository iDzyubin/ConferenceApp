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
    [Route("/api/[controller]")]
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
        [Authorize]
        public IActionResult All()
        {
            var users = _userRepository.Get(user => user.Role == UserRole.User);
            var model = _mapper.Map<IEnumerable<UserViewModel>>(users);
            return Ok(model);
        }


        /// <summary>
        /// Получить информацию о пользователе по его id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get( Guid id )
        {
            var user = _userRepository.Get(id);
            if( user == null )
            {
                return NotFound($"User with id='{id}' not found");
            }

            var model = _mapper.Map<UserViewModel>(user);
            return Ok(model);
        }


        /// <summary>
        /// Обновить информацию о пользователе.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update( Guid id, [FromBody] User user )
        {
            if( id != user.Id )
            {
                return BadRequest("Not valid id: route id and model id are not equal.");
            }

            if( _userRepository.Get(id) == null )
            {
                return NotFound($"User with id='{id}' not found");
            }

            _userRepository.Update(user);
            return Ok($"User with id='{id}' is successfully updated.");
        }


        /// <summary>
        /// Получить роль пользователя по его id.
        /// </summary>
        [HttpGet("{id}/role")]
        [Authorize]
        public IActionResult GetRole( Guid id )
        {
            var user = _userRepository.Get(id);
            if( user == null )
            {
                return NotFound($"User with id='{id}' not found");
            }

            return new JsonResult(new {id, role = user.Role});
        }


        /// <summary>
        /// Получить краткую информацию о пользователях (для списка соавторов).
        /// </summary>
        [HttpGet("user-list")]
        [Authorize]
        public IActionResult GetShortInfo()
        {
            var users = _userRepository.Get(user => user.Role == UserRole.User);
            var model = _mapper.Map<UserShortInfoViewModel>(users);
            return Ok(model);
        }
    }
}