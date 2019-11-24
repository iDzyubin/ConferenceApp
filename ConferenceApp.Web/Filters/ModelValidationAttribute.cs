using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConferenceApp.Web.Filters
{
    public class ModelValidationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted( ActionExecutedContext context )
        {
            var modelState = context.ModelState;
            if( !modelState.IsValid )
            {
                context.Result = new BadRequestObjectResult( modelState );
            }
        }

        public void OnActionExecuting( ActionExecutingContext context )
        {
        }
    }
}