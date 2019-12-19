using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper _mapper;
        private readonly MainDb _db;


        public ReportRepository
        (
            IDocumentService documentService,
            IMapper mapper,
            MainDb db
        )
        {
            _documentService = documentService;
            _mapper = mapper;
            _db = db;
        }


        /// <summary>
        /// Добавить доклад (основная информация).
        /// </summary>
        public Guid Insert( ReportModel model )
        {
            var report = _mapper.Map<Report>( model );
            report.Id = Guid.NewGuid();
            report.Path = String.Empty;
            _db.Insert( report );

            var collaboratorIds =
                ( from user in _db.Users
                    where user.UserStatus == UserStatus.Confirmed
                          && model.Collaborators.Contains( user.Email )
                    select user.Id ).ToList();

            foreach( var collaboratorId in collaboratorIds )
            {
                var collaborator = new Collaborator { UserId = collaboratorId, ReportId = report.Id };
                _db.Insert( collaborator );
            }

            return report.Id;
        }

        public Guid Insert( Report item )
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Изменить статус заявки.
        /// </summary>
        public void ChangeStatus( Guid reportId, ReportStatus status )
        {
            _db.Reports
                .Where( x => x.Id == reportId )
                .Set( x => x.Status, status )
                .Update();
        }


        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public void Delete( Guid reportId )
        {
            _documentService
                .DeleteFile( reportId );
            _db.Collaborators
                .Delete( x => x.ReportId == reportId );
            _db.Reports
                .Delete( x => x.Id == reportId );
        }


        public bool IsExist( Guid id )
        {
            return _db.Reports.FirstOrDefault( x => x.Id == id ) != null;
        }


        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public Report Get( Guid reportId )
        {
            var report = _db.Reports.FirstOrDefault( x => x.Id == reportId );
            if( report == null )
            {
                return null;
            }

            report.Collaboratorsreportidfkeys = GetCollaborators( reportId );
            return report;
        }


        /// <summary>
        /// Выдать информацию по фильтру
        /// </summary>
        public IEnumerable<Report> Get( Func<Report, bool> filter )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Report> GetReportsByUser( Guid userId )
        {
            var reports = _db.Reports.Where(x => x.UserId == userId).ToList();
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = GetCollaborators( report.Id );
            }
            return reports;
        }

        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public IEnumerable<Report> GetAll()
        {
            var reports = _db.Reports.ToList();
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = GetCollaborators( report.Id );
            }
            return reports;
        }

        private List<Collaborator> GetCollaborators( Guid reportId )
        {
            var collaborators = 
                from c in _db.Collaborators
                join user in _db.Users on c.UserId equals user.Id
                where c.ReportId == reportId
                select new Collaborator { ReportId = c.ReportId, UserId = c.UserId, User = user };
            
            return collaborators.ToList();
        }
    }
}