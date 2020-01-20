using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using ConferenceApp.Core.Services;
using ConferenceApp.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.Controllers
{
    /// <summary>
    /// Контроллер, отвечающий за сборники докладов.
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class CompilationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CompilationService _compilationService;
        private readonly ICompilationRepository _compilationRepository;


        public CompilationController
        ( 
            IMapper mapper,
            CompilationService compilationService, 
            ICompilationRepository compilationRepository 
        )
        {
            _mapper = mapper;
            _compilationService = compilationService;
            _compilationRepository = compilationRepository;
        }


        /// <summary>
        /// Получение всех сборников.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _compilationRepository.GetAllAsync();
            var model = _mapper.Map<List<CompilationModel>>( items );
            return Ok( model );
        }
        
        
        /// <summary>
        /// Загрузка сборника на сервер.
        /// </summary>
        [HttpPost("upload")]
        [ExceptionFilter("File did not upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if( file == null ) return BadRequest( "File is empty" );
            
            var compilationId = await _compilationService.InsertFileAsync( file.ConvertToFileStream() );
            return Ok( compilationId ); 
        }


        /// <summary>
        /// Загрузка сборника с сервера.
        /// </summary>
        [HttpGet("download/{compilationId}")]
        [ExceptionFilter("File did not download")]
        [ServiceFilter(typeof(CompilationExists))]
        public async Task<IActionResult> Download(Guid compilationId)
        {
            var (stream, fileName) = await _compilationService.GetFileAsync( compilationId );
            return File( stream, "application/octet-stream", fileName );
        }
    }
}