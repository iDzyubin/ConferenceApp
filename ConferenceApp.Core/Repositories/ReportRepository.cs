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
            // 1. Добавить запись о докладе.
            _db.Insert( report );

            // 2. Добавить информацию о соавторах.
            foreach( var collaborator in report.Collaboratorsreportidfkeys )
            {
                _db.Insert( collaborator );
            }

            // 3. Добавить файл на диск.
            _documentService.InsertFile( report.RequestId, report.Id, file );
        }

        
        public void Insert( Report report )
        {
            throw new NotImplementedException();
        }
        
        
        /// <summary>
        /// Обновить информацию по докладу.
        /// </summary>
        public void Update( Report report, FileStream file )
        {
            // 1. Обновить информацию в БД.
            _db.Update( report );

            // 2. Обновить информацию о соавторах.
            foreach( var collaborator in report.Collaboratorsreportidfkeys )
            {
                _db.Update( collaborator );
            }

            // 3. Обновить информацию на файловой системе.
            _documentService.UpdateFile( report.RequestId, report.Id, file );
        }

        
        public void Update( Report report )
        {
            throw new NotImplementedException();
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
            // TODO.
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


        /// <summary>
        /// Выдать информацию по докладам с учетом фильтра.
        /// </summary>
        public IEnumerable<Report> Get( Func<Report, bool> filter )
        {
            // 1. Получить доклады по условию.
            var reports = _db.Reports.Where( filter ).ToArray();
            if( reports.Length == 0 )
            {
                return new List<Report>();
            }
            
            // 2. Получить соавторов докладов.
            foreach( var report in reports )
            {
                report.Collaboratorsreportidfkeys = _db.Collaborators
                        .Where( x => x.ReportId == report.Id )
                        .AsEnumerable();
            }
            
            // 3. Получить файлы докладов.
            // TODO.
            var files = _documentService.GetFilesByRequest( reports.First().RequestId );

            throw new NotImplementedException();
        }
    }
}