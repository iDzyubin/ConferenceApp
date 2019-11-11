using ConferenceApp.API.Models;

namespace ConferenceApp.API.Services.Jwt
{
    public interface IJwtHandler
    {
        JsonWebToken Create(string username);
    }
}