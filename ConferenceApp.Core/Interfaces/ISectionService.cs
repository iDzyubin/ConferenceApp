using System;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Interfaces
{
    public interface ISectionService
    {
        Task AttachAsync( Guid sessionId, Guid reportId );

        Task DetachAsync( Guid sessionId, Guid reportId );
    }
}