using System;
using System.Threading.Tasks;
using ConferenceApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConferenceApp.Web.Filters
{
    public class SectionExists: Attribute, IAsyncActionFilter
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionExists( ISectionRepository sectionRepository )
        {
            _sectionRepository = sectionRepository;
        }

        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
        {
            var args = context.ActionArguments;
            if( !args.ContainsKey("sectionId") || !(args["sectionId"] is Guid sectionId) )
            {
                await next();
                return;
            }
            
            var isExists = await _sectionRepository.IsExistAsync(sectionId);
            if( !isExists )
            {
                var message = $"Section with id='{sectionId}' not found";
                context.Result = new NotFoundObjectResult( message );
            }
            await next();
        }
    }
}