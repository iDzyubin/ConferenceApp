using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConferenceApp.Web.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly string _errorDescription;
        
        public ExceptionFilter()
        {
            _errorDescription = String.Empty;
        }
        
        public ExceptionFilter( string errorDescription )
        {
            _errorDescription = errorDescription;
        }

        public void OnException( ExceptionContext context )
        {
            context.Result = new BadRequestObjectResult( $"{_errorDescription}: {context.Exception.Message}" );
            context.ExceptionHandled = true;
        }
    }
}