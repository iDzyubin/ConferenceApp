using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ConferenceApp.API.Services.Authorization
{
    public class AuthorizationServiceMiddleware : IMiddleware
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationServiceMiddleware( IAuthorizationService authorizationService )
        {
            _authorizationService = authorizationService;
        }
        
        public async Task InvokeAsync( HttpContext context, RequestDelegate next )
        {
            if( await _authorizationService.IsCurrentActiveToken() )
            {
                await next( context );
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}