using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly MainDb _db;

        public SessionRepository( MainDb db )
        {
            _db = db;
        }
        
        public async Task<Guid> InsertAsync( Session item )
        {
            item.Id = Guid.NewGuid();
            await _db.InsertAsync( item );
            return item.Id;
        }

        public async Task UpdateAsync( Session item )
        {
            var session = await _db.Sessions.FirstOrDefaultAsync( x => x.Id == item.Id );
            if( session == null ) return;

            await _db.UpdateAsync( item );
        }

        public async Task DeleteAsync( Guid id )
        {
            await _db.Sessions.DeleteAsync( x => x.Id == id );
        }
        
        public async Task<Session> GetAsync( Guid id )
        {
            return await _db.Sessions.FirstOrDefaultAsync( x => x.Id == id );
        }

        public List<Session> Get( Func<Session, bool> filter )
        {
            return _db.Sessions.Where( filter ).ToList();
        }

        public async Task<List<Session>> GetAllAsync()
        {
            return await _db.Sessions.ToListAsync();
        }

        public async Task<bool> IsExistAsync( Guid id )
        {
            return await _db.Sessions.FirstOrDefaultAsync( x => x.Id == id ) != null;
        }
    }
}