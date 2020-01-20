using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<ReportInnerModel>
    {
        Task<List<ReportOuterModel>> GetReportsByUserAsync( Guid userId );

        new Task<ReportOuterModel> GetAsync( Guid reportId );

        new Task<List<ReportOuterModel>> GetAllAsync();
    }
}