using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task<ReportsInSection> GetByReport( Guid reportId );
    }
}