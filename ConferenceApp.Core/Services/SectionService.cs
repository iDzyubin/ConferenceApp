using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Services
{
    public class SectionService
    {
        private readonly MainDb _db;

        public SectionService( MainDb db ) 
            => _db = db;
        
        public async Task AttachAsync( Guid sessionId, Guid reportId ) 
            => await _db.InsertAsync( 
                new ReportsInSection { SectionId = sessionId, ReportId = reportId } );

        public async Task DetachAsync( Guid sessionId, Guid reportId ) 
            => await _db.ReportsInSections.DeleteAsync( x 
                => x.SectionId == sessionId && x.ReportId == reportId );
    }
}