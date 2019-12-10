using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Extensions;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDocumentService _documentService;
        private readonly MainDb _db;


        public ReportRepository
        (
            IDocumentService documentService,
            MainDb db
        )
        {
            _documentService = documentService;
            _db = db;
        }


        /// <summary>
        /// Добавить доклад.
        /// </summary>
        public void Insert( ReportModel model )
        {
            // 1. Добавить файл.
            var file = model.File;
            var (reportId, path) = _documentService.InsertFile(model.RequestId, file);

            // 2. Добавить заявку.
            var report = new Report
            {
                Id = reportId,
                Title = model.Title,
                RequestId = model.RequestId,
                Path = path,
                ReportType = model.ReportType,
                Collaborators = model.Collaborators
            };
            _db.Insert(report);
        }


        /// <summary>
        /// Добавить коллекцию докладов.
        /// </summary>
        public void InsertRange( List<ReportModel> reports )
            => reports.ForEach(Insert);


        public void ChangeStatus( Guid reportId, ReportStatus status )
            => _db.Reports
                .Where(x => x.Id == reportId)
                .Set(x => x.Status, status)
                .Update();


        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public void Delete( Guid reportId )
        {
            var report = Get(reportId);

            // 1. Удалить файл.
            _documentService.DeleteFile(report.RequestId, reportId);

            // 2. Удалить заявку.
            _db.Reports.Delete(x => x.Id == reportId);
        }


        /// <summary>
        /// Удалить коллекцию докладов.
        /// </summary>
        public void DeleteRange( Guid requestId )
        {
            var reportIds = _db.Reports
                .Where(x => x.RequestId == requestId)
                .Select(x => x.Id)
                .ToList();
            reportIds.ForEach(Delete);
        }


        public IEnumerable<ReportModel> GetReportsByRequest( Guid requestId )
        {
            var reports = _db.Reports.Where(x => x.RequestId == requestId).AsEnumerable();

            var model = reports.Select(report => new ReportModel
                {
                    Title = report.Title,
                    ReportId = report.Id,
                    RequestId = report.RequestId,
                    ReportStatus = report.Status,
                    ReportType = report.ReportType,
                    Collaborators = report.Collaborators
                })
                .ToList();

            return model;
        }

        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public ReportModel Get( Guid reportId )
            => _db.Reports
                .FirstOrDefault(x => x.Id == reportId)?
                .ConvertToReportModel();


        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public IEnumerable<ReportModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}