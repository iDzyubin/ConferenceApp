using System;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Services
{
    public class RequestService : IRequestService
    {
        private readonly IReportRepository _reportRepository;

        public RequestService( IReportRepository reportRepository )
        {
            _reportRepository = reportRepository;
        }

        /// <summary>
        /// Приложить доклад к заявке.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AttachReport( Guid requestId, ReportModel model )
        {
            if( model.RequestId == Guid.Empty )
            {
                model.RequestId = requestId;
            }
            
            _reportRepository.Insert(model);
        }
    }
}