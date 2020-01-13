using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
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
        private readonly ICompilationService _compilationService;
        private readonly ICompilationRepository _compilationRepository;


        public CompilationController
        ( 
            IMapper mapper,
            ICompilationService compilationService, 
            ICompilationRepository compilationRepository 
        )
        {
            _mapper = mapper;
            _compilationService = compilationService;
            _compilationRepository = compilationRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _compilationRepository.GetAllAsync();
            var model = _mapper.Map<List<CompilationModel>>( items );
            return Ok( model );
        }
        
        
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if( file == null ) return BadRequest( "File is empty" );
            
            try
            {
                var compilationId = await _compilationService.InsertFileAsync( file.ConvertToFileStream() );
                return Ok( compilationId ); 
            }
            catch( Exception e )
            {
                return BadRequest( $"File did not upload: {e.Message}. Try to upload file later." );
            }
        }


        [HttpGet("download/{compilationId}")]
        public async Task<IActionResult> Download(Guid compilationId)
        {
            if( ! await _compilationRepository.IsExistAsync( compilationId ) )
            {
                return NotFound( $"Compilation with id='{compilationId}' not found" );
            }
            
            try
            {
                var (stream, fileName) = await _compilationService.GetFileAsync( compilationId );
                return File( stream, "application/octet-stream", fileName );
            }
            catch( Exception e )
            {
                return BadRequest( $"File did not download: {e.Message}. Try again later." );
            }
            
        }
    }
}