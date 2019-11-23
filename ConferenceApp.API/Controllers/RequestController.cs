using System;
using System.Linq;
using ConferenceApp.API.Extensions;
using ConferenceApp.API.Filters;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public RequestController( IRequestRepository requestRepository )
        {
            _requestRepository = requestRepository;
        }


        /// <summary>
        /// Добавление заявки.
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public IActionResult Create( [FromBody] RequestViewModel model )
        {
            var request = model.ConvertToRequestModel();
            _requestRepository.Insert( request );
            return Ok();
        }


        /// <summary>
        /// Получить список всех заявок.
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var requests = _requestRepository.GetAll();
            var model = requests.Select(request => request.ConvertToRequestViewModel());
            return Ok( model );
        }


        /// <summary>
        /// Получить заявку по id.
        /// </summary>
        [HttpGet( "/api/request/{requestId}" )]
        public IActionResult Get( Guid requestId )
        {
            var request = _requestRepository.Get( requestId );
            if( request == null )
            {
                return NotFound( $"Request with id='{requestId}' not found" );
            }

            var model = request.ConvertToRequestViewModel();
            return Ok( model );
        }


        /// <summary>
        /// Утверждение заявки.
        /// </summary>
        [HttpGet( "/api/request/{requestId}/approve" )]
        [Authorize]
        public IActionResult Approve( Guid requestId )
        {
            _requestRepository.ChangeStatus( requestId, RequestStatus.Approved );
            return Ok( $"Request with id='{requestId}' successfully approved" );
        }


        /// <summary>
        /// Отмена заявки.
        /// </summary>
        [HttpGet( "/api/request/{requestId}/reject" )]
        [Authorize]
        public IActionResult Reject( Guid requestId )
        {
            _requestRepository.ChangeStatus( requestId, RequestStatus.Rejected );
            return Ok( $"Request with id='{requestId}' successfully rejected" );
        }
    }
}