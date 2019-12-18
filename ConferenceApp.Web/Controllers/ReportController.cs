using System;
using System.Collections.Generic;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using ConferenceApp.Web.Filters;
using ConferenceApp.Web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "/api/[controller]" )]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;


        public ReportController
        (
            IUserRepository userRepository,
            IReportRepository reportRepository,
            IDocumentService documentService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _documentService = documentService;
            _mapper = mapper;
        }


        /// <summary>
        /// Вернуть все доклады.
        /// </summary>
        [HttpGet]
        public IActionResult All()
        {
            var reports = _reportRepository.GetAll();
            var model = _mapper.Map<IEnumerable<ReportViewModel>>( reports );
            return Ok( model );
        }


        /// <summary>
        /// Вернуть информацию по докладу.
        /// </summary>
        [HttpGet( "{id}" )]
        public IActionResult Get( Guid id )
        {
            var report = _reportRepository.Get( id );
            if( report == null )
            {
                return NotFound( $"Report with id='{id}' not found" );
            }

            var model = _mapper.Map<ReportViewModel>( report );
            return Ok( model );
        }


        /// <summary>
        /// Вернуть доклады конкретного пользователя.
        /// </summary>
        [HttpGet( "get-reports-by-user/{userid}" )]
        public IActionResult GetReportsByUser( Guid userId )
        {
            if( !_userRepository.IsExist( userId ) )
            {
                return NotFound( $"User with id='{userId}' not found" );
            }

            var reports = _reportRepository.GetReportsByUser( userId );
            var model = _mapper.Map<IEnumerable<ReportViewModel>>( reports );
            return Ok( model );
        }


        /// <summary>
        /// Приложить доклад.
        /// Прикладывается основная часть.
        /// </summary>
        [HttpPost( "attach-to/{userid}" )]
        [ModelValidation]
        public IActionResult Attach( Guid userId, [FromBody] AttachViewModel model )
        {
            if( !_userRepository.IsExist( userId ) )
            {
                return NotFound( $"User with id='{userId}' not found" );
            }

            try
            {
                var report = _mapper.Map<ReportModel>( model );
                var reportId = _reportRepository.Insert( report );

                var result = new JsonResult( new
                    {
                        id = reportId,
                        message = $"Report with id='{reportId}' was successfully attached. File expected..."
                    }
                );
                return Ok( result );
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        /// <summary>
        /// Открепить доклад от заявки.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        [HttpGet( "{id}/detach" )]
        public IActionResult Detach( Guid id )
        {
            if( !_reportRepository.IsExist( id ) )
            {
                return NotFound( $"Report with id='{id}' not found." );
            }

            _reportRepository.Delete( id );
            return NoContent();
        }


        /// <summary>
        /// Утверждение доклада.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        [HttpGet( "{id}/approve" )]
        public IActionResult Approve( Guid id )
        {
            if( !_reportRepository.IsExist( id ) )
            {
                return NotFound( $"Report with id='{id}' not found" );
            }

            _reportRepository.ChangeStatus( id, to: ReportStatus.Approved );
            return Ok( $"Report with id='{id}' successfully approved." );
        }


        /// <summary>
        /// Отклонение доклада.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        [HttpGet( "{id}/reject" )]
        public IActionResult Reject( Guid id )
        {
            if( !_reportRepository.IsExist( id ) )
            {
                return NotFound( $"Report with id='{id}' not found" );
            }

            _reportRepository.ChangeStatus( id, to: ReportStatus.Rejected );
            return Ok( $"Report with id='{id}' successfully rejected." );
        }


        /// <summary>
        /// Загрузка доклада на сервер.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        /// <param name="file">Файл доклада</param>
        [HttpPost( "{id}/upload" )]
        public IActionResult Upload( Guid id, [FromForm] IFormFile file )
        {
            if( !_reportRepository.IsExist( id ) )
            {
                return BadRequest( $"Report with id='{id}' not found." );
            }

            try
            {
                _documentService.InsertFile( id, file.ConvertToFileStream() );
                return Ok( $"File of report with id='{id}' was successfully uploaded." );
            }
            catch( Exception e )
            {
                return BadRequest( $"File did not upload: {e.Message}. Try to upload file later." );
            }
        }


        /// <summary>
        /// Загрузка доклада на сторону пользователя.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        [HttpGet( "{id}/download" )]
        public IActionResult Download( Guid id )
        {
            if( !_reportRepository.IsExist( id ) )
            {
                return BadRequest( $"Report with id='{id}' not found." );
            }

            try
            {
                var stream = _documentService.GetFile( id );
                return File( stream, "application/octet-stream" );
            }
            catch( Exception e )
            {
                return BadRequest( $"File did not download: {e.Message}. Try again later." );
            }
        }
    }
}