using System;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Services
{
    public class UserService
    {
        private readonly MainDb _db;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHashService _passwordHashService;

        
        public UserService
        ( 
            MainDb db,
            IUserRepository userRepository, 
            PasswordHashService passwordHashService 
        )
        {
            _db = db;
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        
        /// <summary>
        /// Попытка войти в систему.
        /// </summary>
        public (User user, bool result) TryToSignInAsync( string email, string password )
        {
            var user = _userRepository.Get(x => x.Email.ToUpper() == email.ToUpper()).FirstOrDefault();
            if( user == null || !_passwordHashService.ValidateHash(password, user.Password) )
            {
                return (null, false);
            }
            return (user, true);
        }

        
        /// <summary>
        /// Подтверждение аккаунта.
        /// </summary>
        public async Task ConfirmAccountAsync( string code )
        {
            var user = _userRepository.Get( u => u.ConfirmCode == code ).FirstOrDefault();
            if( user == null ) throw new Exception( "Ссылка недействительна" );

            switch( user.UserStatus )
            {
                case UserStatus.Unconfirmed:
                    await Confirm( user.Id );
                    break;
                case UserStatus.Confirmed:
                    throw new Exception( "Аккаунт был подтвержден ранее" );
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task Confirm(Guid userId)
        {
            await _db.Users
                .Where( x => x.Id == userId )
                .Set( x => x.UserStatus, UserStatus.Confirmed )
                .UpdateAsync();
        }
    }
}