using System;
using System.IO;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

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
        public (Guid reportId, string path) InsertFile( Guid requestId, FileStream fileStream )
        {
            var path = GetPath();
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            path = Path.Combine( path, requestId.ToString() );
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            using var memoryStream = new MemoryStream();
            fileStream.Position = 0;
            fileStream.CopyTo( memoryStream );

            var reportId = Guid.NewGuid();
            path = Path.Combine( path, reportId.ToString() );
            File.WriteAllBytes( path, memoryStream.ToArray() );

            return ( reportId, path );
        }


        /// <summary>
        /// Удалить доклад с диска.
        /// </summary>
        public FileStatus DeleteFile( Guid requestId, Guid reportId )
        {
            var report = GetReport( reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return FileStatus.NotFoundFile;
            }

            File.Delete( report.Path );
            return FileStatus.Success;
        }


        /// <summary>
        /// Обновить доклад на диске.
        /// </summary>
        public FileStatus UpdateFile( Guid requestId, Guid reportId, FileStream file )
        {
            var report = GetReport( reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return FileStatus.NotFoundFile;
            }

            using var memoryStream = new MemoryStream();
            file.CopyTo( memoryStream );
            File.WriteAllBytes( report.Path, memoryStream.ToArray() );

            return FileStatus.Success;
        }


        /// <summary>
        /// Получить доклад с диска.
        /// </summary>
        public (MemoryStream, FileStatus) GetFile( Guid requestId, Guid reportId )
        {
            var report = GetReport( reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return ( null, FileStatus.NotFoundFile );
            }

            var memoryStream = new MemoryStream( File.ReadAllBytes( report.Path ) );
            return ( memoryStream, FileStatus.Success );
        }


        /// <summary>
        /// Путь до директории.
        /// </summary>
        private string GetPath()
        {
            var path = Directory.GetParent( Directory.GetCurrentDirectory() ).Name;
            path = Path.Combine( path, "ConferenceApp.Core" );
            path = Path.Combine( path, "Files" );
            return path;
        }

        
        private Report GetReport(Guid reportId)
        {
            var report = _db.Reports.FirstOrDefault(x => x.Id == reportId);
            return report;
        }
    }

    public enum FileStatus
    {
        NotFoundFile,
        Success
    }
}