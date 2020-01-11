using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IReportService _reportService;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;


        public ReportController
        (
            IUserRepository userRepository,
            IReportRepository reportRepository,
            IReportService reportService,
            IDocumentService documentService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _reportService = reportService;
            _documentService = documentService;
            _mapper = mapper;
        }


        /// <summary>
        /// Вернуть все доклады.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var reports = await _reportRepository.GetAllAsync();
            var model = _mapper.Map<IEnumerable<ReportViewModel>>( reports );
            return Ok( model );
        }


        /// <summary>
        /// Вернуть информацию по докладу.
        /// </summary>
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( Guid id )
        {
            var report = await _reportRepository.GetAsync( id );
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
        public async Task<IActionResult> GetReportsByUser( Guid userId )
        {
            if( ! await _userRepository.IsExistAsync( userId ) )
            {
                return NotFound( $"User with id='{userId}' not found" );
            }

            var reports = await _reportRepository.GetReportsByUserAsync( userId );
            var model = _mapper.Map<IEnumerable<ReportViewModel>>( reports );
            return Ok( model );
        }


        /// <summary>
        /// Приложить доклад.
        /// Прикладывается основная часть.
        /// </summary>
        [HttpPost( "attach-to/{userid}" )]
        [ModelValidation]
        public async Task<IActionResult> Attach( Guid userId, [FromBody] AttachViewModel model )
        {
            if( ! await _userRepository.IsExistAsync( userId ) )
            {
                return NotFound( $"User with id='{userId}' not found" );
            }

            try
            {
                var report = _mapper.Map<ReportModel>( model );
                var reportId = await _reportRepository.InsertAsync( report );

                var result = new JsonResult( new
                {
                    id = reportId,
                    message = $"Report with id='{reportId}' was successfully attached. File expected..."
                });
                return Ok( result );
            }
            catch( Exception e )
            {
                return BadRequest( e.Message );
            }
        }


        /// <summary>
        /// Обновить информацию по докладу (JSON).
        /// </summary>
        [HttpPut("{reportId}")]
        public async Task<IActionResult> Update( Guid reportId, [FromBody] AttachViewModel model )
        {
            if( ! await _reportRepository.IsExistAsync(reportId) )
            {
                return NotFound( $"Report with id='{reportId}' not found" );
            }

            try
            {
                var report = _mapper.Map<ReportModel>( model );
                report.Id = reportId;
                await _reportRepository.UpdateAsync( report );
                
                var result = new JsonResult(new
                {
                    id = reportId,
                    message = $"Report with id='{reportId}' was successfully updated. File expected..."
                });
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
        public async Task<IActionResult> Detach( Guid id )
        {
            if( ! await _reportRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Report with id='{id}' not found." );
            }

            await _reportRepository.DeleteAsync( id );
            return NoContent();
        }


        /// <summary>
        /// Приложить пользователя к докладу.
        /// </summary>
        [HttpPost("{reportId}/attach-user/{email}")]
        public async Task<IActionResult> AttachUser( Guid reportId, string email )
        {
            if( ! await _reportRepository.IsExistAsync( reportId ) )
            {
                return NotFound( $"Report with id='{reportId}' not found." );
            }

            var user = await _userRepository.GetByEmailAsync( email );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return NotFound( $"User with email='{email}' not found" );
            }

            if( await _reportService.ContainsUser(reportId, user.Id) )
            {
                return BadRequest( $"User with id='{user.Id}' already attached to report with id='{reportId}'" );
            }
            
            try
            {
                await _reportService.AttachUserAsync( reportId, email );
                return Ok( $"User with email='{email}' was successfully attached to report with id='{reportId}'" );
            }
            catch( Exception e )
            {
                return BadRequest( $"User with email='{email}' did not attached: {e.Message}. Try again later." );
            }
        }


        /// <summary>
        /// Открепить пользователя от доклада.
        /// </summary>
        [HttpPost("{reportId}/detach-user/{email}")]
        public async Task<IActionResult> DetachUser( Guid reportId, string email )
        {
            if( ! await _reportRepository.IsExistAsync( reportId ) )
            {
                return NotFound( $"Report with id='{reportId}' not found." );
            }
            
            var user = await _userRepository.GetByEmailAsync( email );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return NotFound( $"User with email='{email}' not found" );
            }
            
            try
            {
                await _reportService.DetachUserAsync( reportId, email );
                return Ok( $"User with email='{email}' was successfully detached from report with id='{reportId}'" );
            }
            catch( Exception e )
            {
                return BadRequest( $"User with email='{email}' did not attached: {e.Message}. Try again later." );
            }
        }


        /// <summary>
        /// Утверждение доклада.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        [HttpPut( "{id}/approve" )]
        public async Task<IActionResult> Approve( Guid id )
        {
            if( ! await _reportRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Report with id='{id}' not found" );
            }

            await _reportRepository.ChangeStatusAsync( id, to: ReportStatus.Approved );
            return Ok( $"Report with id='{id}' successfully approved." );
        }


        /// <summary>
        /// Отклонение доклада.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        [HttpPut( "{id}/reject" )]
        public async Task<IActionResult> Reject( Guid id )
        {
            if( ! await _reportRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Report with id='{id}' not found" );
            }

            await _reportRepository.ChangeStatusAsync( id, to: ReportStatus.Rejected );
            return Ok( $"Report with id='{id}' successfully rejected." );
        }


        /// <summary>
        /// Загрузка доклада на сервер.
        /// </summary>
        /// <param name="id">Id доклада.</param>
        /// <param name="file">Файл доклада</param>
        [HttpPost( "{id}/upload" )]
        public async Task<IActionResult> Upload( Guid id, [FromForm] IFormFile file )
        {
            if( file == null )
            {
                // Если файл обновлять не нужно - выходим.
                return NoContent();
            }
            
            if( ! await _reportRepository.IsExistAsync( id ) )
            {
                return BadRequest( $"Report with id='{id}' not found." );
            }

            try
            {
                await _documentService.InsertFileAsync( id, file.ConvertToFileStream() );
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
        public async Task<IActionResult> Download( Guid id )
        {
            if( ! await _reportRepository.IsExistAsync( id ) )
            {
                return BadRequest( $"Report with id='{id}' not found." );
            }

            try
            {
                var stream = await _documentService.GetFileAsync( id );
                return File( stream, "application/octet-stream" );
            }
            catch( Exception e )
            {
                return BadRequest( $"File did not download: {e.Message}. Try again later." );
            }
        }
    }
}