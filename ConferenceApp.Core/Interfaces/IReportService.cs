using System;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportService
    {
        Task AttachUserAsync( Guid reportId, string email );

        Task DetachUserAsync( Guid reportId, string email );

        Task<bool> ContainsUser( Guid reportId, Guid userId );
    }
}