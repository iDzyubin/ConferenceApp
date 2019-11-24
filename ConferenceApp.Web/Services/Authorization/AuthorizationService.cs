using System;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ConferenceApp.Web.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;

        public AuthorizationService
        (
            IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions
        )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync( GetCurrentAsync() );

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync( GetCurrentAsync() );

        public async Task<bool> IsActiveAsync( string token )
            => await _cache.GetStringAsync( GetKey( token ) ) == null;

        public async Task DeactivateAsync( string token )
        {
            var distributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes( _jwtOptions.Value.ExpiryMinutes )
            };
            
            await _cache.SetStringAsync( GetKey( token ), " ", distributedCacheEntryOptions );
        }
        
        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext
                .Request
                .Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split( " " ).Last();
        }

        private static string GetKey( string token )
            => $"tokens:{token}:deactivated";
    }
}