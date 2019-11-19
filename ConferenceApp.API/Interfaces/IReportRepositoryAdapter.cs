using System;
using System.Collections.Generic;
using ConferenceApp.API.Models;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.API.Interfaces
{
    public interface IReportRepositoryAdapter : IRepository<ReportModel>
    {
        void InsertRange( IEnumerable<ReportModel> reports );

        void DeleteRange( Guid requestId );
    }
}