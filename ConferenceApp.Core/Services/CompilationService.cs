using System;
using System.IO;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class CompilationService : ICompilationService
    {
        private const string CompilationPath = "Compilation";
        private readonly ICompilationRepository _compilationRepository;

        public CompilationService( ICompilationRepository compilationRepository )
        {
            _compilationRepository = compilationRepository;
        }

        public async Task<Guid> InsertFileAsync( FileStream file )
        {
            var path = await InsertFileOnStorage( file );
            var compilation = new Compilation { Path = path };
            return await _compilationRepository.InsertAsync( compilation );
        }

        private async Task<string> InsertFileOnStorage( FileStream file )
        {
            var path = CompilationPath;
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            var fileName = Path.GetFileName( file.Name );
            path = Path.Combine( path, fileName );

            await using var memoryStream = new MemoryStream();
            file.Position = 0;
            file.CopyTo( memoryStream );
            File.WriteAllBytes( path, memoryStream.ToArray() );

            return path;
        }

        public async Task<(MemoryStream, string)> GetFileAsync( Guid compilationId )
        {
            var compilation = await _compilationRepository.GetAsync( compilationId );

            if( compilation == null || !File.Exists(compilation.Path) )
            {
                return (null, String.Empty);
            }
            
            var memoryStream = new MemoryStream( File.ReadAllBytes( compilation.Path ) );
            return (memoryStream, Path.GetFileName(compilation.Path));
        }
    }
}