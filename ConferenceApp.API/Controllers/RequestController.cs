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
        public IActionResult Create( [FromBody] RequestViewModel model )
        {
            var requestModel = new RequestModel { User = model.User, Reports = model.Reports };
            _requestRepositoryAdapter.Insert( requestModel );
            return Ok();
        }


        /// <summary>
        /// Получить список всех заявок.
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var requests = _requestRepositoryAdapter.GetAll();
            return Ok( requests );
        }


        /// <summary>
        /// Получить заявку по id.
        /// </summary>
        [HttpGet( "/api/request/{requestId}" )]
        public IActionResult Get( Guid requestId )
        {
            var request = _requestRepositoryAdapter.Get( requestId );
            if( request == null )
            {
                return NotFound( $"Request with id='{requestId}' not found" );
            }

            return Ok( request );
        }


        /// <summary>
        /// Утверждение заявки.
        /// </summary>
        [HttpGet( "/api/request/{requestId}/approve" )]
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
        [HttpGet( "/api/request/{requestId}/reject" )]
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