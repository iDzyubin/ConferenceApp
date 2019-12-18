using System;
using System.IO;
using ConferenceApp.Core.Services;

namespace ConferenceApp.Core.Interfaces
{
    public interface IDocumentService
    {
        (Guid reportId, string path) InsertFile( Guid requestId, FileStream file );

        void DeleteFile( Guid requestId, Guid reportId );

        MemoryStream GetFile( Guid requestId, Guid reportId );
    }
}