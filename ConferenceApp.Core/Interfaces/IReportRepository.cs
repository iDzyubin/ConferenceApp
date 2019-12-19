using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<Report>, IChangable<ReportStatus>
    {
        Guid Insert( ReportModel model );
        IEnumerable<Report> GetReportsByUser( Guid userId );
    }
}