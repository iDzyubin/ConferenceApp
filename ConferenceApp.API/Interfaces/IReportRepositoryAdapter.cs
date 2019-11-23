using System;
using System.Collections.Generic;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.API.Interfaces
{
    public interface IReportRepositoryAdapter : IRepository<ReportViewModel>
    {
        IEnumerable<ReportViewModel> GetReportsByRequest( Guid requestId );
    }
}