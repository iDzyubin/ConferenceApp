using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<ReportModel>, IChangable<ReportStatus>
    {
        Task<List<ReportVM>> GetReportsByUserAsync( Guid userId );

        new Task<ReportVM> GetAsync( Guid reportId );

        Task<List<ReportVM>> GetAsync();
    }
}