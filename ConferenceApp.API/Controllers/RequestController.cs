using System;
using ConferenceApp.API.Filters;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.API.Models;
using ConferenceApp.Core.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepositoryAdapter _requestRepositoryAdapter;
        private readonly IRequestService _requestService;


        /// <summary>
        /// Basic ctor.
        /// </summary>
        public RequestController
        (
            IRequestRepositoryAdapter requestRepositoryAdapter,
            IRequestService requestService
        )
        {
            _requestRepositoryAdapter = requestRepositoryAdapter;
            _requestService = requestService;
        }


        /// <summary>
        /// Добавление заявки.
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public IActionResult Insert( [FromBody] RequestViewModel model )
        {
            _requestRepositoryAdapter.Insert( model );
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