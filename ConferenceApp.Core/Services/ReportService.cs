using System;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Services
{
    public class ReportService: IChangable<ReportStatus>
    {
        private readonly IUserRepository _userRepository;
        private readonly IReportRepository _reportRepository;
        private readonly NotificationService _notificationService;
        private readonly DocumentService _documentService;
        private readonly MainDb _db;

        public ReportService
        ( 
            IUserRepository userRepository, 
            NotificationService notificationService,
            DocumentService documentService,
            IReportRepository reportRepository,
            MainDb db 
        )
        {
            _notificationService = notificationService;
            _userRepository = userRepository;
            _documentService = documentService;
            _reportRepository = reportRepository;
            _db = db;
        }

        public async Task AttachUserAsync( Guid reportId, string email )
        {
            var user = await _userRepository.GetByEmailAsync( email );
            await _db.InsertAsync( 
                new Collaborator { ReportId = reportId, UserId = user.Id } );
        }

        public async Task DetachUserAsync( Guid reportId, string email )
        {
            var user = await _userRepository.GetByEmailAsync( email );
            await _db.Collaborators.DeleteAsync( x 
                => x.ReportId == reportId && x.UserId == user.Id );
        }

        public async Task<bool> ContainsUser( Guid reportId, string email )
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var collaborator = await _db.Collaborators
                .FirstOrDefaultAsync( x 
                    => x.ReportId == reportId && x.UserId == user.Id );
            
            return collaborator != null;
        }


        public async Task DeleteAsync( Guid reportId )
        {
            await _documentService
                .DeleteFileAsync( reportId );
            await _db.Collaborators
                .DeleteAsync( x => x.ReportId == reportId );
            await _db.ReportsInSections
                .DeleteAsync( x => x.ReportId == reportId );
            await _reportRepository
                .DeleteAsync(  reportId );
        }
        
        
        /// <summary>
        /// Изменить статус заявки.
        /// </summary>
        public async Task ChangeStatusAsync( Guid reportId, ReportStatus to )
        {
            if( await _db.Reports.FirstOrDefaultAsync(x => x.Id == reportId) == null )
            {
                return;
            }
            
            await _db.Reports
                .Where( x => x.Id == reportId )
                .Set( x => x.Status, to )
                .UpdateAsync();

            var user = await (
                from r in _db.Reports
                join u in _db.Users on r.UserId equals u.Id
                where r.Id == reportId
                select u
            ).FirstOrDefaultAsync();
            
            if( AreAllReportsHandled( user.Id ) )
            {
                var reports = await _db.Reports.Where( x => x.UserId == user.Id ).ToListAsync();
                await _notificationService.SendReportReviewResult( user.Email, reports );
            }
        }

        private bool AreAllReportsHandled( Guid userId )
        {
            return _db.Reports
                .Where( x => x.Id == userId )
                .All( report => report.Status != ReportStatus.None );
        }
    }
}