using System;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Services
{
    public interface IReportService
    {
        ReportStatus Approve( Guid requestId );

        ReportStatus Reject( Guid requestId );
    }
}