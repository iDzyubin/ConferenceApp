using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using ConferenceApp.Core.Services;
using ConferenceApp.Web.Filters;
using ConferenceApp.Web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportViewModel = ConferenceApp.Web.ViewModels.ReportViewModel;

namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "/api/[controller]" )]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly ReportService _reportService;
        private readonly DocumentService _documentService;
        private readonly IMapper _mapper;


        public ReportController
        (
            IReportRepository reportRepository,
            ReportService reportService,
            DocumentService documentService,
            IMapper mapper
        )
        {
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
        [HttpGet( "{reportId}" )]
        [ServiceFilter(typeof(ReportExists))]
        public async Task<IActionResult> Get( Guid reportId )
        {
            var report = await _reportRepository.GetAsync( reportId );
            var model = _mapper.Map<ReportViewModel>( report );
            return Ok( model );
        }


        /// <summary>
        /// Вернуть доклады конкретного пользователя.
        /// </summary>
        [HttpGet( "get-reports-by-user/{userId}" )]
        [ServiceFilter(typeof(UserExists))]
        public async Task<IActionResult> GetReportsByUser( Guid userId )
        {
            var reports = await _reportRepository.GetReportsByUserAsync( userId );
            var model = _mapper.Map<IEnumerable<ReportViewModel>>( reports );
            return Ok( model );
        }


        /// <summary>
        /// Приложить доклад.
        /// Прикладывается основная часть.
        /// </summary>
        [HttpPost( "attach-to/{userId}" )]
        [ModelValidation]
        [ServiceFilter(typeof(UserExists))]
        [ExceptionFilter]
        public async Task<IActionResult> Attach( Guid userId, [FromBody] AttachViewModel model )
        {
            var report = _mapper.Map<ReportInnerModel>( model );
            var reportId = await _reportRepository.InsertAsync( report );

            var result = new
            {
                id = reportId,
                message = $"Report with id='{reportId}' was successfully attached. File expected..."
            };
            return Ok( result );
        }


        /// <summary>
        /// Обновить информацию по докладу (JSON).
        /// </summary>
        [HttpPut("{reportId}")]
        [ServiceFilter(typeof(ReportExists))]
        [ExceptionFilter]
        public async Task<IActionResult> Update( Guid reportId, [FromBody] AttachViewModel model )
        {
            var report = _mapper.Map<ReportInnerModel>( model );
            report.Id = reportId;
            await _reportRepository.UpdateAsync( report );
            
            var result = new 
            {
                id = reportId,
                message = $"Report with id='{reportId}' was successfully updated. File expected..."
            };
            return Ok( result );
        }


        /// <summary>
        /// Открепить доклад от заявки.
        /// </summary>
        [HttpGet( "{reportId}/detach" )]
        [ServiceFilter(typeof(ReportExists))]
        public async Task<IActionResult> Detach( Guid reportId )
        {
            await _reportService.DeleteAsync( reportId );
            return NoContent();
        }


        /// <summary>
        /// Приложить пользователя к докладу.
        /// </summary>
        [HttpPost("{reportId}/attach-user/{email}")]
        [ServiceFilter(typeof(ReportExists))]
        [ServiceFilter(typeof(UserExists))]
        [ExceptionFilter("User did not attached")]
        public async Task<IActionResult> AttachUser( Guid reportId, string email )
        {
            if( await _reportService.ContainsUser(reportId, email) )
            {
                return BadRequest( $"User with email='{email}' already attached to report with id='{reportId}'" );
            }
            
            await _reportService.AttachUserAsync( reportId, email );
            return Ok( $"User with email='{email}' was successfully attached to report with id='{reportId}'" );
        }


        /// <summary>
        /// Открепить пользователя от доклада.
        /// </summary>
        [HttpPost("{reportId}/detach-user/{email}")]
        [ServiceFilter(typeof(ReportExists))]
        [ServiceFilter(typeof(UserExists))]
        [ExceptionFilter("User did not detached")]
        public async Task<IActionResult> DetachUser( Guid reportId, string email )
        {
            await _reportService.DetachUserAsync( reportId, email );
            return Ok( $"User with email='{email}' was successfully detached from report with id='{reportId}'" );
        }


        /// <summary>
        /// Утверждение доклада.
        /// </summary>
        [HttpPut( "{reportId}/approve" )]
        [ServiceFilter(typeof(ReportExists))]
        public async Task<IActionResult> Approve( Guid reportId )
        {
            await _reportService.ChangeStatusAsync( reportId, to: ReportStatus.Approved );
            return Ok( $"Report with id='{reportId}' successfully approved." );
        }


        /// <summary>
        /// Отклонение доклада.
        /// </summary>
        [HttpPut( "{reportId}/reject" )]
        [ServiceFilter(typeof(ReportExists))]
        public async Task<IActionResult> Reject( Guid reportId )
        {
            await _reportService.ChangeStatusAsync( reportId, to: ReportStatus.Rejected );
            return Ok( $"Report with id='{reportId}' successfully rejected." );
        }


        /// <summary>
        /// Загрузка доклада на сервер.
        /// </summary>
        [HttpPost( "{reportId}/upload" )]
        [ServiceFilter(typeof(ReportExists))]
        [ExceptionFilter("File did not upload")]
        public async Task<IActionResult> Upload( Guid reportId, [FromForm] IFormFile file )
        {
            // Если файл обновлять не нужно - выходим.
            if( file == null ) return NoContent();
            
            await _documentService.InsertFileAsync( reportId, file.ConvertToFileStream() );
            return Ok( $"File of report with id='{reportId}' was successfully uploaded." );
        }


        /// <summary>
        /// Загрузка доклада на сторону пользователя.
        /// </summary>
        [HttpGet( "{reportId}/download" )]
        [ServiceFilter(typeof(ReportExists))]
        [ExceptionFilter("File did not download")]
        public async Task<IActionResult> Download( Guid reportId )
        {
            var (stream, fileName) = await _documentService.GetFileAsync( reportId );
            return File( stream, "application/octet-stream", fileName );
        }
    }
}