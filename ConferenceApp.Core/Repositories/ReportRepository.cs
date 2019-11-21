using System;
using System.Collections.Generic;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly IDocumentService _documentService;
        private readonly MainDb _db;


        public ReportRepository
        (
            ICollaboratorRepository collaboratorRepository, 
            IDocumentService documentService, 
            MainDb db
        )
        {
            _collaboratorRepository = collaboratorRepository;
            _documentService = documentService;
            _db = db;
        }


        /// <summary>
        /// Добавить доклад.
        /// </summary>
        public void Insert(ReportModel model)
        {
            // 1. Добавить файл.
            var file = model.File;
            var (reportId, path) = _documentService.InsertFile( model.RequestId, file );
            
            // 2. Добавить заявку.
            var report = new Report
            {
                Id         = reportId,
                Title      = model.Title,
                RequestId  = model.RequestId,
                Path       = path,
                ReportType = model.ReportType,
            };
            _db.Insert( report );
            
            // 2. Добавить соавторов.
            var collaborators = model.Collaborators;
            _collaboratorRepository.InsertRange(reportId, collaborators);
        }


        /// <summary>
        /// Добавить коллекцию докладов.
        /// </summary>
        public void InsertRange(IEnumerable<ReportModel> reports)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Обновить информацию по докладу.
        /// </summary>
        public void Update(ReportModel report)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public void Delete(Guid reportId)
        {
            var report = Get( reportId );

            // 1. Удалить файл.
            _documentService.DeleteFile( report.RequestId, reportId );

            // 2. Удалить соавторов.
            _collaboratorRepository.DeleteByReport( reportId );

            // 3. Удалить заявку.
            _db.Reports.Delete( x => x.Id == reportId );
        }


        /// <summary>
        /// Удалить коллекцию докладов.
        /// </summary>
        public void DeleteRange(Guid requestId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public ReportModel Get( Guid reportId )
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Вернуть доклады по фильтру.
        /// </summary>
        public IEnumerable<ReportModel> Get(Func<ReportModel, bool> filter)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public IEnumerable<ReportModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}