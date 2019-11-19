using ConferenceApp.API.Models;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.API.Interfaces
{
    public interface IRequestRepositoryAdapter : IRepository<RequestModel>
    {
    }
}