using System;
using System.IO;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Services
{
    /// <summary>
    /// Сервис для работы с документами на файловой системе.
    /// </summary>
    public class DocumentService : FileService
    {
        protected override string StoragePath { get; } = "Files";
        private readonly MainDb _db;

        
        public DocumentService( MainDb db )
        {
            _db = db;
        }


        /// <summary>
        /// Добавление доклада на диск.
        /// </summary>
        public async Task InsertFileAsync( Guid reportId, FileStream file )
        {
            var report = await GetReportAsync( reportId );
            report.Path = await InsertFileOnStorage(report, file);
            _db.Update( report );
        }

        
        /// <summary>
        /// Удалить доклад с диска.
        /// </summary>
        public async Task DeleteFileAsync( Guid reportId )
        {
            var report = await GetReportAsync( reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return;
            }
            File.Delete( report.Path );
        }


        /// <summary>
        /// Получить доклад с диска.
        /// </summary>
        public async Task<(MemoryStream, string)> GetFileAsync( Guid reportId )
        {
            var report = await GetReportAsync( reportId );
            if( report == null )
            {
                return (null, string.Empty);
            }
            return await base.GetFileAsync(report.Path);
        }
        
        
        private async Task<string> InsertFileOnStorage( Report report, FileStream file )
        {
            var path = GetPath(report, file);            
            await WriteFile(file, path);
            return path;
        }


        private string GetPath( Report report, FileStream file )
        {
            var path = StoragePath;
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            path = Path.Combine( path, report.UserId.ToString() );
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            var fileName = $"{report.Id}{Path.GetExtension( file.Name )}";
            path = Path.Combine( path, fileName );
            return path;
        }
        
        
        private async Task<Report> GetReportAsync(Guid reportId) 
            => await _db.Reports.FirstOrDefaultAsync(x => x.Id == reportId);
    }
}