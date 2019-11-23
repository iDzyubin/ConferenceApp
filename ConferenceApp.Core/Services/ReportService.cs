using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class ReportService : IChangable<ReportStatus>
    {
        private readonly IReportRepository _reportRepository;

        public ReportService( IReportRepository reportRepository )
        {
            _reportRepository = reportRepository;
        }

        public ReportStatus ChangeStatusTo( Guid reportId, ReportStatus status )
        {
            var request = _reportRepository.GetDto( reportId );
            if( request == null )
            {
                return ReportStatus.None;
            }

            _reportRepository.ChangeStatus( reportId, status );
            return status;
        }
    }
}