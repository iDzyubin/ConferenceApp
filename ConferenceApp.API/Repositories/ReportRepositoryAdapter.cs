using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.Models;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.API.Repositories
{
    public class ReportRepositoryAdapter : IReportRepositoryAdapter
    {
        private readonly IReportRepository _reportRepository;
        private readonly IDocumentService _documentService;
        private readonly ICollaboratorRepository _collaboratorRepository;


        public ReportRepositoryAdapter
        (
            IReportRepository reportRepository,
            IDocumentService documentService,
            ICollaboratorRepository collaboratorRepository
        )
        {
            _reportRepository = reportRepository;
            _documentService = documentService;
            _collaboratorRepository = collaboratorRepository;
        }


        public void Insert( ReportModel model )
        {
            var file = model.File;
            var collaborators = model.Collaborators;

            // 1. Добавить файл.
            var fileStream = file.ConvertToFileStream();
            var (reportId, path) = _documentService.InsertFile( model.RequestId, fileStream );

            // 2. Добавить заявку.
            var report = new Report
            {
                Id         = reportId,
                Title      = model.Title,
                RequestId  = model.RequestId,
                Path       = path,
                ReportType = model.ReportType,
            };
            _reportRepository.Insert( report );

            // 2. Добавить соавторов.
            _collaboratorRepository.InsertRange(reportId, collaborators);
        }


        public void InsertRange( IEnumerable<ReportModel> reports )
        {
            foreach( var report in reports )
            {
                Insert( report );
            }
        }


        public void Delete( Guid id )
        {
            var report = _reportRepository.Get( id );

            // 1. Удалить файл.
            _documentService.DeleteFile( report.RequestId, report.Id );

            // 2. Удалить соавторов.
            _collaboratorRepository.DeleteByReport( report.Id );

            // 3. Удалить заявку.
            _reportRepository.Delete( report.Id );
        }


        public void DeleteRange( Guid requestId )
        {
            var reports = _reportRepository.Get( x => x.RequestId == requestId );
            foreach( var report in reports )
            {
                Delete( report.Id );
            }
        }


        public void Update( ReportModel model )
        {
            var file = model.File;
            var report = _reportRepository.Get( model.ReportId );

            // 1. Обновить файл.
            var fileStream = file.ConvertToFileStream();
            _documentService.UpdateFile( report.RequestId, report.Id, fileStream );

            // 2. Обновить информацию по докладу.
            report = new Report
            {
                Id         = report.Id,
                RequestId  = report.RequestId,
                Path       = report.Path,
                Status     = report.Status,
                Title      = model.Title,
                ReportType = model.ReportType
            };
            _reportRepository.Update( report );
        }


        public ReportModel Get( Guid id )
        {
            var report = _reportRepository.Get( id );
            var collaborators = _collaboratorRepository
                .Get( x => x.ReportId == id )
                .ToList();

            return new ReportModel
            {
                ReportId      = report.Id,
                Title         = report.Title,
                ReportStatus  = report.Status,
                ReportType    = report.ReportType,
                Collaborators = collaborators
            };
        }


        public IEnumerable<ReportModel> Get( Func<ReportModel, bool> filter )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ReportModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}