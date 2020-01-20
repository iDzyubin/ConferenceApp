using System;
using System.Threading.Tasks;
using ConferenceApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConferenceApp.Web.Filters
{
    public class UserExists: Attribute, IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;

        public UserExists( IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
        {
            var args = context.ActionArguments;
            if( !args.ContainsKey("userId") && !args.ContainsKey("userId") )
            {
                await next();
                return;
            }

            var isExists = false;
            var message = string.Empty;
            if( args.ContainsKey("userId") && args["userId"] is Guid userId )
            {
                isExists = await _userRepository.IsExistAsync(userId);
                message = $"User with id='{userId}' not found";
            }
            else if( args.ContainsKey("email") && args["email"] is string email )
            {
                isExists = await _userRepository.IsExistAsync(email);
                message = $"User with email='{email}' not found";
            }

            if( !isExists )
            {
                context.Result = new NotFoundObjectResult( message );
            }
            await next();
        }
    }
}