using System;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Interfaces
{
    public interface IRequestService
    {
        void AttachReport( Guid requestId, ReportModel model );
    }
}