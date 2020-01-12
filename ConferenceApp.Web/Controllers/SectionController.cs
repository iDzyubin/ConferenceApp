using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "/api/[controller]" )]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ISectionService _sectionService;

        public SectionController
        (
            ISectionRepository sectionRepository,
            IReportRepository reportRepository,
            ISectionService sectionService
        )
        {
            _sectionRepository = sectionRepository;
            _reportRepository = reportRepository;
            _sectionService = sectionService;
        }


        /// <summary>
        /// Вернуть все сессии.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _sectionRepository.GetAllAsync();
            return Ok( items );
        }


        /// <summary>
        /// Вернуть сессию по id.
        /// </summary>
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( Guid id )
        {
            var item = await _sectionRepository.GetAsync( id );
            if( item == null )
            {
                return NotFound( $"Section with id='{id}' not found" );
            }

            return Ok( item );
        }


        /// <summary>
        /// Добавить новую сессию
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public async Task<IActionResult> Create( [FromBody] Section section )
        {
            var id = await _sectionRepository.InsertAsync( section );
            return Ok( new
            {
                message = $"Section with id='{id}' was successfully created",
                id
            });
        }


        /// <summary>
        /// Удалить сессию.
        /// </summary>
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( Guid id )
        {
            if( !await _sectionRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Section with id='{id}' not found" );
            }

            await _sectionRepository.DeleteAsync( id );
            return NoContent();
        }


        /// <summary>
        /// Обновить информацию о сессии.
        /// </summary>
        [HttpPut( "{id}" )]
        [ModelValidation]
        public async Task<IActionResult> Update( Guid id, [FromBody] Section section )
        {
            if( !await _sectionRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Section with id='{id}' not found" );
            }

            await _sectionRepository.UpdateAsync( section );
            return NoContent();
        }


        /// <summary>
        /// Добавить доклад в сессию.
        /// </summary>
        [HttpPost( "{reportId}/attach-to/{sessionId}" )]
        public async Task<IActionResult> Attach( Guid sessionId, Guid reportId )
        {
            if( !await _sectionRepository.IsExistAsync( sessionId ) )
            {
                return NotFound( $"Section with id='{sessionId}' not found" );
            }

            if( !await _reportRepository.IsExistAsync( reportId ) )
            {
                return NotFound( $"Report with id='{reportId}' not found." );
            }

            try
            {
                await _sectionService.AttachAsync( sessionId, reportId );
                return Ok( $"Report with id='{reportId}' was successfully attached to section with id='{sessionId}'" );
            }
            catch( Exception e )
            {
                return BadRequest( $"Report with id='{reportId}' did not attached: {e.Message}. Try again later." );
            }
        }


        /// <summary>
        /// Открепить доклад от сессии
        /// </summary>
        [HttpPost( "{reportId}/detach-from/{sessionId}" )]
        public async Task<IActionResult> Detach( Guid sessionId, Guid reportId )
        {
            if( !await _sectionRepository.IsExistAsync( sessionId ) )
            {
                return NotFound( $"Section with id='{sessionId}' not found" );
            }

            if( !await _reportRepository.IsExistAsync( reportId ) )
            {
                return NotFound( $"Report with id='{reportId}' not found." );
            }

            try
            {
                await _sectionService.DetachAsync( sessionId, reportId );
                return Ok( $"Report with id='{reportId}' was successfully detached from section with id='{sessionId}'"
                );
            }
            catch( Exception e )
            {
                return BadRequest( $"Report with id='{reportId}' did not detached: {e.Message}. Try again later." );
            }
        }
    }
}