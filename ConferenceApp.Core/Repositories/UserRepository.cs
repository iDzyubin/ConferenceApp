using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDb _db;

        public UserRepository( MainDb db )
        {
            _db = db;
        }

        public async Task<Guid> InsertAsync( User user )
        {
            user.Id = Guid.NewGuid();
            user.Phone ??= string.Empty;
            await _db.InsertAsync( user );
            return user.Id;
        }

        public async Task UpdateAsync( User user )
        {
            var info = await _db.Users.FirstOrDefaultAsync( x => x.Id == user.Id );
            if( info == null ) return;

            await _db.Users
                .Where( x => x.Id == user.Id )
                .Set(x => x.FirstName, user.FirstName)
                .Set(x => x.MiddleName, user.MiddleName)
                .Set(x => x.LastName, user.LastName)
                .Set(x => x.Position, user.Position)
                .Set(x => x.Organisation, user.Organisation)
                .Set(x => x.OrganisationAddress, user.OrganisationAddress)
                .Set(x => x.City, user.City)
                .Set(x => x.Phone, user.Phone)
                .Set(x => x.StartResidenceDate, user.StartResidenceDate)
                .Set(x => x.EndResidenceDate, user.EndResidenceDate)
                .UpdateAsync();
        }

        
        public async Task DeleteAsync( Guid userId )
            => await _db.Users.DeleteAsync( x => x.Id == userId );

        
        public async Task<User> GetAsync( Guid userId )
            => await _db.Users.FirstOrDefaultAsync( x => 
                    x.Id == userId && 
                    x.UserStatus == UserStatus.Confirmed );

        public async Task<bool> IsExistAsync( Guid userId )
            => await GetAsync( userId ) != null;

        
        public async Task<User> GetByEmailAsync( string email )
            => await _db.Users.FirstOrDefaultAsync(x => 
                x.Email.ToUpper() == email.ToUpper() && 
                x.UserStatus == UserStatus.Confirmed);
        
        
        public async Task<bool> IsExistAsync( string email )
            => await GetByEmailAsync( email ) != null;

        
        public List<User> Get( Func<User, bool> filter )
            => _db.Users.Where( filter ).ToList();

        
        public async Task<List<User>> GetAllAsync()
            => await _db.Users
                .Where(x 
                    => x.UserRole == UserRole.User && 
                       x.UserStatus == UserStatus.Confirmed)
                .ToListAsync();
    }
}