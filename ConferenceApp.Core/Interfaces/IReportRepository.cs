using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<ReportModel>
    {
        void InsertRange( List<ReportModel> reports );

        Guid InsertWithId( ReportModel reportModel );

        void DeleteRange( Guid requestId );

        IEnumerable<ReportModel> GetReportsByUser( Guid userId );

        void ChangeStatus( Guid requestId, ReportStatus to );
    }
}