using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDocumentService _documentService;
        private readonly ISectionRepository _sectionRepository;
        private readonly IMapper _mapper;
        private readonly MainDb _db;


        public ReportRepository
        (
            IDocumentService documentService,
            ISectionRepository sectionRepository,
            IMapper mapper,
            MainDb db
        )
        {
            _documentService = documentService;
            _sectionRepository = sectionRepository;
            _mapper = mapper;
            _db = db;
        }


        /// <summary>
        /// Добавить доклад (основная информация).
        /// </summary>
        public async Task<Guid> InsertAsync( ReportModel model )
        {
            var report = _mapper.Map<Report>( model );
            report.Id = Guid.NewGuid();
            report.Path = String.Empty;

            await _db.InsertAsync( report );
            await InsertCollaboratorsAsync( report.Id, model.Collaborators );

            return report.Id;
        }


        /// <summary>
        /// Обновить информацию по докладу.
        /// </summary>
        public async Task UpdateAsync( ReportModel model )
        {
            var report = _mapper.Map<Report>( model );
            report.Path = String.Empty;
            
            _db.Update( report );
            await InsertCollaboratorsAsync(model.Id, model.Collaborators);            
        }

        
        /// <summary>
        /// Изменить статус заявки.
        /// </summary>
        public async Task ChangeStatusAsync( Guid reportId, ReportStatus status )
        {
            await _db.Reports
                .Where( x => x.Id == reportId )
                .Set( x => x.Status, status )
                .UpdateAsync();
        }


        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public async Task DeleteAsync( Guid reportId )
        {
            await _documentService
                .DeleteFileAsync( reportId );
            await _db.Collaborators
                .DeleteAsync( x => x.ReportId == reportId );
            await _db.Reports
                .DeleteAsync( x => x.Id == reportId );
        }


        public async Task<bool> IsExistAsync( Guid id )
        {
            var user = await _db.Reports.FirstOrDefaultAsync( x => x.Id == id );
            return user != null;
        }


        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public async Task<ReportVM> GetAsync( Guid reportId )
        {
            var report = await _db.Reports.FirstOrDefaultAsync( x => x.Id == reportId );
            if( report == null )
            {
                return null;
            }

            report.Collaboratorsreportidfkeys = await GetCollaboratorsAsync( reportId );

            var model = _mapper.Map<ReportVM>( report );
            model.SectionId = (await _sectionRepository.GetByReport( reportId ))?.SectionId ?? Guid.Empty;
            return model;
        }


        public async Task<List<ReportVM>> GetReportsByUserAsync( Guid userId )
        {
            var reports = _db.Reports.Where(x => x.UserId == userId).ToList();
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = await GetCollaboratorsAsync( report.Id );
            }

            var model = _mapper.Map<List<ReportVM>>( reports );
            foreach( var item in model )
            {
                item.SectionId = (await _sectionRepository.GetByReport( item.Id ))?.SectionId ?? Guid.Empty;
            }
            
            return model;
        }

        
        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public async Task<List<ReportVM>> GetAsync()
        {
            var reports = _db.Reports.ToList();
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = await GetCollaboratorsAsync( report.Id );
            }
            
            var model = _mapper.Map<List<ReportVM>>( reports );
            foreach( var item in model )
            {
                item.SectionId = (await _sectionRepository.GetByReport( item.Id ))?.SectionId ?? Guid.Empty;
            }
            
            return model;
        }
        
        /// <summary>
        /// Список соавторов доклада.
        /// </summary>
        private async Task<List<Collaborator>> GetCollaboratorsAsync( Guid reportId )
        {
            var collaborators = 
                from c in _db.Collaborators
                join user in _db.Users on c.UserId equals user.Id
                where c.ReportId == reportId
                select new Collaborator { ReportId = c.ReportId, UserId = c.UserId, User = user };
            
            return await collaborators.ToListAsync();
        }

        /// <summary>
        /// Добавить (или обновить) список соавторов.
        /// </summary>
        private async Task InsertCollaboratorsAsync(Guid reportId, IEnumerable<string> collaborators)
        {
            // Выбрать всех подтвержденных пользователей, чья почта была указана.
            var collaboratorIds = await 
                ( from user in _db.Users
                    where user.UserStatus == UserStatus.Confirmed
                          && collaborators.Contains( user.Email )
                    select user.Id ).ToListAsync();

            // Добавить их в таблицу.
            foreach( var collaboratorId in collaboratorIds )
            {
                var collaborator = new Collaborator { UserId = collaboratorId, ReportId = reportId };
                await _db.InsertAsync( collaborator );
            }
        }

        public List<ReportModel> Get( Func<ReportModel, bool> filter )
        {
            throw new NotImplementedException();
        }

        Task<ReportModel> IRepository<ReportModel>.GetAsync( Guid id )
        {
            throw new NotImplementedException();
        }

        public Task<List<ReportModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}