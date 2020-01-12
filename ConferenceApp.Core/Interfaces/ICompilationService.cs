using System;
using System.IO;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Interfaces
{
    public interface ICompilationService
    {
        Task<Guid> InsertFileAsync( FileStream file );
        
        Task<(MemoryStream, string)> GetFileAsync( Guid compilationId );
    }
}