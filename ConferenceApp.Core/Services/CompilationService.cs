using System;
using System.IO;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class CompilationService : FileService
    {
        protected override string StoragePath { get; } = "Compilation";
        private readonly ICompilationRepository _compilationRepository;

        
        public CompilationService( ICompilationRepository compilationRepository )
        {
            _compilationRepository = compilationRepository;
        }

        
        /// <summary>
        /// Добавить сборник на диск.
        /// </summary>
        public async Task<Guid> InsertFileAsync( FileStream file )
        {
            var path = await InsertFileOnStorage( file );
            var compilation = new Compilation { Path = path };
            return await _compilationRepository.InsertAsync( compilation );
        }


        /// <summary>
        /// Получить сборник с диска.
        /// </summary>
        public async Task<(MemoryStream, string)> GetFileAsync( Guid compilationId )
        {
            var compilation = await _compilationRepository.GetAsync( compilationId );
            if( compilation == null )
            {
                return (null, string.Empty);
            }
            return await base.GetFileAsync(compilation.Path);
        }
        
        
        private async Task<string> InsertFileOnStorage( FileStream file )
        {
            var path = GetPath(file);
            await WriteFile(file, path);
            return path;
        }


        private string GetPath(FileStream file)
        {
            var path = StoragePath;
            if( !Directory.Exists( path ) )
            {
                Directory.CreateDirectory( path );
            }

            var fileName = Path.GetFileName( file.Name );
            path = Path.Combine( path, fileName );
            return path;
        }
    }
}