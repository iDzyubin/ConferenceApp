using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Services
{
    /// <summary>
    /// Сервис для работы с документами на файловой системе.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly MainDb _db;


        public DocumentService( MainDb db ) => _db = db;

        /// <summary>
        /// Добавление доклада на диск.
        /// </summary>
        public (FileStatus status, string path) InsertFile( Guid requestId, Guid reportId, FileStream fileStream )
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

            path = Path.Combine( path, reportId.ToString() );
            File.WriteAllBytes( path, memoryStream.ToArray() );

            return ( FileStatus.Success, path );
        }


        /// <summary>
        /// Удалить доклад с диска.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="reportId"></param>
        public FileStatus DeleteFile( Guid requestId, Guid reportId )
        {
            var report = GetReport( requestId, reportId );
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
        /// <param name="requestId"></param>
        /// <param name="reportId"></param>
        /// <param name="file"></param>
        public FileStatus UpdateFile( Guid requestId, Guid reportId, FileStream file )
        {
            var report = GetReport( requestId, reportId );
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
            var report = GetReport( requestId, reportId );
            if( report == null || !File.Exists( report.Path ) )
            {
                return ( null, FileStatus.NotFoundFile );
            }

            var memoryStream = new MemoryStream( File.ReadAllBytes( report.Path ) );
            return ( memoryStream, FileStatus.Success );
        }


        // TODO. Test it.
        /// <summary>
        /// Получить все доклады по заявке.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public (IEnumerable<ReportFile>, FileStatus) GetFilesByRequest( Guid requestId )
        {
            var path = Path.Combine( GetPath(), requestId.ToString() );
            if( !Directory.Exists( path ) )
            {
                return ( null, FileStatus.NotFoundDirectory );
            }

            var filePaths = Directory.GetFiles( path );
            var files = new List<ReportFile>();
            Parallel.ForEach( filePaths, filePath =>
                {
                    using var memoryStream = new MemoryStream( File.ReadAllBytes( filePath ) );
                    files.Add( new ReportFile
                        {
                            ReportId = Guid.Parse( Path.GetFileName( filePath ) ), File     = memoryStream
                        }
                    );
                }
            );

            return ( files, FileStatus.Success );
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


        /// <summary>
        /// Выдать доклад по id.
        /// </summary>
        private Report GetReport( Guid requestId, Guid reportId )
        {
            var report = _db.Reports.FirstOrDefault( x =>
                x.Id == reportId &&
                x.RequestId == requestId
            );
            return report;
        }
    }

    public enum FileStatus
    {
        NotFoundDirectory,
        NotFoundFile,
        Success
    }
}