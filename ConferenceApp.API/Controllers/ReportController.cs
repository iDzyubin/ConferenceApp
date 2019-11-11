using System;
using AutoMapper;
using ConferenceApp.API.Filters;
using ConferenceApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ConferenceApp.API.Models;
using ConferenceApp.Core.Services;
using dto = ConferenceApp.Core.DataModels;

namespace ConferenceApp.API.Controllers
{
    [ApiController]
    [Route( "api/[controller]/[action]" )]
    public class ReportController : ControllerBase
    {
//        private readonly IMapper _mapper;
//        private readonly IReportRepository _reportRepository;
//        private readonly IRequestRepository _requestRepository;

        private readonly IDocumentService _documentService;


        public ReportController
        (
//            IMapper mapper,
//            IReportRepository reportRepository,
//            IRequestRepository requestRepository,
            
            IDocumentService documentService
        )
        {
//            _mapper = mapper;
//            _reportRepository = reportRepository;
//            _requestRepository = requestRepository;
            
            _documentService = documentService;
        }


//        // TODO.
//        /// <summary>
//        /// Добавление файла к заявке.
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost( "{requestId}" )]
//        [ModelValidation]
//        public IActionResult Insert( Guid requestId, [FromBody] Report report )
//        {
//            var isExist = _requestRepository.Get( requestId ) != null;
//            if( isExist )
//            {
//                return NotFound( $"Report of request with id='{requestId}' not found" );
//            }
//
//            var model = _mapper.Map<dto.Report>( report );
//            _reportRepository.Insert( model );
//            return Ok();
//        }
//
//
//        // TODO.
//        /// <summary>
//        /// Удаление файла от заявки.
//        /// </summary>
//        /// <returns></returns>
//        [HttpDelete( "{reportId}" )]
//        public IActionResult Delete( Guid reportId )
//        {
//            var isExist = _reportRepository.Get( reportId ) != null;
//            if( isExist )
//            {
//                return NotFound( $"Report with id='{reportId}'" );
//            }
//
//            _reportRepository.Delete( reportId );
//            return NoContent();
//        }
//
//
//        // TODO.
//        /// <summary>
//        /// Обновление файла.
//        /// </summary>
//        /// <returns></returns>
//        [HttpPut( "{reportId}" )]
//        [ModelValidation]
//        public IActionResult Update( Guid reportId, [FromBody] Report report )
//        {
//            var isExist = _reportRepository.Get( reportId ) != null;
//            if( isExist )
//            {
//                return NotFound( $"Report with id='{reportId}'" );
//            }
//
//            var model = _mapper.Map<dto.Report>( report );
//            _reportRepository.Update( model );
//            return NoContent();
//        }
//
//
//        // TODO.
//        /// <summary>
//        /// Вернуть файл по id файла.
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet( "{reportId}" )]
//        public IActionResult Get( Guid reportId )
//        {
//            var report = _reportRepository.Get( reportId );
//            if( report == null )
//            {
//                return NotFound( $"Report with id='{reportId}'" );
//            }
//
//            return Ok( report );
//        }
//
//
//        // TODO.
//        /// <summary>
//        /// Вернуть файлы по заявке.
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet( "{requestId}" )]
//        public IActionResult GetByRequest( Guid requestId )
//        {
//            var request = _requestRepository.Get( requestId );
//            if( request == null )
//            {
//                return NotFound( $"Request with id='{requestId}' not found" );
//            }
//
//            var reports = _reportRepository.Get( x => x.RequestId == requestId );
//            return Ok( reports );
//        }
    }
}