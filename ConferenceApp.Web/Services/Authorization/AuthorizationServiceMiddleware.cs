using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ConferenceApp.Web.Services.Authorization
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
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
    }
}