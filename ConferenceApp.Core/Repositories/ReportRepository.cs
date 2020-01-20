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
        private readonly ISectionRepository _sectionRepository;
        private readonly IMapper _mapper;
        private readonly MainDb _db;


        public ReportRepository
        (
            ISectionRepository sectionRepository,
            IMapper mapper,
            MainDb db
        )
        {
            _sectionRepository = sectionRepository;
            _mapper = mapper;
            _db = db;
        }


        /// <summary>
        /// Добавить доклад (основная информация).
        /// </summary>
        public async Task<Guid> InsertAsync( ReportInnerModel innerModel )
        {
            var report = _mapper.Map<Report>( innerModel );
            report.Id = Guid.NewGuid();
            report.Path = String.Empty;

            await _db.InsertAsync( report );
            await InsertCollaboratorsAsync( report.Id, innerModel.Collaborators );

            return report.Id;
        }


        /// <summary>
        /// Обновить информацию по докладу.
        /// </summary>
        public async Task UpdateAsync( ReportInnerModel innerModel )
        {
            var report = _mapper.Map<Report>( innerModel );
            report.Path = String.Empty;
            
            _db.Update( report );
            await InsertCollaboratorsAsync(innerModel.Id, innerModel.Collaborators);            
        }

        
        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public async Task DeleteAsync( Guid reportId )
        {
            await _db.Reports.DeleteAsync( x => x.Id == reportId );
        }

        
        /// <summary>
        /// Проверка, существует ли доклад.
        /// </summary>
        public async Task<bool> IsExistAsync( Guid id )
        {
            var user = await _db.Reports.FirstOrDefaultAsync( x => x.Id == id );
            return user != null;
        }

        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public async Task<ReportOuterModel> GetAsync( Guid reportId )
        {
            var report = await _db.Reports.FirstOrDefaultAsync( x => x.Id == reportId );
            if( report == null )
            {
                return null;
            }

            report.Collaboratorsreportidfkeys = await GetCollaboratorsAsync( reportId );

            var model = _mapper.Map<ReportOuterModel>( report );
            model.SectionId = (await _sectionRepository.GetByReport( reportId ))?.SectionId ?? Guid.Empty;
            return model;
        }

        /// <summary>
        /// Вернуть доклады для конкретного пользователя.
        /// </summary>
        public async Task<List<ReportOuterModel>> GetReportsByUserAsync( Guid userId )
        {
            var reports = _db.Reports.Where(x => x.UserId == userId).ToList();
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = await GetCollaboratorsAsync( report.Id );
            }

            var model = _mapper.Map<List<ReportOuterModel>>( reports );
            foreach( var item in model )
            {
                item.SectionId = (await _sectionRepository.GetByReport( item.Id ))?.SectionId ?? Guid.Empty;
            }
            
            return model;
        }

        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public async Task<List<ReportOuterModel>> GetAllAsync()
        {
            var reports = _db.Reports.ToList();
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = await GetCollaboratorsAsync( report.Id );
            }
            
            var model = _mapper.Map<List<ReportOuterModel>>( reports );
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
            var users = await GetConfirmedUsersByEmail( collaborators );

            // Добавить их в таблицу.
            foreach( var user in users )
            {
                var collaborator = new Collaborator { UserId = user.Id, ReportId = reportId };
                await _db.InsertAsync( collaborator );
            }
        }


        /// <summary>
        /// Выбрать всех подтвержденных пользователей,
        /// чья почта была указана.
        /// </summary>
        private async Task<List<User>> GetConfirmedUsersByEmail( IEnumerable<string> collaborators )
        {
            var users = await
                (from user in _db.Users
                    where user.UserStatus == UserStatus.Confirmed
                          && collaborators.Contains(user.Email)
                    select user).ToListAsync();
            return users;
        }
        
        
        public List<ReportInnerModel> Get( Func<ReportInnerModel, bool> filter )
        {
            throw new NotImplementedException();
        }

        Task<ReportInnerModel> IRepository<ReportInnerModel>.GetAsync( Guid id )
        {
            throw new NotImplementedException();
        }

        Task<List<ReportInnerModel>> IRepository<ReportInnerModel>.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}