using System;
using ConferenceApp.API.Extensions;
using ConferenceApp.API.Filters;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepositoryAdapter _reportRepositoryAdapter;
        private readonly IReportService _reportService;
        private readonly IDocumentService _documentService;


        public ReportController
        (
            IReportRepositoryAdapter reportRepositoryAdapter,
            IReportService reportService,
            IDocumentService documentService
        )
        {
            _reportRepositoryAdapter = reportRepositoryAdapter;
            _reportService = reportService;
            _documentService = documentService;
        }


        /// <summary>
        /// Добавление файла к заявке.
        /// </summary>
        [HttpPost( "{requestId}" )]
        [ModelValidation]
        public IActionResult Insert( Guid requestId, [FromForm] ReportViewModel model )
        {
            model.RequestId = requestId;
            _reportRepositoryAdapter.Insert( model );
            return Ok();
        }


        /// <summary>
        /// Удаление файла от заявки.
        /// </summary>
        [HttpDelete( "{reportId}" )]
        public IActionResult Delete( Guid reportId )
        {
            var report = _reportRepositoryAdapter.Get( reportId );
            if( report == null )
            {
                return NotFound( $"Report with id='{reportId} not found'" );
            }

            _reportRepositoryAdapter.Delete( reportId );
            return NoContent();
        }


        /// <summary>
        /// Обновление файла.
        /// </summary>
        [HttpPut( "{reportId}" )]
        [ModelValidation]
        public IActionResult Update( Guid reportId, [FromBody] ReportViewModel model )
        {
            var report = _reportRepositoryAdapter.Get( reportId );
            if( report == null )
            {
                return NotFound( $"Report with id='{reportId}' not found" );
            }

            _reportRepositoryAdapter.Update( model );
            return NoContent();
        }


        /// <summary>
        /// Вернуть файл по id файла.
        /// </summary>
        [HttpGet( "{reportId}" )]
        public IActionResult Get( Guid reportId )
        {
            var report = _reportRepositoryAdapter.Get( reportId );
            if( report == null )
            {
                return NotFound( $"Report with id='{reportId}' not found" );
            }

            return new JsonResult( new
                {
                    id            = report.ReportId,
                    title         = report.Title,
                    reportType    = report.ReportType.GetDisplayName(),
                    status        = report.ReportStatus.GetDisplayName(),
                    collaborators = report.Collaborators
                }
            );
        }
        
        
        /// <summary>
        /// Утверждение заявки.
        /// </summary>
        [HttpGet( "/api/report/{reportId}/approve" )]
        [Authorize]
        public IActionResult Approve( Guid reportId )
        {
            var status = _reportService.Approve( reportId );
            if( status != ReportStatus.Approved )
            {
                return BadRequest( "Status does not changed. Try again later." );
            }

            return Ok( $"Report with id='{reportId}' successfully approved" );
        }


        /// <summary>
        /// Отмена заявки.
        /// </summary>
        [HttpGet( "/api/report/{reportId}/reject" )]
        [Authorize]
        public IActionResult Reject( Guid reportId )
        {
            var status = _reportService.Reject( reportId );
            if( status != ReportStatus.Approved )
            {
                return BadRequest( "Status does not changed. Try again later." );
            }

            return Ok( $"Report with id='{reportId}' successfully rejected" );
        }


        /// <summary>
        /// Загрузка файла.
        /// </summary>
        [HttpGet( "/api/report/{reportId}/download" )]
        public IActionResult Download( Guid reportId )
        {
            var report = _reportRepositoryAdapter.Get( reportId );
            if( report == null )
            {
                return BadRequest( $"Report with id='{reportId}' not found" );
            }

            var (stream, status) = _documentService.GetFile( report.RequestId, report.ReportId );
            if( status != FileStatus.Success )
            {
                return BadRequest( $"File does not download: {status}. Try again later." );
            }

            return File( stream, "application/octet-stream" );
        }
    }
}