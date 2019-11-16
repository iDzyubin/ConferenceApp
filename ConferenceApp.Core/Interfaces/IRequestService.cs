using System;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRequestService
    {
        RequestStatus Approve( Guid requestId );

        RequestStatus Reject( Guid requestId );
    }
}