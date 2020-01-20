using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class CompilationRepository : ICompilationRepository
    {
        private readonly MainDb _db;

        public CompilationRepository( MainDb db ) => _db = db;

        public async Task<Guid> InsertAsync( Compilation item )
        {
            item.Id = Guid.NewGuid();
            await _db.InsertAsync( item );
            return item.Id;
        }
        
        public async Task<Compilation> GetAsync( Guid id )
            => await _db.Compilations.FirstOrDefaultAsync( x => x.Id == id );

        public async Task<List<Compilation>> GetAllAsync()
            => await _db.Compilations.ToListAsync();

        public async Task<bool> IsExistAsync( Guid id )
            => await GetAsync( id ) != null;
        
        public Task UpdateAsync( Compilation item ) => throw new NotImplementedException();

        public Task DeleteAsync( Guid id ) => throw new NotImplementedException();
        
        public List<Compilation> Get( Func<Compilation, bool> filter ) => throw new NotImplementedException();
    }
}