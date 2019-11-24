using ConferenceApp.Web.Models;

namespace ConferenceApp.Web.Services.Jwt
{
    public interface IJwtHandler
    {
        JsonWebToken Create(string username);
    }
}