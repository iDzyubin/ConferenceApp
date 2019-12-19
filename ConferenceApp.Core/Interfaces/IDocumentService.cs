using System;
using System.IO;

namespace ConferenceApp.Core.Interfaces
{
    public interface IDocumentService
    {
        void InsertFile( Guid reportId, FileStream file );

        void DeleteFile( Guid reportId );

        MemoryStream GetFile( Guid reportId );
    }
}