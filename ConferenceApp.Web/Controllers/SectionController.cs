using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
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
        private readonly SectionService _sectionService;

        public SectionController( ISectionRepository sectionRepository, SectionService sectionService )
        {
            _sectionRepository = sectionRepository;
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
        /// Вернуть секцию по id.
        /// </summary>
        [HttpGet( "{sectionId}" )]
        [ServiceFilter(typeof(SectionExists))]
        public async Task<IActionResult> Get( Guid sectionId )
        {
            var item = await _sectionRepository.GetAsync( sectionId );
            return Ok( item );
        }


        /// <summary>
        /// Добавить новую секцию
        /// </summary>
        [HttpPost]
        [ModelValidation]
        public async Task<IActionResult> Create( [FromBody] Section section )
        {
            var id = await _sectionRepository.InsertAsync( section );
            return Ok( new { id, message = $"Section with id='{id}' was successfully created" });
        }


        /// <summary>
        /// Удалить секцию.
        /// </summary>
        [HttpDelete( "{sectionId}" )]
        [ServiceFilter(typeof(SectionExists))]
        public async Task<IActionResult> Delete( Guid sectionId )
        {
            await _sectionRepository.DeleteAsync( sectionId );
            return NoContent();
        }


        /// <summary>
        /// Обновить информацию о секции.
        /// </summary>
        [HttpPut( "{sectionId}" )]
        [ModelValidation]
        [ServiceFilter(typeof(SectionExists))]
        public async Task<IActionResult> Update( Guid sectionId, [FromBody] Section section )
        {
            await _sectionRepository.UpdateAsync( section );
            return NoContent();
        }


        /// <summary>
        /// Добавить доклад в секцию.
        /// </summary>
        [HttpPost( "{reportId}/attach-to/{sectionId}" )]
        [ServiceFilter(typeof(SectionExists))]
        [ServiceFilter(typeof(ReportExists))]
        [ExceptionFilter("Report did not attached")]
        public async Task<IActionResult> Attach( Guid sectionId, Guid reportId )
        {
            await _sectionService.AttachAsync( sectionId, reportId );
            return Ok( $"Report with id='{reportId}' was successfully attached to section with id='{sectionId}'" );
        }


        /// <summary>
        /// Открепить доклад от секции
        /// </summary>
        [HttpPost( "{reportId}/detach-from/{sectionId}" )]
        [ServiceFilter(typeof(SectionExists))]
        [ServiceFilter(typeof(ReportExists))]
        [ExceptionFilter("Report did not detached")]
        public async Task<IActionResult> Detach( Guid sectionId, Guid reportId )
        {
            await _sectionService.DetachAsync( sectionId, reportId );
            return Ok( $"Report with id='{reportId}' was successfully detached from section with id='{sectionId}'");
        }
    }
}