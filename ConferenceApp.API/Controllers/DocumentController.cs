using System;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    public class DocumentController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IDocumentService _documentService;

        public DocumentController
        (
            IReportRepository reportRepository,
            IDocumentService documentService
        )
        {
            _reportRepository = reportRepository;
            _documentService = documentService;
        }


        [HttpGet( "{reportId}" )]
        public IActionResult Download( Guid reportId )
        {
            var report = _reportRepository.Get( reportId );
            if( report == null )
            {
                return BadRequest( $"Report with id='{reportId}' not found" );
            }

            var (stream, status) = _documentService.GetFile( report.RequestId, report.Id );
            if( status != FileStatus.Success )
            {
                return BadRequest( $"File does not download: {status}. Try again later." );
            }

            return File( stream, "application/octet-stream" );
        }
    }
}