using System.Threading.Tasks;

namespace ConferenceApp.API.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> IsCurrentActiveToken();

        Task DeactivateCurrentAsync();

        Task<bool> IsActiveAsync( string token );

        Task DeactivateAsync( string token );
    }
}