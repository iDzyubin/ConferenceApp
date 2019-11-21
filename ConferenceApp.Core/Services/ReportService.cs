using System;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService( IReportRepository reportRepository )
        {
            _reportRepository = reportRepository;
        }

        public ReportStatus Approve( Guid requestId )
        {
            var report = _reportRepository.GetDto( requestId );
            if( report == null )
            {
                return ReportStatus.None;
            }

            report.Status = ReportStatus.Approved;
            _reportRepository.Update( report );

            return ReportStatus.Approved;
        }

        public ReportStatus Reject( Guid requestId )
        {
            var report = _reportRepository.Get( requestId );
            if( report == null )
            {
                return ReportStatus.None;
            }

            report.Status = ReportStatus.Rejected;
            _reportRepository.Update( report );

            return ReportStatus.Rejected;
        }
    }
}