using System;
using System.IO;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Interfaces
{
    public interface IDocumentService
    {
        Task InsertFileAsync( Guid reportId, FileStream file );

        Task DeleteFileAsync( Guid reportId );

        Task<MemoryStream> GetFileAsync( Guid reportId );
    }
}