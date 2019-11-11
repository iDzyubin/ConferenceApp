using System;
using System.Collections.Generic;
using System.IO;
using ConferenceApp.Core.DataModels;

namespace ConferenceApp.Core.Interfaces
{
    public interface IReportRepository : IRepository<Report>
    {
        void Insert(Report report, FileStream file);

        void Update(Report report, FileStream file);
        
        IEnumerable<Report> Get( Func<Report, bool> filter );
    }
}