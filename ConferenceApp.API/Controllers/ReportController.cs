using System;
using System.IO;
using AutoMapper;
using ConferenceApp.API.Extensions;
using ConferenceApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ConferenceApp.API.Models;
using dto = ConferenceApp.Core.DataModels;

namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class ReportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        private readonly IRequestRepository _requestRepository;


        public ReportController
        (
            IMapper mapper,
            IReportRepository reportRepository,
            IRequestRepository requestRepository
        )
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _requestRepository = requestRepository;
        }


        // TODO.
        /// <summary>
        /// Добавление файла к заявке.
        /// </summary>
        /// <returns></returns>
        [HttpPost( "{requestId}" )]
        public IActionResult Insert( Guid requestId, [FromForm] Report report )
        {
            var file = report.File;
            if( file == null )
            {
                return BadRequest( "File does not attached" );
            }

            var isExist = _requestRepository.Get( requestId ) != null;
            if( !isExist )
            {
                return NotFound( $"Request with id='{requestId}' not found" );
            }

            using var fileStream = new FileStream( file.FileName, FileMode.Create );
            file.CopyTo( fileStream );
            
            var model = _mapper.Map<dto.Report>( report );
            model.RequestId = requestId;
            
            _reportRepository.Insert( model, fileStream );
            return Ok();
        }


        /// <summary>
        /// Удаление файла от заявки.
        /// </summary>
        /// <returns></returns>
        [HttpDelete( "{reportId}" )]
        public IActionResult Delete( Guid reportId )
        {
            var isExist = _reportRepository.Get( reportId ) != null;
            if( !isExist )
            {
                return NotFound( $"Report with id='{reportId} not found'" );
            }

            _reportRepository.Delete( reportId );
            return NoContent();
        }


        // TODO.
        /// <summary>
        /// Обновление файла.
        /// </summary>
        /// <returns></returns>
        [HttpPut( "{reportId}" )]
        public IActionResult Update( Guid reportId, [FromBody] Report report )
        {
            var file = report.File;
            if( file == null )
            {
                return BadRequest( "File does not attached" );
            }
            
            var isExist = _reportRepository.Get( reportId ) != null;
            if( !isExist )
            {
                return NotFound( $"Report with id='{reportId}' not found" );
            }

            using var fileStream = new FileStream( file.FileName, FileMode.Create );
            file.CopyTo( fileStream );
            
            var model = _mapper.Map<dto.Report>( report );
            model.Id = reportId;
            
            _reportRepository.Update( model, fileStream );
            return NoContent();
        }


        /// <summary>
        /// Вернуть файл по id файла.
        /// </summary>
        /// <returns></returns>
        [HttpGet( "{reportId}" )]
        public IActionResult Get( Guid reportId )
        {
            var report = _reportRepository.Get( reportId );
            if( report == null )
            {
                return NotFound( $"Report with id='{reportId}'" );
            }

            return new JsonResult(new
            {
                id            = report.Id,
                title         = report.Title,
                reportType    = report.ReportType.GetDisplayName(),
                status        = report.Status.GetDisplayName(),
                collaborators = report.Collaboratorsreportidfkeys
            });
        }
    }
}