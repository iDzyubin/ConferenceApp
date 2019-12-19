using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<ReportModel>, IChangable<ReportStatus>
    {
        IEnumerable<ReportModel> GetReportsByUser( Guid userId );
    }
}