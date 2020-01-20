using System;
using System.IO;
using System.Threading.Tasks;

namespace ConferenceApp.Core.Services
{
    public class FileService
    {
        protected virtual string StoragePath { get; }
        
        protected async Task<(MemoryStream, string)> GetFileAsync( string path )
        {
            if( !File.Exists(path) )
            {
                return (null, String.Empty);
            }
            
            var memoryStream = new MemoryStream( await File.ReadAllBytesAsync( path ) );
            return (memoryStream, Path.GetFileName(path));
        }

        protected async Task WriteFile(FileStream file, string path)
        {
            await using var memoryStream = new MemoryStream();
            file.Position = 0;
            file.CopyTo( memoryStream );
            File.WriteAllBytes( path, memoryStream.ToArray() );
        }
    }
}