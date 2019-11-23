using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRequestRepository : IRepository<RequestModel>
    {
        Request GetDto( Guid requestId );

        void ChangeStatus( Guid requestId, RequestStatus status );
    }
}