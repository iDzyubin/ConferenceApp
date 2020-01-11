using System;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly MainDb _db;

        public ReportService( MainDb db )
        {
            _db = db;
        }

        public async Task AttachUserAsync( Guid reportId, string email )
        {
            var user = await GetUserByEmail( email );

            await _db.InsertAsync( 
                new Collaborator { ReportId = reportId, UserId = user.Id } 
            );
        }

        public async Task DetachUserAsync( Guid reportId, string email )
        {
            var user = await GetUserByEmail( email );

            await _db.Collaborators.DeleteAsync( x =>
                x.ReportId == reportId && x.UserId == user.Id
            );
        }

        public async Task<bool> ContainsUser( Guid reportId, Guid userId )
        {
            var collaborator = await _db.Collaborators.FirstOrDefaultAsync( x => 
                x.ReportId == reportId && x.UserId == userId );
            
            return collaborator != null;
        }
        
        private async Task<User> GetUserByEmail(string email) =>
            await _db.Users.FirstOrDefaultAsync( x =>
                x.Email.ToUpper() == email.ToUpper()
            );
    }
}