using System;
using System.Threading.Tasks;
using ConferenceApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConferenceApp.Web.Filters
{
    public class CompilationExists : Attribute, IAsyncActionFilter
    {
        private readonly ICompilationRepository _compilationRepository;

        public CompilationExists( ICompilationRepository compilationRepository )
        {
            _compilationRepository = compilationRepository;
        }

        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
        {
            var args = context.ActionArguments;
            if( !args.ContainsKey("compilationId") || !(args["compilationId"] is Guid userId) )
            {
                await next();
                return;
            }
            
            var isExists = await _compilationRepository.IsExistAsync(userId);
            if( !isExists )
            {
                var message = $"Compilation with id='{userId}' not found";
                context.Result = new NotFoundObjectResult( message );
            }
            await next();
        }
    }
}