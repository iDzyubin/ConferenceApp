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
    public class DocumentService : IDocumentService
    {
        private readonly MainDb _db;


        public DocumentService( MainDb db )
        {
            _db = db;
        }


        /// <summary>
        /// Добавление доклада на диск.
        /// </summary>
        public async Task InsertFileAsync( Guid reportId, FileStream fileStream )
        {
            var report = await GetReportAsync( reportId );

            var path = GetPath();
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            path = Path.Combine( path, report.UserId.ToString() );
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            report.Path = await InsertFileAsync( reportId, fileStream, path );
            _db.Update( report );
        }

        private static async Task<string> InsertFileAsync( Guid reportId, FileStream fileStream, string path )
        {
            await using var memoryStream = new MemoryStream();
            fileStream.Position = 0;
            fileStream.CopyTo( memoryStream );

            var fileName = $"{reportId}{Path.GetExtension( fileStream.Name )}";
            path = Path.Combine( path, fileName );
            File.WriteAllBytes( path, memoryStream.ToArray() );
            return path;
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
        public async Task<MemoryStream> GetFileAsync( Guid reportId )
        {
            var report = await GetReportAsync( reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return null;
            }

            var memoryStream = new MemoryStream( File.ReadAllBytes( report.Path ) );
            return memoryStream;
        }


        /// <summary>
        /// Путь до директории.
        /// </summary>
        private string GetPath() => "Files";

        
        private async Task<Report> GetReportAsync(Guid reportId) 
            => await _db.Reports.FirstOrDefaultAsync(x => x.Id == reportId);
    }
}