using System;
using ConferenceApp.API.Filters;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestService _requestService;


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public RequestController
        (
            IRequestRepository requestRepository,
            IRequestService requestService
        )
        {
            _requestRepository = requestRepository;
            _requestService = requestService;
        }

        // TODO.
        /// <summary>
        /// Добавление заявки.
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public IActionResult Insert( [FromBody] Request request )
        {
            _requestRepository.Insert( request );
            return Ok();
        }

        
        /// <summary>
        /// Утверждение заявки.
        /// </summary>
        [HttpGet( "{requestId}" )]
        [Authorize]
        public IActionResult Approve( Guid requestId )
        {
            var status = _requestService.Approve( requestId );
            if( status != RequestStatus.Approved )
            {
                return BadRequest( "Status does not changed. Try again later." );
            }

            return Ok( $"Request with id='{requestId}' successfully approved" );
        }

        
        /// <summary>
        /// Отмена заявки.
        /// </summary>
        [HttpGet( "{requestId}" )]
        [Authorize]
        public IActionResult Reject( Guid requestId )
        {
            var status = _requestService.Reject( requestId );
            if( status != RequestStatus.Rejected )
            {
                return BadRequest( "Status does not changed. Try again later." );
            }

            return Ok( $"Request with id='{requestId}' successfully rejected" );
        }
    }
}