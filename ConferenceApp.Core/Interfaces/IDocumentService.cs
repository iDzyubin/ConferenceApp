using System;
using System.IO;
using ConferenceApp.Core.Services;

namespace ConferenceApp.Core.Interfaces
{
    public interface IDocumentService
    {
        (Guid reportId, string path) InsertFile( Guid requestId, FileStream file );

        FileStatus DeleteFile( Guid requestId, Guid reportId );

        FileStatus UpdateFile( Guid requestId, Guid reportId, FileStream file );

        (MemoryStream, FileStatus) GetFile( Guid requestId, Guid reportId );
    }
}