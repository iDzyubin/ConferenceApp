using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class RequestService : IChangable<RequestStatus>
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService( IRequestRepository requestRepository )
        {
            _requestRepository = requestRepository;
        }


        public RequestStatus ChangeStatusTo( Guid requestId, RequestStatus status )
        {
            var request = _requestRepository.GetDto( requestId );
            if( request == null )
            {
                return RequestStatus.None;
            }

            _requestRepository.ChangeStatus( requestId, status );
            return status;
        }
    }
}