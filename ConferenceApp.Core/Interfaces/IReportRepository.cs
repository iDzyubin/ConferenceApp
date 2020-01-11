using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<Report>, IChangable<ReportStatus>
    {
        Task<Guid> InsertAsync( ReportModel model );
        
        Task UpdateAsync( ReportModel model );

        Task<List<Report>> GetReportsByUserAsync( Guid userId );
    }
}