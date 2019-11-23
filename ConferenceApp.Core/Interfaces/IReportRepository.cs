using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<ReportModel>
    {
        void InsertRange( List<ReportModel> reports );

        void DeleteRange( Guid requestId );

        IEnumerable<ReportModel> GetReportsByRequest( Guid requestId );

        void ChangeStatus( Guid requestId, ReportStatus status );
    }
}