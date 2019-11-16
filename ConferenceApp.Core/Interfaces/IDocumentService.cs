using System;
using System.Collections.Generic;
using System.IO;
using ConferenceApp.Core.Models;
using ConferenceApp.Core.Services;

namespace ConferenceApp.Core.Interfaces
{
    public interface IDocumentService
    {
        (FileStatus status, string path) InsertFile( Guid requestId, Guid reportId, FileStream file );

        FileStatus DeleteFile( Guid requestId, Guid reportId );

        FileStatus UpdateFile( Guid requestId, Guid reportId, FileStream file );

        (MemoryStream, FileStatus) GetFile( Guid requestId, Guid reportId );

        (IEnumerable<ReportFile>, FileStatus) GetFilesByRequest( Guid requestId );
    }
}