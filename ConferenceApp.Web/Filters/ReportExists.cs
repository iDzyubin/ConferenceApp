using System;
using System.Threading.Tasks;
using ConferenceApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConferenceApp.Web.Filters
{
    public class ReportExists: Attribute, IAsyncActionFilter
    {
        private readonly IReportRepository _reportRepository;

        public ReportExists( IReportRepository reportRepository )
        {
            _reportRepository = reportRepository;
        }

        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
        {
            var args = context.ActionArguments;
            if( !args.ContainsKey("reportId") || !(args["reportId"] is Guid userId) )
            {
                await next();
                return;
            }
            
            var isExists = await _reportRepository.IsExistAsync(userId);
            if( !isExists )
            {
                var message = $"Report with id='{userId}' not found";
                context.Result = new NotFoundObjectResult( message );
            }
            await next();
        }
    }
}