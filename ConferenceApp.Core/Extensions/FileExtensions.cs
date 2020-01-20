using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ConferenceApp.Core.Extensions
{
    public static class FileExtensions
    {
        public static FileStream ConvertToFileStream( this IFormFile file )
        {
            var fileStream = new FileStream( file.FileName, FileMode.Create );
            file.CopyTo( fileStream );
            return fileStream;
        }
    }
}