using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService( IRequestRepository requestRepository )
        {
            _requestRepository = requestRepository;
        }

        public RequestStatus Approve( Guid requestId )
        {
            var request = _requestRepository.Get( requestId );
            if( request == null )
            {
                return RequestStatus.None;
            }

            request.Status = RequestStatus.Approved;
            _requestRepository.Update( request );

            return RequestStatus.Approved;
        }

        public RequestStatus Reject( Guid requestId )
        {
            var request = _requestRepository.Get( requestId );
            if( request == null )
            {
                return RequestStatus.None;
            }

            request.Status = RequestStatus.Rejected;
            _requestRepository.Update( request );

            return RequestStatus.Rejected;
        }
    }
}