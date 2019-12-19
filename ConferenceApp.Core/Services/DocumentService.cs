using System;
using System.IO;
using System.Linq;
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
        public void InsertFile( Guid reportId, FileStream fileStream )
        {
            var report = GetReport( reportId );

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

            report.Path = InsertFile( reportId, fileStream, path );
            _db.Update( report );
        }

        private static string InsertFile( Guid reportId, FileStream fileStream, string path )
        {
            using var memoryStream = new MemoryStream();
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
        public void DeleteFile( Guid reportId )
        {
            var report = GetReport( reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return;
            }

            File.Delete( report.Path );
        }


        /// <summary>
        /// Получить доклад с диска.
        /// </summary>
        public MemoryStream GetFile( Guid reportId )
        {
            var report = GetReport( reportId );
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

        
        private Report GetReport(Guid reportId) 
            => _db.Reports.FirstOrDefault(x => x.Id == reportId);
    }
}