using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDocumentService _documentService;
        private readonly MainDb _db;


        public ReportRepository( IDocumentService documentService, MainDb db )
        {
            _documentService = documentService;
            _db = db;
        }


        /// <summary>
        /// Добавить доклад.
        /// </summary>
        public void Insert( Report report, FileStream file )
        {
            report.Id       = Guid.NewGuid();
            report.FileName = report.Id.ToString();
            
            // 1. Добавить файл на диск.
            var (status, path) = _documentService.InsertFile( report.RequestId, report.Id, file );
            if( status != FileStatus.Success )
            {
                return;
            }

            report.Status = ReportStatus.None;
            report.Path   = path;

            // 2. Добавить запись о докладе.
            Insert( report );

            // 3. Добавить информацию о соавторах.
            foreach( var collaborator in report.Collaboratorsreportidfkeys )
            {
                collaborator.Id = Guid.NewGuid();
                _db.Insert( collaborator );
            }
        }


        public void Insert( Report report )
        {
            if( report.Id == Guid.Empty )
            {
                report.Id = Guid.NewGuid();
            }
            _db.Insert( report );
        }


        /// <summary>
        /// Обновить информацию по докладу.
        /// </summary>
        public void Update( Report report, FileStream file )
        {
            if( file != null )
            {
                // 1. Обновить информацию на файловой системе.
                _documentService.UpdateFile( report.RequestId, report.Id, file );
            }
            
            // 2. Обновить информацию в БД.
            Update( report );

            // 3. Обновить информацию о соавторах.
            foreach( var collaborator in report.Collaboratorsreportidfkeys )
            {
                _db.Update( collaborator );
            }
        }


        public void Update( Report report )
        {
            _db.Update( report );
        }


        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public void Delete( Guid id )
        {
            var report = _db.Reports.FirstOrDefault( x => x.Id == id );
            if( report == null )
            {
                return;
            }

            // 1. Удаление записей о соавторах.
            _db.Collaborators.Delete( x => x.ReportId == id );

            // 2. Удаление файла с диска.
            _documentService.DeleteFile( report.RequestId, report.Id );

            // 3. Удаление записи о докладе.
            _db.Reports.Delete( x => x.Id == id && x.RequestId == report.RequestId );
        }


        // TODO.
        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public Report Get( Guid id )
        {
            // 1. Получить запись о докладе.
            var report = _db.Reports.FirstOrDefault( x => x.Id == id );
            if( report == null )
            {
                return null;
            }

            // 2. Прочитать информацию о соавторах.
            var collaborators = _db.Collaborators.Where( x => x.ReportId == id ).AsEnumerable();

            // 3. Прочитать файл с диска.
            _documentService.GetFile( report.RequestId, report.Id );

            // 4. Объединение данных.
            report.Collaboratorsreportidfkeys = collaborators;

            return report;
        }


        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public IEnumerable<Report> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}