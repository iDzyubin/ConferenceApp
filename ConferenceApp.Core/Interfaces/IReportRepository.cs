using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<ReportModel>, IChangable<ReportStatus>
    {
        Task<List<ReportModel>> GetReportsByUserAsync( Guid userId );
    }
}