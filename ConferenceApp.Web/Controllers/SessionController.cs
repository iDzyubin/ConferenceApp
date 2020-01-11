using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Web.Controllers
{
    [ApiController]
    [Route( "/api/[controller]" )]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ISessionService _sessionService;

        public SessionController
        (
            ISessionRepository sessionRepository,
            IReportRepository reportRepository,
            ISessionService sessionService
        )
        {
            _sessionRepository = sessionRepository;
            _reportRepository = reportRepository;
            _sessionService = sessionService;
        }


        /// <summary>
        /// Вернуть все сессии.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _sessionRepository.GetAllAsync();
            return Ok( items );
        }


        /// <summary>
        /// Вернуть сессию по id.
        /// </summary>
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( Guid id )
        {
            var item = await _sessionRepository.GetAsync( id );
            if( item == null )
            {
                return NotFound( $"Session with id='{id}' not found" );
            }

            return Ok( item );
        }


        /// <summary>
        /// Добавить новую сессию
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public async Task<IActionResult> Create( [FromBody] Session session )
        {
            var id = await _sessionRepository.InsertAsync( session );
            return Ok( $"Session with id='{id}' was successfully created" );
        }


        /// <summary>
        /// Удалить сессию.
        /// </summary>
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( Guid id )
        {
            if( !await _sessionRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Session with id='{id}' not found" );
            }

            await _sessionRepository.DeleteAsync( id );
            return NoContent();
        }


        /// <summary>
        /// Обновить информацию о сессии.
        /// </summary>
        [HttpPut( "{id}" )]
        [ModelValidation]
        public async Task<IActionResult> Update( Guid id, [FromBody] Session session )
        {
            if( !await _sessionRepository.IsExistAsync( id ) )
            {
                return NotFound( $"Session with id='{id}' not found" );
            }

            await _sessionRepository.UpdateAsync( session );
            return NoContent();
        }


        /// <summary>
        /// Добавить доклад в сессию.
        /// </summary>
        [HttpPost( "{reportId}/attach-to/{sessionId}" )]
        public async Task<IActionResult> Attach( Guid sessionId, Guid reportId )
        {
            if( !await _sessionRepository.IsExistAsync( sessionId ) )
            {
                return NotFound( $"Session with id='{sessionId}' not found" );
            }

            if( !await _reportRepository.IsExistAsync( reportId ) )
            {
                return NotFound( $"Report with id='{reportId}' not found." );
            }

            try
            {
                await _sessionService.AttachAsync( sessionId, reportId );
                return Ok( $"Report with id='{reportId}' was successfully attached to session with id='{sessionId}'" );
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
            if( !await _sessionRepository.IsExistAsync( sessionId ) )
            {
                return NotFound( $"Session with id='{sessionId}' not found" );
            }

            if( !await _reportRepository.IsExistAsync( reportId ) )
            {
                return NotFound( $"Report with id='{reportId}' not found." );
            }

            try
            {
                await _sessionService.DetachAsync( sessionId, reportId );
                return Ok( $"Report with id='{reportId}' was successfully detached from session with id='{sessionId}'"
                );
            }
            catch( Exception e )
            {
                return BadRequest( $"Report with id='{reportId}' did not detached: {e.Message}. Try again later." );
            }
        }
    }
}