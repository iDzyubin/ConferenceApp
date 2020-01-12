using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Services;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NotificationService _notificationService;
        private readonly MainDb _db;

        public UserRepository( MainDb db, NotificationService notificationService )
        {
            _notificationService = notificationService;
            _db = db;
        }

        public async Task<Guid> InsertAsync( User user )
        {
            user.Id = Guid.NewGuid();
            user.Phone ??= string.Empty;
            
            user.ConfirmCode = Guid.NewGuid().ToString( "N" );
            await _db.InsertAsync( user );
            
            var confirmUrl = $"/Account/Confirm?code={user.ConfirmCode}";
            await _notificationService.SendAccountConfirmationAsync( user.Email, confirmUrl );
            
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

        public async Task Confirm( Guid userId )
        {
            await _db.Users
                .Where( x => x.Id == userId )
                .Set( x => x.UserStatus, UserStatus.Confirmed )
                .UpdateAsync();
        }

        public async Task DeleteAsync( Guid userId )
        {
            await _db.Users.DeleteAsync( x => x.Id == userId );
        }

        public async Task<User> GetAsync( Guid userId )
        {
            var user = await _db.Users.FirstOrDefaultAsync( x => x.Id == userId );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return null;
            }
            return user;
        }

        public async Task<bool> IsExistAsync( Guid userId )
        {
            var user = await _db.Users.FirstOrDefaultAsync( x => x.Id == userId );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return false;
            }
            return true;
        }

        public List<User> Get( Func<User, bool> filter )
        {
            var users = _db.Users.Where( filter );
            return users.ToList();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task ChangeStatusAsync( Guid reportId, UserStatus status )
            => await _db.Users
                .Where(x => x.Id == reportId)
                .Set(x => x.UserStatus, status)
                .UpdateAsync();

        public async Task<User> GetByEmailAsync( string email )
            => await _db.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper());
    }
}