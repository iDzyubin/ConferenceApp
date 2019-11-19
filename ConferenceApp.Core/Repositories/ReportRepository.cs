using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly MainDb _db;


        public ReportRepository( MainDb db )
            => _db = db;


        /// <summary>
        /// Добавить доклад.
        /// </summary>
        public void Insert( Report report )
            => _db.Insert( report );

        
        /// <summary>
        /// Обновить информацию по докладу.
        /// </summary>
        public void Update( Report report )
            => _db.Update( report );


        /// <summary>
        /// Удалить доклад.
        /// </summary>
        public void Delete( Guid reportId )
            => _db.Reports.Delete( x => x.Id == reportId );


        /// <summary>
        /// Выдать информацию по докладу.
        /// </summary>
        public Report Get( Guid reportId )
            => _db.Reports.FirstOrDefault( x => x.Id == reportId );


        /// <summary>
        /// Вернуть доклады по фильтру.
        /// </summary>
        public IEnumerable<Report> Get( Func<Report, bool> filter )
            => _db.Reports.Where( filter ).AsEnumerable();


        /// <summary>
        /// Выдать информацию по всем докладам.
        /// </summary>
        public IEnumerable<Report> GetAll()
            => _db.Reports.AsEnumerable();
    }
}