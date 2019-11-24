using System;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using ConferenceApp.Web.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IDocumentService _documentService;


        public ReportController
        (
            IRequestRepository requestRepository,
            IReportRepository reportRepository,
            IDocumentService documentService
        )
        {
            _requestRepository = requestRepository;
            _reportRepository = reportRepository;
            _documentService = documentService;
        }

        
        /// <summary>
        /// Вернуть файл по id файла.
        /// </summary>
        [HttpGet( "{reportId}" )]
        [Authorize]
        public IActionResult Get( Guid reportId )
        {
            var report = _reportRepository.Get( reportId );
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
        /// Выдать доклады по заявке.
        /// </summary>
        [HttpGet( "/api/report/get-reports-by-request/{requestId}" )]
        [Authorize]
        public IActionResult GetByRequest( Guid requestId )
        {
            var request = _requestRepository.Get( requestId );
            if( request == null )
            {
                return NotFound( $"Request with id='{requestId}' not found" );
            }
            
            var reports = _reportRepository.GetReportsByRequest( requestId );
            var model = reports.Select(report => report.ConvertToReportViewModel());
            return Ok( model );
        }


        /// <summary>
        /// Утверждение доклада.
        /// </summary>
        [HttpGet( "/api/report/{reportId}/approve" )]
        [Authorize]
        public IActionResult Approve( Guid reportId )
        {
            _reportRepository.ChangeStatus( reportId, ReportStatus.Approved );
            return Ok( $"Report with id='{reportId}' successfully approved" );
        }


        /// <summary>
        /// Отклонение доклада.
        /// </summary>
        [HttpGet( "/api/report/{reportId}/reject" )]
        [Authorize]
        public IActionResult Reject( Guid reportId )
        {
            _reportRepository.ChangeStatus( reportId, ReportStatus.Rejected );
            return Ok( $"Report with id='{reportId}' successfully rejected" );
        }


        /// <summary>
        /// Загрузка файла.
        /// </summary>
        [HttpGet( "/api/report/{reportId}/download" )]
        public IActionResult Download( Guid reportId )
        {
            var report = _reportRepository.Get( reportId );
            if( report == null )
            {
                return BadRequest( $"Report with id='{reportId}' not found" );
            }

            var (stream, status) = _documentService.GetFile( report.RequestId, report.ReportId );
            if( status != FileStatus.Success )
            {
                return BadRequest( $"File did not download: {status}. Try again later." );
            }

            return File( stream, "application/octet-stream" );
        }
    }
}