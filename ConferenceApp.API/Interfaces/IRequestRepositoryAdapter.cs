using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.API.Interfaces
{
    public interface IRequestRepositoryAdapter : IRepository<RequestViewModel>
    {
    }
}