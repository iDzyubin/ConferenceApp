using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Services
{
    public class SessionService : ISessionService
    {
        private readonly MainDb _db;

        public SessionService( MainDb db )
        {
            _db = db;
        }
        
        public async Task AttachAsync( Guid sessionId, Guid reportId )
        {
            await _db.InsertAsync( 
                new ReportsInSession { SessionId = sessionId, ReportId = reportId } );
        }

        public async Task DetachAsync( Guid sessionId, Guid reportId )
        {
            await _db.ReportsInSessions.DeleteAsync( x => 
                x.SessionId == sessionId && x.ReportId == reportId );
        }
    }
}