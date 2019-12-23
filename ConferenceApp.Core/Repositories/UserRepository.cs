using System;
using System.Collections.Generic;
using System.Linq;
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

        public Guid Insert( User user )
        {
            user.Id = Guid.NewGuid();
            user.Phone ??= string.Empty;
            
            user.ConfirmCode = Guid.NewGuid().ToString( "N" );
            _db.Insert( user );
            
            var confirmUrl = $"/Account/Confirm?code={user.ConfirmCode}";
            _notificationService.SendAccountConfirmation( user.Email, confirmUrl );
            
            return user.Id;
        }

        public void Update( User user )
        {
            var info = _db.Users.FirstOrDefault( x => x.Id == user.Id );
            if( info == null ) return;

            user.Password   = info.Password;
            user.UserStatus = info.UserStatus;
            _db.Update( user );
        }
        
        public void Delete( Guid userId ) => _db.Users.Delete( x => x.Id == userId );

        public User Get( Guid userId )
        {
            var user = _db.Users.FirstOrDefault( x => x.Id == userId );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return null;
            }
            return user;
        }

        public bool IsExist( Guid userId )
        {
            var user = _db.Users.FirstOrDefault( x => x.Id == userId );
            if( user == null || user.UserStatus == UserStatus.Unconfirmed )
            {
                return false;
            }
            return true;
        }

        public IEnumerable<User> Get( Func<User, bool> filter ) => _db.Users.Where( filter ).ToList();

        public IEnumerable<User> GetAll() => _db.Users.AsEnumerable();

        public void ChangeStatus( Guid reportId, UserStatus status )
            => _db.Users
                .Where(x => x.Id == reportId)
                .Set(x => x.UserStatus, status)
                .Update();

        public User GetByEmail( string email )
            => _db.Users.FirstOrDefault(x => x.Email == email);
    }
}